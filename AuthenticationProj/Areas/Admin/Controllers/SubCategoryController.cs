using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryInterface _subCategory;
        private readonly ICategoryInterface _category;

        public SubCategoryController(ISubCategoryInterface subCategory, ICategoryInterface category)
        {
            _subCategory = subCategory;
            _category = category;
        }

        // GET: SubCategoryController
        [Authorize(Permissions.SubCategories.View)]
        public async Task<ActionResult> Index()
        {
            var SubCategories = await _subCategory.GetAllSubCategories();
            var result = new List<AllSubCategoriesAndCategoriesForm>();
            Dictionary<string, string> catNames = new Dictionary<string, string>();
            catNames = await _subCategory.GetAllCategoriesNames();
            foreach (var SubCat in SubCategories)
            {
                result.Add
                (new AllSubCategoriesAndCategoriesForm
                {
                    id = SubCat.id,
                    name = SubCat.Name,
                    CatName = catNames[SubCat.Name]
                });
            }
            return View(result);
        }

        // GET: SubCategoryController/Details/5
        [Authorize(Permissions.SubCategories.View)]
        public async Task<ActionResult> Details(int id)
        {
            var SubCategory = await _subCategory.GetSubCategoryById(id);
            var Category = await _subCategory.GetCategoryBySubCategoryId(id);
            var result = new SubCategoriesAndCategoriesForm
            {
                id = id,
                name = SubCategory.Name,
                CatName = Category
            };
            return View(result);
        }

        // GET: SubCategoryController/Create
        [Authorize(Permissions.SubCategories.Create)]
        public async Task<ActionResult> Create()
        {
            var Categories = await _category.GetAllCategories();
            return View(Categories);
        }

        // POST: SubCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.SubCategories.Create)]
        public async Task<ActionResult> Create(string name, string CategoryName)
        {
            var Category = await _category.GetCategoryByName(CategoryName);
            var CategoryId = Category.id;
            var result = await _subCategory.CreateNewSubCategory(name, CategoryId);

            if (result == true)
            {
                return RedirectToAction("Index");
            }
            return Problem("This SubCategory allready added");

        }

        // GET: SubCategoryController/Edit/5
        [Authorize(Permissions.SubCategories.Edit)]
        public async Task<ActionResult> Edit(int id)
        {
            var SubCategory = await _subCategory.GetSubCategoryById(id);
            var Category = await _subCategory.GetCategoryBySubCategoryId(id);
            var categories = await _category.GetAllCategories();
            List<string> catsNames = new List<string>();
            foreach (var category in categories)
            {
                catsNames.Add(category.Name);
            }
            var result = new SubCategoriesAndCategoriesEditForm
            {
                id = id,
                name = SubCategory.Name,
                CatName = Category,
                CatsNames = catsNames
            };
            return View(result);
        }

        // POST: SubCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.SubCategories.Edit)]
        public async Task<ActionResult> Edit(int id, string name, string CatName)
        {
            var Category = await _category.GetCategoryByName(CatName);
            var CatId = Category.id;
            var subCategory = new SubCategory
            {
                id = id,
                Name = name,
                CategoryId = CatId
            };
            var result = await _subCategory.EditSubCategory(subCategory);
            if (result == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: SubCategoryController/Delete/5
        [Authorize(Permissions.SubCategories.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _subCategory.DeleteSubCategory(id);
            if (result == true)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }
    }
}
