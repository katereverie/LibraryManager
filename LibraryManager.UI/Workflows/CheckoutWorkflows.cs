namespace LibraryManager.UI.Workflows;

public static class CheckoutWorkflows
{
    public static void Checkout(ICheckoutService serivce)
    {
        Console.Clear();

        Borrower borrower;
        string email = IO.GetRequiredString("Enter Borrower's Email: ");
        var getBorrowerResult = serivce.GetBorrowerByEmail(email);

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
            var getMediaResult = serivce.GetAvailableMedia();

            if (!getMediaResult.Ok)
            {
                Console.WriteLine(getMediaResult.Message);
                IO.AnyKey();
                return;
            }

            var mediaList = getMediaResult.Data;
            IO.PrintAvailableMedia(mediaList);

            int mediaID = IO.GetMediaID(mediaList, "Enter the ID of the media to check out: ");

            var checkoutResult = serivce.CheckoutMedia(mediaID, borrower.BorrowerID);

            Console.WriteLine(checkoutResult.Ok ? "Media successfully checked out." : checkoutResult.Message);
           
            exitChoice = IO.GetCheckoutOption();
        }
    }

    public static void Return(ICheckoutService serivce)
    {
        Console.Clear();

        Borrower borrower;
        string email = IO.GetRequiredString("Enter Borrower's Email: ");
        var getBorrowerResult = serivce.GetBorrowerByEmail(email);

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
            var getCheckoutLogResult = serivce.GetCheckedOutMediaByBorrowerID(borrower.BorrowerID);
            if (!getCheckoutLogResult.Ok)
            {
                Console.WriteLine(getCheckoutLogResult.Message);
                IO.AnyKey();
                return;
            }

            var checkoutLogList = getCheckoutLogResult.Data;

            IO.PrintBorrowersCheckedoutMedia(checkoutLogList);

            int logID = IO.GetCheckoutLogID(checkoutLogList, "Enter the Log ID to return: ");
            var returnResult = serivce.ReturnMedia(logID);

            Console.WriteLine(returnResult.Ok ? "Return successfull" : returnResult.Message);

            if (checkoutLogList.Count == 1)
                break;

            returnOption = IO.GetReturnOption();
        }

        IO.AnyKey();
    }

    public static void CheckoutLog(ICheckoutService serivce)
    {
        Console.Clear();

        var getMediaResult = serivce.GetAllCheckedoutMedia();

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
