using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
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
        [HttpGet]
        [ProducesResponseType(typeof(List<Borrower>), StatusCodes.Status200OK)]
        public IActionResult GetAllBorrowers()
        {
            var result = _borrowerService.GetAllBorrowers();

            if (result.Ok)
            {
                _logger.LogInformation("All borrowers retrieved successfully");
                return Ok(result.Data);
            }
            
            _logger.LogError("Error retrieving all borrowers. {Message}", result.Message);
            return StatusCode(500, result.Message);
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

            if (result.Ok)
            {
                _logger.LogInformation("Borrower retrieved successfully.");
                return Ok(result.Data);
            }

            if (result.Message.Contains("No Borrower"))
            {
                _logger.LogWarning(result.Message, $"Borrower with email {email} not found");
                return NotFound();
            }

            _logger.LogError(result.Message, "Error retrieving a borrower.");
            return StatusCode(500, result.Message);
        }

        public IActionResult AddBorrower(AddBorrower borrowerToAdd)
        {

        }
    }
}
