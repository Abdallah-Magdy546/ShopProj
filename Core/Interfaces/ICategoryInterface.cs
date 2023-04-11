using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICategoryInterface
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategorieById(int id);
        Task<bool> DeleteCategory(int id);
        Task<bool> AddCategory(string name);
        Task<bool> EditCategory(Category category);
        Task<Category> GetCategoryByName(string name);

        //Task<bool> AddingSubCategoryToTheCategory (int categoryId , int SubCategoryId);
        Task<Dictionary<int, List<string>>> GetAllSubCategories();
        Task<List<string>> GetAllSubCategoriesById(int id);

    }
}
