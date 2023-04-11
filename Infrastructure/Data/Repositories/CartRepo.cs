using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class CartRepo : ICartInterface
    {
        private readonly ApplicationDbContext _context;

        public CartRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddingItemToCart(int ProductId, string UserId)
        {
            Cart CartItem = await _context.Carts.Where(Item=>Item.ProductId==ProductId && Item.UserId==UserId).FirstOrDefaultAsync();
            if (CartItem != null)
            {
                Cart Item = new Cart
                {
                    ProductId = ProductId,
                    UserId = UserId,
                    Quantity = CartItem.Quantity + 1
                };
                _context.Carts.Remove(CartItem);
                await _context.Carts.AddAsync(Item);
                await _context.SaveChangesAsync();
                return true;
            }
            else 
            {
                Cart item = new Cart
                {
                    ProductId = ProductId,
                    UserId = UserId,
                    Quantity = 1
                };
                await _context.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ClearCart(string UserId)
        {
           var items =  await _context.Carts.Where(i=>i.UserId==UserId).ToListAsync();
            if(items!=null)
            {
                foreach(var item in items)
                {
                    _context.Carts.Remove(item);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteItemFromCart(int ProductId, string UserId)
        {
            var item = await _context.Carts.Where(i=>i.ProductId==ProductId&&i.UserId==UserId).FirstOrDefaultAsync();
            if (item!=null)
            {
                _context.Carts.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Cart>> GetAllCartItems(string userId)
        {
            var Items = await _context.Carts.Where(i=>i.UserId== userId).ToListAsync();
            return Items;
        }

        public async Task<bool> ReduceItemQuantity(int ProductId, string UserId)
        {
            var item = await _context.Carts.Where(prd=>prd.ProductId==ProductId && prd.UserId==UserId).FirstOrDefaultAsync();
            if (item!=null)
            {
                Cart Newitem = new Cart
                {
                    ProductId = ProductId,
                    UserId = UserId,
                    Quantity = item.Quantity - 1,
                };
                _context.Carts.Remove(item);
                await _context.Carts.AddAsync(Newitem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
