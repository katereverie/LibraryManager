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

    [HttpPost]
    public IActionResult Checkin(int checkoutLogID, string email)
    {
        var result = _checkoutService.ReturnMedia(checkoutLogID);

        if (!result.Ok)
        {
            TempData["ErrorMessage"] = "An error occurred when returning media.";
            return RedirectToAction("Index", "Borrower");
        }

        return RedirectToAction("Details", "Borrower", new { email });
    }
}
