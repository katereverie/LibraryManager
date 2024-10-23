using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.MVC.Controllers;

public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService; 

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    public IActionResult Index()
    {
        return View();
    }
}
