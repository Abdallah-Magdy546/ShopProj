using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SubCategoryRepo : ISubCategoryInterface
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNewSubCategory(string name  , int CategoryId)
        {
            var SubCat = await _context.subcategories.Where(c=>c.Name == name).FirstOrDefaultAsync();
            if (SubCat!=null)
            {
                return false;
            }
            var SubCategory = new SubCategory
            {
                Name = name,
                CategoryId = CategoryId
            };
            await _context.subcategories.AddAsync(SubCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSubCategory(int id)
        {
            var SubCat = await _context.subcategories.FindAsync(id);
            if (SubCat==null)
            {
                return false;
            }
            _context.subcategories.Remove(SubCat);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditSubCategory(SubCategory subCategory)
        {
            var Subcategory = new SubCategory
            {
                id = subCategory.id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
            };
            var SubCat = await _context.subcategories.Where(c=>c.id== subCategory.id).FirstOrDefaultAsync();
            if (SubCat==null) 
            {
                return false;
            }
            _context.subcategories.Remove(SubCat);
            await _context.subcategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategories()
        {

           return await _context.subcategories.ToListAsync();

        }

        public async Task<SubCategory> GetSubCategoryById(int id )
        {
            return await _context.subcategories.Where(c=>c.id== id).FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryByName(string name)
        {
            var SubCategory = await _context.subcategories.Where(s=>s.Name==name).FirstOrDefaultAsync();
            return SubCategory;
        }
        public async Task<string> GetCategoryBySubCategoryId (int id)
        {
            var SubCat= await _context.subcategories.FindAsync(id);
            var category = await _context.categories.FindAsync(SubCat.CategoryId);
            var result = category.Name;
            return result;
        }

        public async Task<Dictionary<string, string>> GetAllCategoriesNames()
        {
            Dictionary<string,string> result = new Dictionary<string,string>();
            var SubCats = await _context.subcategories.ToListAsync();
            foreach (var subCat in SubCats)
            {
                var category = await _context.categories.FindAsync(subCat.CategoryId);
                if (!result.ContainsKey(subCat.Name))
                {
                    result[subCat.Name] = category.Name;
                }
            }
            return result;

        }

        public async Task<List<string>> GetAllSubCategoriesNamesByCategoryId(int id)
        {
            var SubCategories = await _context.subcategories.ToListAsync();
            var SubCategoriesNames = new List<string>();
            foreach(var SubCat in  SubCategories)
            {
                if (SubCat.CategoryId == id)
                {
                    SubCategoriesNames.Add(SubCat.Name);
                }
            }
            return SubCategoriesNames;
        }
        public async Task<List<SubCategory>> GetAllSubCategoriesByCategoryId (int id)
        {
            var result = await _context.subcategories.Where(s=>s.CategoryId==id).ToListAsync();
            return result;
        }
    }
}
