using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductInterface
    {
        Task<IEnumerable<Product>> GetAllProductsBySubCategoryId (int? id);
        Task<Product> GetProductById (int id);
        Task<bool> AddProduct(Product product);
        Task<bool> EditProduct (Product product);
        Task<bool> DeleteProduct (int id);
        Task<bool> DeleteAllProductsBySubCategoryId(int id);
        Task<Product> GetProductByName (string name);
        Task<List<Product>> GetProductsRandomly ();
    }
}
