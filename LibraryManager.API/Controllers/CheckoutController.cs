using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
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
        /// Constructor method of CheckoutController takes an ICheckoutService and an ILogger as parameters, and store them in private readonly fields
        /// </summary>
        /// <param name="checkoutService">Service for handling checkout-related operations</param>
        /// <param name="logger">Logger for logging events and errors</param>
        public CheckoutController (ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of available media that can be checked out
        /// </summary>
        /// <returns>List of available media</returns>
        [HttpGet]
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
            return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
        }

        /// <summary>
        /// Retrieves a list of checkout logs that shows all checked-out media
        /// </summary>
        /// <returns>List of checked-out media</returns>
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
            return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
        }

        /// <summary>
        /// Checks out a media
        /// </summary>
        /// <param name="mediaID">The ID of the media</param>
        /// <param name="borrowerID">The ID of the borrower</param>
        /// <returns></returns>
        [HttpPost("media/{mediaID}/{borrowerID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CheckoutMedia(int mediaID, int borrowerID)
        {
            var result = _checkoutService.CheckoutMedia(mediaID, borrowerID);

            if (result.Ok)
            {
                _logger.LogInformation("Media successfully checked out.");
                return Created();
            }

            if (result.Message.Contains("Borrower has"))
            {
                _logger.LogWarning("Borrower not allowed to check out any more media. {Message}", result.Message);
                return Conflict();
            }

            _logger.LogError("Error checking out media. Error: {ErrorMessage}", result.Message);
            return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
        }

        /// <summary>
        /// Returns a media
        /// </summary>
        /// <param name="checkoutLogID">the ID of the checkout log</param>
        /// <returns></returns>
        [HttpPost("returns/{checkoutLogID}")]
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
            return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
        }
    }
}
