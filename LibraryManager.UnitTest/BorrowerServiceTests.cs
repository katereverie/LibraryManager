using LibraryManagement.Test.MockRepos;
using LibraryManager.Application.Services;
using LibraryManager.Core.Entities;
using NUnit.Framework;

namespace LibraryManagement.Test;

[TestFixture]
public class BorrowerServiceTests
{
    [Test]
    public void AddBorrower()
    {
        var service = new BorrowerService(new MockBorrowerRepo());

        Borrower newB1 = new Borrower
        {
            BorrowerID = 2,
            FirstName = "Issac",
            LastName = "Doe",
            Email = "issac@test.com",
            Phone = "1989999666"
        };

        Borrower newB2 = new Borrower
        {
            BorrowerID = 3,
            FirstName = "Issac\'s",
            LastName = "Mom",
            Email = "issacmom@example.com",
            Phone = "1989999666"
        };

        var addResult1 = service.AddBorrower(newB1);
        var addResult2 = service.AddBorrower(newB2);

        Assert.That(addResult1.Ok, Is.False);
        Assert.That(addResult2.Ok, Is.True);
    }
}
