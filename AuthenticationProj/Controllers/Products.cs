using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;
using System.Security.Claims;
using static Core.Constants.Permissions;

namespace ShopProj.Controllers
{
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly ICategoryInterface _categories;
        private readonly ISubCategoryInterface _subCategories;
        private readonly IProductInterface _products;
        private readonly ICartInterface _cart;

        public ProductsController(ICategoryInterface categories, ISubCategoryInterface subCategories,
            IProductInterface products, ICartInterface cart)
        {
            _categories = categories;
            _subCategories = subCategories;
            _products = products;
            _cart = cart;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categories.GetAllCategories();
            return View(result);
        }
        public async Task<IActionResult> SubCategories(int id )
        {
            var result = await _categories.GetAllSubCategoriesById(id);
            List<SubCategory> subCategories = new List<SubCategory>();
            foreach(var subCategory in result )
            {
                var Subcat = await _subCategories.GetSubCategoryByName(subCategory);
                subCategories.Add(Subcat);
            }
            return View(subCategories);
        }
        public async Task<IActionResult> Products(string name , string sortOrder , string searchString, int? pageNumber, string currentFilter)
        {
            var SubCategory = await _subCategories.GetSubCategoryByName(name);
            var products = await _products.GetAllProductsBySubCategoryId(SubCategory.id);
            ViewBag.SubCategoryName = name;
            ViewBag.SubCategoryId = SubCategory.id;

            /////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] =  string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BrandSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Brand" : "";
            ViewData["ModelSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Model" : "";
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Id" : ""; 
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
                products = products.Where(prd=>prd.name.ToUpper().Contains(searchString.ToUpper()) || prd.model.ToUpper().Contains(searchString.ToUpper()) ||
                prd.brand.ToUpper().Contains(searchString.ToUpper())||prd.SellerName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Id":
                    products = products.OrderByDescending(s => s.id);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.price);
                    break;
                case "name_desc":
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
        public async Task<IActionResult> Details(int id)
        {
            var Product = await _products.GetProductById(id);
            var SubCategory = await _subCategories.GetSubCategoryById(Product.SubCategoryId);
            ViewBag.SubCategoryName = SubCategory.Name;
            return View(Product);
        }
        public async Task<IActionResult> AddToCart(int ProductId , string name)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = await _cart.AddingItemToCart(ProductId, UserId);
            if (result==true)
            {
                return RedirectToAction("Products", new { name = name });
            }
            return BadRequest();
        }
    }
}
