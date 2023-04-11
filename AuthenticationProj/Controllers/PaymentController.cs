using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;

public class PaymentController : Controller
{
    private readonly ICartInterface _cart;
    private readonly IOrderInterface _order;

    public PaymentController(ICartInterface cart , IOrderInterface order)
    {
        _cart = cart;
        _order = order;
    }
    public IActionResult Index(float Total)
    {
        ViewBag.Total = Total;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Charge(string stripeToken, string email, decimal amount)
    {
        StripeConfiguration.ApiKey = "sk_test_51MvKVDEIEh78QVF8AgoY0IwcaWcJ7xQodpZ0CYE33rdcorqB23nXlx0rF79cRERoyOSVzLzcY1rk3Y9ouUXYG7Lf00nd4OMHef";

        var options = new ChargeCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "usd",
            Description = "Payment Description",
            Source = stripeToken,
            StatementDescriptor = "Payment Descriptor",
            ReceiptEmail = email
        };

        var service = new ChargeService();
        Charge charge = await service.CreateAsync(options);

        if (charge.Status=="succeeded")
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cart.GetAllCartItems(UserId);
            foreach (var item in cart)
            {
                var order = new Order
                {
                    UserId = UserId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity, 
                    status="Pinding"
                };
                await _order.AddingNewOrder(order);
                await _cart.DeleteItemFromCart(item.ProductId, item.UserId);
            }
            return RedirectToAction("Success");
        }
        else
        {
            return RedirectToAction("Error");
        }

    }
    public async Task<IActionResult> Success()
    {
        return View();
    }
    public async Task<IActionResult> Error()
    {
        return View();
    }
}