using AuthenticationProj.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class CategoriesRepo : ICategoryInterface
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
        
            List<Category> categories = await _context.categories.ToListAsync();

            return categories;

        }

        public async Task<Category> GetCategorieById(int id)
        {
            return await _context.categories.FindAsync(id);
        }

        public async Task<bool> AddCategory(string name )
        {
            
            var category = new Category
            {
                Name = name
            };
            var cat = await _context.categories.Where(c => c.Name == name).FirstOrDefaultAsync();
            if ( cat != null)
            {
                return false;
            }
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null) 
            {
                return false;
            }
            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
                
        }

        public async Task<bool> EditCategory(Category category)
        {

            var cat = await _context.categories.Where(c => c.id == category.id).FirstOrDefaultAsync();
            if (cat != null) 
            {
                _context.categories.Remove(cat);
                await _context.categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            var category = await _context.categories.Where(c=>c.Name== name).FirstOrDefaultAsync();
            return category;
        }

        //public async Task<bool> AddingSubCategoryToTheCategory(int categoryId, int SubCategoryId)
        //{
        //    var Category = await _context.categories.Where(c=>c.id==categoryId).FirstOrDefaultAsync();
        //    //var SubCategory = await _context.subcategories.Where(c => c.id == SubCategoryId).FirstOrDefaultAsync();
        //    if (Category != null)
        //    {
        //        //var subCategory = await _context.categoriesAndSubCategories.FindAsync(SubCategoryId);
        //        //if (subCategory != null) { return false; }
        //        var CategoriesAndSubCategories = new CategoriesAndSubCategories { CategoryId=categoryId, SubCategoryId=SubCategoryId };
        //        await _context.categoriesAndSubCategories.AddAsync(CategoriesAndSubCategories);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;   
        //}

        public async Task<List<string>> GetAllSubCategoriesById(int id)
        {
            var SubCats = await _context.subcategories.ToListAsync();
            if (SubCats == null)
            {
                return null;
            }
            List<string> result = new List<string>();
            foreach(var subCat in SubCats)
            {
                if (subCat.CategoryId == id)
                    result.Add(subCat.Name);
            }

            return result;
        }
        public async Task<Dictionary<int,List<string>>> GetAllSubCategories()
        {
           Dictionary<int, List<string>> CatAndSubCat = new Dictionary<int, List<string>>();
           var SubCats = await _context.subcategories.ToListAsync();
           foreach(var cat in SubCats)
            {
                if (CatAndSubCat.ContainsKey(cat.CategoryId))
                    CatAndSubCat[cat.CategoryId].Add(cat.Name);
                else if(!CatAndSubCat.ContainsKey(cat.CategoryId))
                {
                    CatAndSubCat.Add(cat.CategoryId, new List<string> {cat.Name});
                }
            }
            return CatAndSubCat;
        }
    }
}
