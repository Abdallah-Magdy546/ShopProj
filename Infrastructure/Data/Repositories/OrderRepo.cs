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
    public class OrderRepo : IOrderInterface
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddingNewOrder(Order order)
        {
            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.orders.FindAsync(id);
            return order;
            
        }

        public async Task<List<Order>> GetOrders(string UserId)
        {
            var orders = await _context.orders.Where(ord=>ord.UserId == UserId).ToListAsync();
            return orders;
        }

        public async Task<bool> RemovingOrder(int id)
        {
            var order = await _context.orders.FindAsync(id);
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
