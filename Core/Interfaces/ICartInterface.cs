using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICartInterface
    {
        Task<List<Cart>> GetAllCartItems(string userId);
        Task<bool> AddingItemToCart(int ProductId, String UserId);
        Task<bool> ClearCart(String UserId);
        Task<bool> DeleteItemFromCart(int ProductId , string UserId);
        Task<bool> ReduceItemQuantity(int ProductId, string UserId);
    }
}
