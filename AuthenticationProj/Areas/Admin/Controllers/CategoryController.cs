using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Core.ViewModel;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            var result = new List<AllCategoriesAndSubCategoriesForm>();
            foreach (var cat in categories)
            {
                result.Add (new AllCategoriesAndSubCategoriesForm
                {
                    id = cat.id,
                    name = cat.Name,
                    SubCatsNames = _SubCategories.GetAllSubCategoriesNamesByCategoryId(cat.id).Result.ToList(),
                    Photo=cat.Photo
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
            var SubCategories = await _SubCategories.GetAllSubCategoriesByCategoryId(category.id);
            ViewBag.NumOfSubCategory = SubCategories.Count();
            var result = new CategoriesAndSubCategoriesForm
            {
                id = id,
                name = category.Name,
                SubCats = SubCats,
                Photo=category.Photo
                
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
        public async Task<ActionResult> Create(string name , IFormFile Photo)
        {
            if (Photo == null)
            {
                return BadRequest("Photo parameter is null");
            }
            var category = new Category
            {
                Name = name
            };
            using (var memoryStream = new MemoryStream())
            {
                Photo.CopyTo(memoryStream);
                category.Photo = memoryStream.ToArray();
            }
            await _categories.AddCategory(category);

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
        public async Task<ActionResult> Edit(int id, string name, IFormFile Photo)
        {
           
            var category = new Category
            {
                id = id,
                Name = name
            };
            using (var memoryStream = new MemoryStream())
            {
                Photo.CopyTo(memoryStream);
                category.Photo = memoryStream.ToArray();
            }
            await _categories.EditCategory(category);
            return RedirectToAction(nameof(Index));
        }
        // GET: CategoryController/Delete/5
        [Authorize(Permissions.Categories.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            
          var result =  await _categories.DeleteCategory(id);
            if (result == true)
            {
                await _SubCategories.DeleteSubCategoriesByCategroyId(id);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();


        }
    }
}
