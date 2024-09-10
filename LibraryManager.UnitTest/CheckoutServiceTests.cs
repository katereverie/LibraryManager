using LibraryManagement.Test.MockRepos;
using LibraryManager.Application.Services;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class CheckoutServiceTests
    {
        [Test]
        public void CheckBorrowerStatus()
        {
            var service = new CheckoutService(new MockCheckoutRepo(), new MockMediaRepo());

            // Borrower with ID 1 has 3 unreturned checked out items
            // Borrower with ID 2 has 1 checked out item
            // Borrower with ID 3 has overdue item
            var result1 = service.CheckBorrowStatus(1);
            var result2 = service.CheckBorrowStatus(2);
            var result3 = service.CheckBorrowStatus(3);

            Assert.That(result1.Ok, Is.False);
            Assert.That(result1.Message, Is.EqualTo("Borrower has more than 3 checked-out items."));
            Assert.That(result2.Ok, Is.True);
            Assert.That(result3.Ok, Is.False);
            Assert.That(result3.Message, Is.EqualTo("Borrower has overdue item."));
        }
    }
}
