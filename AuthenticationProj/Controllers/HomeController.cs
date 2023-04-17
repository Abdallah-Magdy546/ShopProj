using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopProj.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ICategoryInterface _category;
        private readonly ISubCategoryInterface _subCategory;
        private readonly IProductInterface _product;

        public HomeController(ICategoryInterface Category , ISubCategoryInterface subCategory , IProductInterface product)
        {
            _category = Category;
            _subCategory = subCategory;
            _product = product;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Category = await _category.GetAllCategories();
            ViewBag.SubCategory = await _subCategory.GetSubCategoryRandomly();
            ViewBag.Products = await _product.GetProductsRandomly();
            return View();
        }
    }
}
