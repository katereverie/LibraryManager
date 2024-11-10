using LibraryManager.API.Models;
using LibraryManager.Core.DTOs;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers;

/// <summary>
/// Controller for handling borrower-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BorrowerController : Controller
{
    private readonly IBorrowerService _borrowerService;
    private readonly ILogger<BorrowerController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BorrowerController"/> class, injecting the borrower service and logger.
    /// </summary>
    /// <param name="borrowerService">Service for handling borrower-related operations.</param>
    /// <param name="logger">Logger for logging events and errors.</param>
    public BorrowerController(IBorrowerService borrowerService, ILogger<BorrowerController> logger)
    {
        _borrowerService = borrowerService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all borrowers, including their information.
    /// </summary>
    /// <returns>A <see cref="List{Borrower}"/> containing borrower details.</returns>
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
    /// Retrieves a single borrower based on their email.
    /// </summary>
    /// <param name="email">The email address of the borrower.</param>
    /// <returns>A <see cref="Borrower"/> object if found; otherwise, a 404 Not Found status.</returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(Borrower), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBorrower(string email)
    {
        var result = _borrowerService.GetBorrower(email);

        if (result.Message.Contains("No Borrower"))
        {
            _logger.LogWarning(result.Message);
            return NotFound("Borrower not found.");
        }

        if (result.Ok)
        {
            _logger.LogInformation("Borrower retrieved successfully.");
            return Ok(result.Data);
        }

        _logger.LogError(result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a borrower's details, including their checkout history.
    /// </summary>
    /// <param name="email">The email address of the borrower.</param>
    /// <returns>A <see cref="BorrowerDetailsDTO"/> with borrower details and logs if found; otherwise, a 404 Not Found status.</returns>
    [HttpGet("{email}/logs")]
    [ProducesResponseType(typeof(BorrowerDetailsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBorrowerWithLogs(string email)
    {
        var result = _borrowerService.GetBorrowerWithLogs(email);

        if (result.Message.Contains("No borrower"))
        {
            _logger.LogWarning(result.Message);
            return NotFound("Borrower not found.");
        }

        if (result.Ok)
        {
            _logger.LogInformation("Borrower with checkout logs retrieved successfully.");
            return Ok(result.Data);
        }

        _logger.LogError(result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Adds a new borrower with provided details.
    /// </summary>
    /// <param name="borrowerToAdd">A <see cref="AddBorrower"/> object containing the borrower's information.</param>
    /// <returns>A 201 Created response if successful; otherwise, appropriate error responses.</returns>
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
    /// Updates an existing borrower's details.
    /// </summary>
    /// <param name="borrowerID">The unique ID of the borrower.</param>
    /// <param name="borrowerToEdit">A <see cref="EditBorrower"/> object with the updated borrower details.</param>
    /// <returns>A 204 No Content response if successful; otherwise, appropriate error responses.</returns>
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

        _logger.LogError("Error updating borrower. {BorrowerEmail} Error: {ErrorMessage}", entity.Email, result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Deletes a borrower and their associated data.
    /// </summary>
    /// <param name="borrowerID">The unique ID of the borrower.</param>
    /// <returns>A 204 No Content response if successful; otherwise, appropriate error responses.</returns>
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
}
