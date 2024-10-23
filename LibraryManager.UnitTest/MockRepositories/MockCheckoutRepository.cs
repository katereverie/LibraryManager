using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManagement.Test.MockRepos;

public class MockCheckoutRepo : ICheckoutRepository
{
    private List<CheckoutLog> _logs = new List<CheckoutLog>
    {
        new CheckoutLog
        {
            CheckoutLogID = 1,
            BorrowerID = 1,
            MediaID = 1,
            CheckoutDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7),
            ReturnDate = null
        },
        new CheckoutLog
        {
            CheckoutLogID = 2,
            BorrowerID = 1,
            MediaID = 2,
            CheckoutDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7),
            ReturnDate = null
        },
        new CheckoutLog
        {
            CheckoutLogID = 3,
            BorrowerID = 1,
            MediaID = 3,
            CheckoutDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7),
            ReturnDate = null
        },
        new CheckoutLog
        {
            CheckoutLogID = 4,
            BorrowerID = 2,
            MediaID = 4,
            CheckoutDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7),
            ReturnDate = null
        },
        new CheckoutLog
        {
            CheckoutLogID = 5,
            BorrowerID = 3,
            MediaID = 5,
            CheckoutDate = new DateTime(2024, 1, 1),
            DueDate = new DateTime(2024, 1, 1).AddDays(7),
            ReturnDate = null
        }

    };

    public List<CheckoutLog> GetCheckoutLogsByBorrowerID(int borrowerID)
    {
        return _logs.FindAll(cl => cl.BorrowerID == borrowerID);
    }

    public int Add(CheckoutLog newCheckoutLog)
    {
        _logs.Add(newCheckoutLog);
        return newCheckoutLog.CheckoutLogID;
    }

    public List<CheckoutLog> GetAllCheckedoutMedia()
    {
        throw new NotImplementedException();
    }

    public List<Media> GetAvailableMedia()
    {
        throw new NotImplementedException();
    }

    public Borrower? GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public List<CheckoutLog> GetCheckedoutMediaByBorrowerID(int borrowerID)
    {
        throw new NotImplementedException();
    }

    public void Update(int checkoutLogID)
    {
        throw new NotImplementedException();
    }

    public List<CheckoutLog> GetCheckoutLogsByBorrowerEmail(string email)
    {
        throw new NotImplementedException();
    }
}
