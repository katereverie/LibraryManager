using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
