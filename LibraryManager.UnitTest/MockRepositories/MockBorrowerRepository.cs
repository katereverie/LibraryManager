using LibraryManager.Core.DTOs;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManagement.Test.MockRepos;

public class MockBorrowerRepo : IBorrowerRepository
{
    private List<Borrower> _repo = new List<Borrower>
    {
        new Borrower
        {
            BorrowerID = 1,
            FirstName = "Issac",
            LastName = "Test",
            Email = "issac@test.com",
            Phone = "9119111911"
        }
    };

    public Borrower? GetByEmail(string email)
    {
        return _repo.Find(b => b.Email == email);
    }

    public int Add(Borrower newBorrower)
    {
        _repo.Add(newBorrower);
        return _repo.Last().BorrowerID;
    }

    public void Delete(Borrower borrower)
    {
        throw new NotImplementedException();
    }

    public List<Borrower> GetAll()
    {
        throw new NotImplementedException();
    }

    public List<CheckoutLog> GetCheckoutLogs(Borrower borrower)
    {
        throw new NotImplementedException();
    }

    public void Update(Borrower request)
    {
        throw new NotImplementedException();
    }

    public ViewBorrowerDTO? GetByEmailWithLogs(string email)
    {
        throw new NotImplementedException();
    }
}
