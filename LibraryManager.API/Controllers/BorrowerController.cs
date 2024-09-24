using LibraryManager.API.Models;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerController : Controller
    {
        private readonly IBorrowerService _borrowerService;
        private readonly ILogger<BorrowerController> _logger;

        public BorrowerController(IBorrowerService borrowerService, ILogger<BorrowerController> logger)
        {
            _borrowerService = borrowerService;
            _logger = logger;
        }

        /// <summary>
        /// List all borrowers
        /// </summary>
        /// <returns></returns>
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
        /// View a borrower
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        /// Add a borrower
        /// </summary>
        /// <param name="borrowerToAdd"></param>
        /// <returns></returns>
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
        /// Edit a borrower
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <param name="borrowerToEdit"></param>
        /// <returns></returns>
        [HttpPut("{borrowerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditBorrower(int borrowerId, EditBorrower borrowerToEdit)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state when editing borrower. {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var entity = new Borrower
            {
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
        /// Delete a borrower
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <returns></returns>
        [HttpDelete("{borrowerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteBorrower(int borrowerId)
        {
            var entity = new Borrower
            {
                BorrowerID = borrowerId
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
}
