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

    public Borrower? GetByEmail(string email)
    {
        return _dbContext.Borrower.SingleOrDefault(b => b.Email == email);
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
