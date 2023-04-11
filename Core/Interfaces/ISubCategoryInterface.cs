using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISubCategoryInterface
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategories();
        Task<SubCategory> GetSubCategoryById(int id);
        Task<bool> CreateNewSubCategory(string name , int CategoryId);
        Task<bool> EditSubCategory(SubCategory subCategory);
        Task<bool> DeleteSubCategory(int id);
        Task<SubCategory> GetSubCategoryByName(string name);
        Task<string> GetCategoryBySubCategoryId(int id);
        Task<Dictionary<string, string>> GetAllCategoriesNames();
        Task<List<string>> GetAllSubCategoriesNamesByCategoryId(int id);
        Task<List<SubCategory>> GetAllSubCategoriesByCategoryId(int id);
    }
}
