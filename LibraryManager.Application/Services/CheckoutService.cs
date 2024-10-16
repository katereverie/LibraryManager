using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Application.Services;

public class CheckoutService : ICheckoutService
{
    private ICheckoutRepository _checkoutRepository;
    private IBorrowerRepository _borrowerRepository;

    public CheckoutService(ICheckoutRepository checkoutRepository, IBorrowerRepository borrowerRepository)
    {
        _checkoutRepository = checkoutRepository;
        _borrowerRepository = borrowerRepository;
    }

    public Result CheckoutMedia(int mediaID, string email)
    {
        try
        {
            var borrower = _borrowerRepository.GetByEmail(email);

            if (borrower == null)
            {
                return ResultFactory.Fail($"Borrower with {email} not found.");
            }

            var logs = _checkoutRepository.GetCheckoutLogsByBorrowerEmail(email);
            int checkoutItemCount = 0;

            foreach (var log in logs)
            {
                if (log.DueDate < DateTime.Now )
                    return ResultFactory.Fail("Borrower has overdue item.");
                
                if (log.ReturnDate == null)
                    checkoutItemCount++;
            }

            if (checkoutItemCount >= 3)
                return ResultFactory.Fail("Borrower has more than 3 checked-out items.");

            var newCheckoutLog = new CheckoutLog
            {
                BorrowerID = borrower.BorrowerID,
                MediaID = mediaID,
                CheckoutDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null
            };

            _checkoutRepository.Add(newCheckoutLog);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result<List<CheckoutLog>> GetAllCheckedoutMedia()
    {
        try
        {
            var list = _checkoutRepository.GetAllCheckedoutMedia();

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<CheckoutLog>>("Currently no checked-out media.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
        }
    }

    public Result<List<Media>> GetAvailableMedia()
    {
        try
        {
            var list = _checkoutRepository.GetAvailableMedia();

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<Media>>("All media are either checked out or archived.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Media>>(ex.Message);
        }
    }

    public Result ReturnMedia(int checkoutLogID)
    {
        try
        {
            _checkoutRepository.Update(checkoutLogID);

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }
}
