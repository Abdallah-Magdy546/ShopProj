using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderInterface
    {
        Task<List<Order>> GetOrders(string UserId);
        Task<Order> GetOrderById(int id);
        Task<bool> AddingNewOrder(Order order);
        Task<bool> RemovingOrder(int id);
    }
}
