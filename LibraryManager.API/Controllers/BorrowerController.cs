using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    public class BorrowerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
