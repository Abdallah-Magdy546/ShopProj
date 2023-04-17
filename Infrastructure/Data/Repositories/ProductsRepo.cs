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
    public class ProductsRepo : IProductInterface
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsBySubCategoryId(int? id)
        {
            return await _context.products.Where(prd => prd.SubCategoryId == id).ToListAsync();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.products.FindAsync(id);
        }
      
        public async Task<bool> AddProduct(Product product)
        {
            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditProduct(Product product)
        {
            var Product = await _context.products.FindAsync(product.id);
            if (Product != null)
            {
                _context.products.Remove(Product);
                await _context.products.AddAsync(product);
                await _context.SaveChangesAsync();  
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllProductsBySubCategoryId(int id)
        {
            var products = await _context.products.ToListAsync();
            foreach(var product in products)
            {
                if (product.SubCategoryId == id)
                {
                    _context.products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            return true;    
            
            
        }

        public async Task<Product> GetProductByName(string name)
        {
            var product = await _context.products.Where(prd=>prd.name == name).FirstOrDefaultAsync();
            return product;
        }

        public async Task<List<Product>> GetProductsRandomly()
        {
            var Categories = await _context.categories.ToListAsync();
            var SubCategories = new List<SubCategory>();
            var products = new List<Product>();
            foreach (var category in Categories)
            {
                var subCategory = await _context.subcategories.Where(sub=>sub.CategoryId==category.id).FirstOrDefaultAsync();
                if(subCategory != null)
                {
                    SubCategories.Add(subCategory);
                }
            }
            foreach(var subCategory in SubCategories)
            {
                var product = await _context.products.Where(prd=>prd.SubCategoryId==subCategory.id).FirstOrDefaultAsync();
                if(product != null)
                {
                    products.Add(product);
                }
            }
            return products;
        }
    }
}
