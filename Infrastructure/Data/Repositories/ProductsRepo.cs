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
    }
}
