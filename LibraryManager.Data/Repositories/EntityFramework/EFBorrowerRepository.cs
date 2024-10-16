using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Data.Repositories.EntityFramework;

public class EFBorrowerRepository : IBorrowerRepository
{
    private readonly LibraryContext _dbContext;
    public EFBorrowerRepository(string connectionString)
    {
        _dbContext = new LibraryContext(connectionString);
    }
    public int Add(Borrower newBorrower)
    {
        _dbContext.Borrower.Add(newBorrower);
        _dbContext.SaveChanges();

        return newBorrower.BorrowerID;
    }

    public void Delete(Borrower borrower)
    {
        var checkoutLogs = _dbContext.CheckoutLog.Where(cl => cl.BorrowerID == borrower.BorrowerID);
        _dbContext.Remove(borrower);
        _dbContext.SaveChanges();

        if (checkoutLogs != null)
        {
            _dbContext.RemoveRange(checkoutLogs);
            _dbContext.SaveChanges();
        } 
    }

    public List<Borrower> GetAll()
    {
        return _dbContext.Borrower.ToList();
    }

    // Method for getting borrower without checkout logs
    public Borrower? GetByEmail(string email)
    {
        return _dbContext.Borrower.FirstOrDefault(b => b.Email == email);
    }

    // Method for getting borrower with checkout logs (returns DTO)
    public ViewBorrowerDTO? GetByEmailWithLogs(string email)
    {
        var borrowerWithLogs = _dbContext.Borrower
            .Where(b => b.Email == email)
            .Select(b => new ViewBorrowerDTO
            {
                BorrowerID = b.BorrowerID,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Email = b.Email,
                CheckoutLogs = b.CheckoutLogs.Select(cl => new CheckoutLogDTO
                {
                    CheckoutDate = cl.CheckoutDate,
                    ReturnDate = cl.ReturnDate,
                    MediaID = cl.MediaID,
                    Title = cl.Media.Title
                }).ToList()
            })
            .FirstOrDefault();

        return borrowerWithLogs;
    }

    public List<CheckoutLog> GetCheckoutLogsByEmail(string email)
    {
        return _dbContext.CheckoutLog
                         .Include(cl => cl.Media)
                         .Include(cl => cl.Borrower)
                         .Where(cl => cl.Borrower.Email == email)
                         .ToList();
    }

    public void Update(Borrower borrower)
    {
        var borrowerToUpdate = _dbContext.Borrower.SingleOrDefault(b => b.BorrowerID == borrower.BorrowerID);

        if (borrowerToUpdate != null)
        {
            borrowerToUpdate.FirstName = borrower.FirstName;
            borrowerToUpdate.LastName = borrower.LastName;
            borrowerToUpdate.Email = borrower.Email;
            borrowerToUpdate.Phone = borrower.Phone;
            
            _dbContext.SaveChanges();
        }
    }
}
