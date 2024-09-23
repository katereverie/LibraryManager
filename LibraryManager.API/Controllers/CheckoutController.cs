using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
