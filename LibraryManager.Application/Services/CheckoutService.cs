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
            if (!_checkoutRepository.IsMediaAvailable(mediaID))
            {
                return ResultFactory.Fail($"Media with ID {mediaID} is not avaialbel");
            }

            var borrower = _borrowerRepository.GetByEmail(email);

            if (borrower == null)
            {
                return ResultFactory.Fail($"Borrower with {email} not found.");
            }

            var logs = _checkoutRepository.GetCheckoutLogsByBorrowerID(borrower.BorrowerID);
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

    public Result CheckoutMedia(int mediaID, int borrowerID)
    {
        throw new NotImplementedException();
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

    public Result<List<CheckoutLog>> GetCheckedOutMediaByBorrowerID(int borrowerID)
    {
        try
        {
            var list = _checkoutRepository.GetCheckedoutMediaByBorrowerID(borrowerID);

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<CheckoutLog>>("Borrower hasn't checked out any media.");

        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
        }
    }

    public Result<List<CheckoutLog>> GetCheckoutLogsByBorrowerID(int borrowerID)
    {
        try
        {
            var list = _checkoutRepository.GetCheckoutLogsByBorrowerID(borrowerID);

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<CheckoutLog>>($"No checkout log by Borrrower ID {borrowerID} found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
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
