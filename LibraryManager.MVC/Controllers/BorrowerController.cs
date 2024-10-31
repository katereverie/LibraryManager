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

    public IActionResult Index(string? email)
    {
        var borrowers = new List<BorrowerForm>();

        if (string.IsNullOrEmpty(email))
        {
            var result = _borrowerService.GetAllBorrowers();
            if (result.Ok)
            {
                borrowers.AddRange(result.Data.Select(e => new BorrowerForm(e)).ToList());
                if (!borrowers.Any())
                {
                    TempData["InfoMessage"] = "No borrowers found in the system.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
        }
        else
        {
            var result = _borrowerService.GetBorrower(email);
            if (result.Ok)
            {
                borrowers.Add(new BorrowerForm(result.Data));
                TempData["SuccessMessage"] = "Borrower found.";
            }
            else
            {
                if (result.Message.Contains("No borrower"))
                {
                    TempData["WarningMessage"] = $"No borrower found with email: {email}";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
        }

        return View(borrowers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new BorrowerForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BorrowerForm model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = _borrowerService.AddBorrower(model.ToEntity());

        if (!result.Ok)
        {
            string messageType = result.Message.Contains("already") ? "Warning" : "Error";
            TempData[$"{messageType}Message"] = result.Message;

            return View(model);
        }

        TempData["SuccessMessage"] = $"Borrower created with ID {result.Data}";

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Details(string email)
    {
        var result = _borrowerService.GetBorrowerWithLogs(email);
        if (result.Ok)
        {
            return View(result.Data);
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Edit(string email)
    {
        var result = _borrowerService.GetBorrower(email);

        return result.Ok ? View(new BorrowerForm(result.Data)) : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(BorrowerForm model)
    {
        if (!ModelState.IsValid)
        {
            TempData["WarningMessage"] = "Invalid format";
            return View(model);
        }

        var result = _borrowerService.UpdateBorrower(model.ToEntity());

        if (result.Ok)
        {
            TempData["SuccessMessage"] = "Borrower information updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "An error occurred while upding borrower information.";
        }

        return View(model);
    }
}
