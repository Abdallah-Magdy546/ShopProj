using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Core.ViewModel;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryInterface _categories;
        private readonly ISubCategoryInterface _SubCategories;
        public CategoryController(ICategoryInterface categories, ISubCategoryInterface SubCategories)
        {
            _categories = categories;
            _SubCategories = SubCategories;
        }
        [Authorize(Permissions.Categories.View)]
        public async Task<ActionResult> Index()
        {
            List<Category> categories = await _categories.GetAllCategories();
            if (categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            //Dictionary<int, List<string>> SubCategories = new Dictionary<int, List<string>>();
            //SubCategories = await _categories.GetAllSubCategories();
            var result = new List<AllCategoriesAndSubCategoriesForm>();
            foreach (var cat in categories)
            {
                result.Add
                (new AllCategoriesAndSubCategoriesForm
                {
                    id = cat.id,
                    name = cat.Name,
                    SubCatsNames = _SubCategories.GetAllSubCategoriesNamesByCategoryId(cat.id).Result.ToList()
                });

            }

            return View(result);

        }

        // GET: CategoryController/Details/5
        [Authorize(Permissions.Categories.View)]
        public async Task<ActionResult> Details(int id)
        {
            var category = await _categories.GetCategorieById(id);
            var SubCats = await _categories.GetAllSubCategoriesById(id);
            var result = new CategoriesAndSubCategoriesForm
            {
                id = id,
                name = category.Name,
                SubCats = SubCats
            };
            return View(result);
        }

        // GET: CategoryController/Create
        [Authorize(Permissions.Categories.Create)]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Categories.Create)]
        public async Task<ActionResult> Create(string name)
        {
            await _categories.AddCategory(name);

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        [Authorize(Permissions.Categories.Edit)]
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _categories.GetCategorieById(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Categories.Edit)]
        public async Task<ActionResult> Edit(int id, string name)
        {
            var category = new Category
            {
                id = id,
                Name = name
            };
            await _categories.EditCategory(category);
            return RedirectToAction(nameof(Index));
        }
        // GET: CategoryController/Delete/5
        [Authorize(Permissions.Categories.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _categories.DeleteCategory(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
