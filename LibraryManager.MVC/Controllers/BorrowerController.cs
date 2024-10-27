using LibraryManager.Core.Interfaces;
using LibraryManager.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.MVC.Controllers;

public class BorrowerController : Controller
{
    private readonly IBorrowerService _borrowerService;

    public BorrowerController(IBorrowerService borrowerService)
    {
        _borrowerService = borrowerService;
    }

    public IActionResult Index()
    {
        var result = _borrowerService.GetAllBorrowers();

        if (result.Ok && result.Data != null)
        {
            var borrowers = result.Data.Select(e => new BorrowerForm(e)).ToList();
            return View(borrowers);
        }

        TempData["ErrorMessage"] = result.Message;
        return View("Error");
    }
}
