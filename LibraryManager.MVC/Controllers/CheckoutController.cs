using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.MVC.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
