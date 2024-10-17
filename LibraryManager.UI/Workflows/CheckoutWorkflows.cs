using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Utilities;

namespace LibraryManager.UI.Workflows;

public static class CheckoutWorkflows
{
    public static async Task CheckoutMedia(ICheckoutAPIClient client)
    {
        Console.Clear();

        try
        {
            var email = IO.GetRequiredString("Enter Borrower's Email: ");
            int exitChoice = 0;
            while (exitChoice != 2)
            {
                var availableMedias = await client.GetAvailableMediaAsync();
                if (!availableMedias.Any())
                {
                    Console.WriteLine("No available media items found.");
                    break;
                }
                IO.PrintAvailableMedia(availableMedias);
                int mediaID = IO.GetMediaID(availableMedias, "Enter the ID of the media to check out: ");
                await client.CheckoutMediaAsync(mediaID, email);
                Console.WriteLine("Media successfully checked out.");
                exitChoice = IO.GetCheckoutOption();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task ReturnMedia(ICheckoutAPIClient client)
    {
        Console.Clear();

        try
        {
            var email = IO.GetRequiredString("Enter Borrower's Email: ");

            int returnOption = 0;
            while (returnOption != 2)
            {
                var currentCheckoutLogs = await client.GetCurrentCheckoutLogsAsync();

                var borrowerCheckoutLogs = currentCheckoutLogs.FindAll(cl => cl.Borrower.Email == email);

                if (borrowerCheckoutLogs.Any())
                {
                    IO.PrintBorrowersCheckedoutMedia(borrowerCheckoutLogs);
                    int logID = IO.GetCheckoutLogID(borrowerCheckoutLogs, "Enter the Log ID to return: ");
                    await client.ReturnMediaAsync(logID);
                    Console.WriteLine($"Media successfully returned.");
                    if (borrowerCheckoutLogs.Count == 1)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"Either borrower with {email} is not registered or borrower has not checked out any media items");
                    break;
                }

                returnOption = IO.GetReturnOption();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        IO.AnyKey();
    }

    public static async Task GetCurrentCheckoutLogs(ICheckoutAPIClient client)
    {
        Console.Clear();

        try
        {
            var currentCheckoutLogs = await client.GetCurrentCheckoutLogsAsync();

            if (currentCheckoutLogs.Any())
            {
                IO.PrintCheckoutLogList(currentCheckoutLogs);
            }
            else
            {
                Console.WriteLine("No checkout logs found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        IO.AnyKey();
    }
}
