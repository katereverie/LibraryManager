using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.MVC.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
