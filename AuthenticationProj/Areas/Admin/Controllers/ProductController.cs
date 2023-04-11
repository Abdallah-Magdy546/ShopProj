using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using NuGet.Versioning;

namespace ShopProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductInterface _product;
        private readonly ICategoryInterface _category;
        private readonly ISubCategoryInterface _subCategory;

        public ProductController(IProductInterface product , ICategoryInterface Category , ISubCategoryInterface subCategory)
        {
            _product = product;
            _category = Category;
            _subCategory = subCategory;
        }

        [Authorize(Permissions.Products.View)]
        public async Task<IActionResult> Index()
        {
            var Catgeories = await _category.GetAllCategories();
            return View(Catgeories);
        }
        [Authorize(Permissions.Products.View)]
        public async Task<IActionResult> SubCategory(int id)
        {
            var result = await _subCategory.GetAllSubCategoriesByCategoryId(id);
            return View(result);
        }
        [Authorize(Permissions.Products.View)]
        public async Task<IActionResult> Products (int id, string sortOrder, string searchString, int? pageNumber, string currentFilter)
        {
            var products = await _product.GetAllProductsBySubCategoryId(id);
            var SubCategory = await _subCategory.GetSubCategoryById(id);
            ViewBag.SubCategoryName = SubCategory.Name;
            ViewBag.SubCategoryId = id;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["BrandSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Brand" : "";
            ViewData["ModelSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Model" : "";
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Id-Desc" : "";
            ViewData["PriceSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Price" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(prd => prd.name.ToUpper().Contains(searchString.ToUpper()) || prd.model.ToUpper().Contains(searchString.ToUpper()) ||
                prd.brand.ToUpper().Contains(searchString.ToUpper()) || prd.SellerName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Id-Desc":
                    products = products.OrderByDescending(s => s.id);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.price);
                    break;
                case "name":
                    products = products.OrderBy(s => s.name);
                    break;
                case "Brand":
                    products = products.OrderBy(s => s.brand);
                    break;
                case "Model":
                    products = products.OrderBy(s => s.model);
                    break;
                default:
                    products = products.OrderBy(s => s.id);
                    break;
            }

            int PageSize = 10;
            return View(PaginatedList<Product>.CreateAsync(products, pageNumber ?? 1, PageSize));
        }
        [Authorize(Permissions.Products.View)]
        public async Task<IActionResult> Details (int id )
        {
            var product = await _product.GetProductById(id);
            var SubCategory = await _subCategory.GetSubCategoryById(product.SubCategoryId);
            ViewBag.SubCategoryName = SubCategory.Name;
            return View(product);   
        }
        [Authorize(Permissions.Products.Create)]
        public async Task<IActionResult> Create (int SubCategoryId)
        {
            var SubCategory = await _subCategory.GetSubCategoryById(SubCategoryId);
            ViewBag.SubCategoryName = SubCategory.Name;
            ViewBag.SubCategoryId=SubCategoryId;
            ViewBag.SubCategoryiesNames = await _subCategory.GetAllSubCategoriesNamesByCategoryId(SubCategory.CategoryId);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Products.Create)]
        public async Task<IActionResult> Create (string name , string model , string brand , float price,
            string SellerName , DateTime ProductionDate , string SubCategoryName)
        {
            var SubCategory = await _subCategory.GetSubCategoryByName(SubCategoryName);
            var product = new Product
            {
                name = name,
                brand = brand,
                model = model,
                price = price,
                SellerName = SellerName,
                ProductionDate = ProductionDate,
                SubCategoryId = SubCategory.id
            };
            var result = await _product.AddProduct(product);
            if ( result==true)
            {
                return RedirectToAction("Products" , new { id = SubCategory.id});
            }
            return BadRequest();                  
        }
        [Authorize(Permissions.Products.Edit)]
        public async Task<IActionResult> Edit (int id)
        {
            var product = await _product.GetProductById(id);
            var SubCategory = await _subCategory.GetSubCategoryById(product.SubCategoryId);
            ViewBag.SubCategoryName = SubCategory.Name;
            ViewBag.AllSubCategoriesNames = await _subCategory.GetAllSubCategoriesNamesByCategoryId(SubCategory.CategoryId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Products.Edit)]
        public async Task<IActionResult> Edit(string name , string brand , string model , float price ,
            string SellerName , DateTime ProductionDate , int id , int SubCategoryId , string SubCategoryName)
        {
            var SubCategory = await _subCategory.GetSubCategoryByName(SubCategoryName);
            var product = new Product
            {
                id = id,
                name = name,
                brand = brand,
                price = price,
                model = model,
                SellerName = SellerName,
                ProductionDate = ProductionDate,
                SubCategoryId = SubCategory.id
            };
            var result = await _product.EditProduct(product);
            if (result == true)
            {
                return RedirectToAction("Products","Product", new {area= "Admin",id = SubCategory.id});
            }
            return NotFound();
        }
        [Authorize(Permissions.Products.Delete)]
        public async Task<IActionResult> Delete (int id)
        {
            var Product = await _product.GetProductById(id);
            var SubCategoryId = Product.SubCategoryId;
            var result = await _product.DeleteProduct(id);
            if (result == true)
            {
                return RedirectToAction("Products", new { id = SubCategoryId });
            }
            return NotFound();
        }
        [Authorize(Permissions.Products.Delete)]
        public async Task<IActionResult> DeleteAllProducts (int id)
        {
            await _product.DeleteAllProductsBySubCategoryId(id);
           
            return RedirectToAction("Products", new { id = id });

        }

    }
}
