using LibraryManagement.Test.MockRepos;
using LibraryManager.Application.Services;
using NUnit.Framework;

namespace LibraryManager.UnitTest;

[TestFixture]
public class CheckoutServiceTests
{
    [Test]
    public void CheckoutMedia_BorrowerHasMoreThanThreeCheckedOutItems()
    {
        var service = new CheckoutService(new MockCheckoutRepo(), new MockBorrowerRepo());
        
        
        // Borrower with ID 1 has 3 unreturned checked out items
        var result = service.CheckoutMedia(6, "test@example.com");

        Console.WriteLine(result.Message);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Borrower has more than 3 checked-out items."));
    }

    [Test]
    public void CheckoutMedia_BorrowerOverdueItems()
    {
        var service = new CheckoutService(new MockCheckoutRepo(), new MockBorrowerRepo());

        // Borrower with ID 3 has overdue item
        var result = service.CheckoutMedia(6, "test@example.com");

        Console.WriteLine(result.Message);

        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Borrower has overdue item."));
    }

    [Test]
    public void CheckoutMedia_Success()
    {
        var service = new CheckoutService(new MockCheckoutRepo(), new MockBorrowerRepo());

        // Borrower with ID 2 has 1 checked out item
        var result = service.CheckoutMedia(6, "test@example.com");

        Assert.That(result.Ok, Is.True);
    }
}
