using Core.Interfaces;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShopProj.Controllers
{
    public class Orders : Controller
    {
        private readonly IOrderInterface _order;
        private readonly IProductInterface _product;

        public Orders(IOrderInterface order, IProductInterface product)
        {
            _order = order;
            _product = product;
        }

        public async Task<IActionResult> Index()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _order.GetOrders(UserId);
            var OrdersView = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var product = await _product.GetProductById(order.ProductId);
                OrdersView.Add(new OrderViewModel
                {
                    Id = order.Id,
                    ProductName = product.name,
                    Quantity = order.Quantity,
                    Status = order.status,
                    Photo=product.Photo
                });
            }
            return View(OrdersView);
        }
    }
}
