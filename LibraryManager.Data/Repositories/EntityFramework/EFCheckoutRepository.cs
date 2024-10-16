using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Data.Repositories.EntityFramework;

public class EFCheckoutRepository : ICheckoutRepository
{
    private readonly LibraryContext _dbContext;

    public EFCheckoutRepository(string connectionString)
    {
        _dbContext = new LibraryContext(connectionString);
    }
    public int Add(CheckoutLog newCheckoutLog)
    {
        _dbContext.CheckoutLog.Add(newCheckoutLog);
        _dbContext.SaveChanges();

        return newCheckoutLog.CheckoutLogID;
    }

    public List<CheckoutLog> GetAllCheckedoutMedia()
    {
        return _dbContext.CheckoutLog
                         .Include(cl => cl.Borrower)
                         .Include(cl => cl.Media)
                         .Where(cl => cl.ReturnDate == null)
                         .ToList();
    }

    public List<Media> GetAvailableMedia()
    {
        return _dbContext.Media
                         .Include(m => m.CheckoutLogs)
                         .Include(m => m.MediaType)
                         .Where(m => !m.IsArchived)
                         .Where(m => !m.CheckoutLogs.Any() ||
                                     m.CheckoutLogs.OrderByDescending(cl => cl.CheckoutLogID)
                                                    .First().ReturnDate != null)
                         .ToList();
    }

    public List<CheckoutLog> GetCheckoutLogsByBorrowerEmail(string email)
    {
        return _dbContext.CheckoutLog
                         .Include(cl => cl.Borrower)
                         .Where(cl => cl.Borrower.Email == email && cl.ReturnDate == null)
                         .ToList();
    }

    public List<CheckoutLog> GetCheckedoutMediaByBorrowerID(int borrowerID)
    {
        return _dbContext.CheckoutLog
                         .Include(cl => cl.Media)
                         .Where(cl => cl.BorrowerID == borrowerID && cl.ReturnDate == null)
                         .ToList();
    }

    public void Update(int checkoutLogID)
    {
        var checkoutLog = _dbContext.CheckoutLog
                          .FirstOrDefault(cl => cl.CheckoutLogID == checkoutLogID);

        if (checkoutLog != null)
        {
            checkoutLog.ReturnDate = DateTime.Now;

            _dbContext.SaveChanges();
        }
    }
}

