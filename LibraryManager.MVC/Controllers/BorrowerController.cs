﻿using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.MVC.Controllers
{
    public class BorrowerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
