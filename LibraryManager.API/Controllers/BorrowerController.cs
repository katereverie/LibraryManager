using LibraryManager.API.Models;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers;

/// <summary>
/// Controller for handling borrower-related operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BorrowerController : Controller
{
    private readonly IBorrowerService _borrowerService;
    private readonly ILogger<BorrowerController> _logger;

    /// <summary>
    /// Constructor method of BorrowerController, which injects ICheckoutService and ILogger and stores them in private readonly fields.
    /// </summary>
    /// <param name="borrowerService">Service for handling borrower-related operations</param>
    /// <param name="logger">Logger for logging events and errors</param>
    public BorrowerController(IBorrowerService borrowerService, ILogger<BorrowerController> logger)
    {
        _borrowerService = borrowerService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all borrowers, which includes their information
    /// </summary>
    /// <returns>List of objects of type Borrower</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(List<Borrower>), StatusCodes.Status200OK)]
    public IActionResult GetAllBorrowers()
    {
        var result = _borrowerService.GetAllBorrowers();

        if (result.Ok)
        {
            _logger.LogInformation("Borrowers retrieved successfully.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving borrowers. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a single borrower, which includes their personal information
    /// </summary>
    /// <param name="email">The email address of a borrower</param>
    /// <returns>A single Borrower object or null if not found</returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(Borrower), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBorrower(string email)
    {
        var result = _borrowerService.GetBorrower(email);

        if (result.Message.Contains("Borrower with email"))
        {
            _logger.LogWarning("Borrower not found. Email: {BorrowerEmail}", email);
            return NotFound();
        }

        if (result.Ok)
        {
            _logger.LogInformation("Borrower retrieved successfully.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving borrowe. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Adds a single borrower, which includes the borrower's first and last name, email address, and phone number.
    /// </summary>
    /// <param name="borrowerToAdd">A JSON object for adding borrower, which includes a borrower's FirstName, LastName, Email, Phone</param>
    /// <returns>An IActionResult indicating corresponding HTTP response</returns>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddBorrower(AddBorrower borrowerToAdd)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when adding borrower. {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        var entity = new Borrower
        {
            FirstName = borrowerToAdd.FirstName,
            LastName = borrowerToAdd.LastName,
            Email = borrowerToAdd.Email,
            Phone = borrowerToAdd.Phone
        };

        var result = _borrowerService.AddBorrower(entity);

        if (result.Ok)
        {
            _logger.LogInformation("Borrower added successfully. {BorrowerEmail}", entity.Email);
            return Created();
        }

        if (result.Message.Contains("Borrower with email"))
        {
            _logger.LogWarning("Attempt to add borrower with existing email. {Message}", entity.Email);
            return Conflict();
        }

        _logger.LogError("Error adding borrower. {BorrowerEmail} Error: {ErrorMessage}", entity.Email, result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Edits the first name, the last name, the email address, and phone number of a borrower.
    /// </summary>
    /// <param name="borrowerID">The ID number that uniquely identifies a borrower</param>
    /// <param name="borrowerToEdit">A JSON object for editing a borrower,  which includes the borrower's FirstName, LastName, Email, Phone</param>
    /// <returns>An IActionResult indicating corresponding HTTP response</returns>
    [HttpPut("{borrowerID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EditBorrower(int borrowerID, EditBorrower borrowerToEdit)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when editing borrower. {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        var entity = new Borrower
        {
            BorrowerID = borrowerID,
            FirstName = borrowerToEdit.FirstName,
            LastName = borrowerToEdit.LastName,
            Email = borrowerToEdit.Email,
            Phone = borrowerToEdit.Phone
        };

        var result = _borrowerService.UpdateBorrower(entity);

        if (result.Ok)
        {
            _logger.LogInformation("Borrower successfully updated.");
            return NoContent();
        }

        if (result.Message.Contains("Borrower with email"))
        {
            _logger.LogWarning("Attempted email edit conflicts with an existing email. {Message}", result.Message);
            return Conflict();
        }

        _logger.LogError("Error adding borrower. {BorrowerEmail} Error: {ErrorMessage}", entity.Email, result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Deletes a borrower by deleting their personal information and their associated data, such as checkout logs, if any.
    /// </summary>
    /// <param name="borrowerID">The ID number that uniquely identifies a borrower</param>
    /// <returns>An IActionResult indicating corresponding HTTP response</returns>
    [HttpDelete("{borrowerID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteBorrower(int borrowerID)
    {
        var entity = new Borrower
        {
            BorrowerID = borrowerID
        };

        var result = _borrowerService.DeleteBorrower(entity);

        if (result.Ok)
        {
            _logger.LogInformation("Borrower successfully deleted.");
            return NoContent();
        }

        _logger.LogError("Error deleting borrower. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves borrower information including checkout records
    /// </summary>
    /// <param name="email">borrower's email address</param>
    /// <returns></returns>
    [HttpGet("info/{email}")]
    [ProducesResponseType(typeof(List<CheckoutLog>), StatusCodes.Status200OK)]
    public IActionResult GetBorrowerInformation(string email)
    {
        var result = _borrowerService.GetCheckoutLogsByEmail(email);

        if (result.Ok)
        {
            _logger.LogInformation("Borrower information successfully retrieved.");
            return Ok(result.Data);
        }

        _logger.LogError("Error getting borrower information. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }
}
