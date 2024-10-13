using LibraryManager.UI.Interfaces;

namespace LibraryManager.UI.Workflows;

public static class CheckoutWorkflows
{
    public static async Task Checkout(ICheckoutAPIClient client)
    {
        Console.Clear();

        Borrower borrower;
        string email = IO.GetRequiredString("Enter Borrower's Email: ");
        var getBorrowerResult = client.GetBorrowerByEmail(email);

        if (!getBorrowerResult.Ok)
        {
            Console.WriteLine(getBorrowerResult.Message);
            IO.AnyKey();
            return;
        }

        borrower = getBorrowerResult.Data;

        int exitChoice = 0;
        while (exitChoice != 2)
        {
            var getMediaResult = client.GetAvailableMedia();

            if (!getMediaResult.Ok)
            {
                Console.WriteLine(getMediaResult.Message);
                IO.AnyKey();
                return;
            }

            var mediaList = getMediaResult.Data;
            IO.PrintAvailableMedia(mediaList);

            int mediaID = IO.GetMediaID(mediaList, "Enter the ID of the media to check out: ");

            var checkoutResult = client.CheckoutMedia(mediaID, borrower.BorrowerID);

            Console.WriteLine(checkoutResult.Ok ? "Media successfully checked out." : checkoutResult.Message);
           
            exitChoice = IO.GetCheckoutOption();
        }
    }

    public static async Task Return(ICheckoutAPIClient client)
    {
        Console.Clear();

        Borrower borrower;
        string email = IO.GetRequiredString("Enter Borrower's Email: ");
        var getBorrowerResult = client.GetBorrowerByEmail(email);

        if (!getBorrowerResult.Ok)
        {
            Console.WriteLine(getBorrowerResult.Message);
            IO.AnyKey();
            return;
        }

        borrower = getBorrowerResult.Data;

        int returnOption = 0;
        while (returnOption != 2)
        {
            var getCheckoutLogResult = client.GetCheckedOutMediaByBorrowerID(borrower.BorrowerID);
            if (!getCheckoutLogResult.Ok)
            {
                Console.WriteLine(getCheckoutLogResult.Message);
                IO.AnyKey();
                return;
            }

            var checkoutLogList = getCheckoutLogResult.Data;

            IO.PrintBorrowersCheckedoutMedia(checkoutLogList);

            int logID = IO.GetCheckoutLogID(checkoutLogList, "Enter the Log ID to return: ");
            var returnResult = client.ReturnMedia(logID);

            Console.WriteLine(returnResult.Ok ? "Return successfull" : returnResult.Message);

            if (checkoutLogList.Count == 1)
                break;

            returnOption = IO.GetReturnOption();
        }

        IO.AnyKey();
    }

    public static async Task CheckoutLog(ICheckoutAPIClient client)
    {
        Console.Clear();

        var getMediaResult = client.GetAllCheckedoutMedia();

        if (!getMediaResult.Ok)
        {
            Console.WriteLine(getMediaResult.Message);
        }
        else
        {
            IO.PrintCheckoutLogList(getMediaResult.Data);
        }

        IO.AnyKey();
    }
}
