using LibraryManagement.Test.MockRepos;
using LibraryManager.Application.Services;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class CheckoutServiceTests
    {
        [Test]
        public void CheckoutMedia_BorrowerHasMoreThanThreeCheckedoutItems()
        {
            var service = new CheckoutService(new MockCheckoutRepo(), new MockMediaRepo());

            // Borrower with ID 1 has 3 unreturned checked out items
            var result = service.CheckoutMedia(6, 1);

            Console.WriteLine(result.Message);
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Message, Is.EqualTo("Borrower has more than 3 checked-out items."));
        }

        [Test]
        public void CheckoutMedia_BorrowerOverdueItems()
        {
            var service = new CheckoutService(new MockCheckoutRepo(), new MockMediaRepo());

            // Borrower with ID 3 has overdue item
            var result = service.CheckoutMedia(6, 3);

            Console.WriteLine(result.Message);

            Assert.That(result.Ok, Is.False);
            Assert.That(result.Message, Is.EqualTo("Borrower has overdue item."));
        }

        [Test]
        public void CheckoutMedia_Success()
        {
            var service = new CheckoutService(new MockCheckoutRepo(), new MockMediaRepo());

            // Borrower with ID 2 has 1 checked out item
            var result = service.CheckoutMedia(6, 2);

            Assert.That(result.Ok, Is.True);
        }
    }
}
