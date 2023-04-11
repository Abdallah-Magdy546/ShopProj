using AuthenticationProj.Pages.Category;
using Core.Interfaces;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShopProj.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartInterface _cart;
        private readonly IProductInterface _product;

        public CartController(ICartInterface cart, IProductInterface product)
        {
            _cart = cart;
            _product = product;
        }

        public async Task<IActionResult>Index()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var CartItems = await _cart.GetAllCartItems(UserId);
            float TotalPrice = 0;
            List<CartViewModel> Carts = new List<CartViewModel>();
            foreach(var item in CartItems)
            {
                var product = await _product.GetProductById(item.ProductId);
                var cart = new CartViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = product.name,
                    price = product.price,
                    Quantity = item.Quantity,
                    TotalProductPrice = product.price * item.Quantity
                };
                TotalPrice += cart.TotalProductPrice;
                Carts.Add(cart);
            }
            ViewBag.TotalPrice = TotalPrice;
            return View(Carts);
        }
        public async Task<IActionResult> AddingMoreProduct(int ProductId)
        {
            var result = await _cart.AddingItemToCart(ProductId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (result==true)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
        public async Task<IActionResult> ReduceAmount(int ProductId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cart.ReduceItemQuantity( ProductId , UserId);
            
            if (result == true) 
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
        public async Task<IActionResult> DeleteProduct (int ProductId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cart.DeleteItemFromCart(ProductId , UserId);
            if (result == true)
            {
                return RedirectToAction("Index");   
            }
            return BadRequest();
        }
        public async Task<IActionResult> ClearTheCart ()
        {
            var result = await _cart.ClearCart(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (result == true)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
    }
}
