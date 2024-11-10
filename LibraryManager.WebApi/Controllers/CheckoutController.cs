using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers;

/// <summary>
/// Controller for handling checkout-related operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;
    private readonly ILogger<CheckoutController> _logger;

    /// <summary>
    /// Constructor for <see cref="CheckoutController"/>, injecting <see cref="ICheckoutService"/> and <see cref="ILogger{CheckoutController}"/>
    /// </summary>
    /// <param name="checkoutService">Service for handling checkout-related operations</param>
    /// <param name="logger">Logger for logging events and errors</param>
    public CheckoutController(ICheckoutService checkoutService, ILogger<CheckoutController> logger)
    {
        _checkoutService = checkoutService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of available <see cref="Media"/> that can be checked out.
    /// </summary>
    /// <returns>List of available <see cref="Media"/> objects and an <see cref="IActionResult"/> indicating the HTTP response.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(List<Media>), StatusCodes.Status200OK)]
    public IActionResult GetAvailableMedia()
    {
        var result = _checkoutService.GetAvailableMedia();

        if (result.Ok)
        {
            _logger.LogInformation("Available media successfully retrieved.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving available media. Error: {ErrorMessage}", result.Message);
        return StatusCode(StatusCodes.Status500InternalServerError,
            "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a list of <see cref="CheckoutLog"/> showing all checked-out media.
    /// </summary>
    /// <returns>List of checked-out media as <see cref="CheckoutLog"/> and an <see cref="IActionResult"/> indicating the HTTP response.</returns>
    [HttpGet("log")]
    [ProducesResponseType(typeof(List<CheckoutLog>), StatusCodes.Status200OK)]
    public IActionResult GetCheckoutLog()
    {
        var result = _checkoutService.GetAllCheckedoutMedia();

        if (result.Ok)
        {
            _logger.LogInformation("All checked-out media successfully retrieved.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving all checked-out media. Error: {ErrorMessage}", result.Message);
        return StatusCode(StatusCodes.Status500InternalServerError,
            "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Checks out a <see cref="Media"/> item that is neither archived nor already checked-out.
    /// </summary>
    /// <param name="mediaID">The ID number that uniquely identifies a <see cref="Media"/>.</param>
    /// <param name="email">The email that uniquely identifies a borrower.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the HTTP response.</returns>
    [HttpPost("media/{mediaID}/{email}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult CheckoutMedia(int mediaID, string email)
    {
        var result = _checkoutService.CheckoutMedia(mediaID, email);

        if (result.Ok)
        {
            _logger.LogInformation("Media successfully checked out.");
            return CreatedAtAction(nameof(GetCheckoutLog), new { mediaID, email });
        }

        if (result.Message.Contains("Borrower with") || result.Message.Contains("Borrower has"))
        {
            _logger.LogWarning("Checkout process terminated. {Message}", result.Message);
            return Conflict(result.Message);
        }

        _logger.LogError("Error checking out media. Error: {ErrorMessage}", result.Message);
        return StatusCode(StatusCodes.Status500InternalServerError,
            "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Returns a <see cref="Media"/> item by assigning a return date to the corresponding <see cref="CheckoutLog"/>.
    /// </summary>
    /// <param name="checkoutLogID">The ID number that uniquely identifies a <see cref="CheckoutLog"/>.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the HTTP response.</returns>
    [HttpPut("returns/{checkoutLogID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult ReturnMedia(int checkoutLogID)
    {
        var result = _checkoutService.ReturnMedia(checkoutLogID);

        if (result.Ok)
        {
            _logger.LogInformation("Media successfully returned.");
            return NoContent();
        }

        _logger.LogError("Error returning media. Error: {ErrorMessage}", result.Message);
        return StatusCode(StatusCodes.Status500InternalServerError,
            "An unexpected error occurred while processing your request. Please try again later.");
    }
}
