using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using LibraryManager.UI.Utilities;

namespace LibraryManager.UI.Workflows;

public static class BorrowerWorkflows
{
    public static async Task ListAllBorrowers(IBorrowerAPIClient client)
    {
        Console.Clear();

        try
        {
            var borrowers = await client.GetAllBorrowersAsync();

            if (borrowers.Any())
                IO.PrintBorrowerList(borrowers);
            else
                Console.WriteLine("Currently, there's no registered borrower.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task ViewBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        try
        {
            var email = IO.GetRequiredString("Enter borrower email: ");

            var borrowerDTO = await client.GetBorrowerWithLogsAsync(email);

            Console.Clear();
            if (borrowerDTO == null)
            {
                Console.WriteLine($"Borrower with the email address: {email} not found.");
            }
            else
            {
                IO.PrintBorrowerInformation(borrowerDTO);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\nError: {ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task AddBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        try
        {
            var newBorrower = new AddBorrowerRequest
            {
                FirstName = IO.GetRequiredString("First Name: "),
                LastName = IO.GetRequiredString("Last Name: "),
                Email = IO.GetRequiredString("Email: "),
                Phone = IO.GetRequiredString("Phone: ")
            };

            await client.AddBorrowerAsync(newBorrower);
            Console.WriteLine($"New Borrower successfully registered.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n Error: {ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task EditBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        try
        {
            var email = IO.GetRequiredString("Enter the Email of the Borrower to be edited: ");
            var borrower = await client.GetBorrowerAsync(email);

            if (borrower == null)
            {
                Console.WriteLine($"Borrower with the email address: {email} not found.");
            }
            else
            {
                Console.WriteLine("Edit Borrower (Press \"Enter\" to skip)");
                var editedBorrower = new EditBorrowerRequest
                {
                    BorrowerID = borrower.BorrowerID,
                    FirstName = IO.GetEditedString($"First Name ({borrower.FirstName}): ", borrower.FirstName),
                    LastName = IO.GetEditedString($"Last Name ({borrower.LastName}): ", borrower.LastName),
                    Email = IO.GetEditedString($"Email ({borrower.Email}): ", borrower.Email),
                    Phone = IO.GetEditedString($"Phone ({borrower.Phone}): ", borrower.Phone),
                };

                
                await client.EditBorrowerAsync(editedBorrower);
                Console.WriteLine($"Borrower successfully edited.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n Error: {ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task DeleteBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        try
        {
            var email = IO.GetRequiredString("Enter the Email of the borrower to be deleted: ");
            var borrower = await client.GetBorrowerAsync(email);

            if (borrower == null)
            {
                Console.WriteLine($"Borrower with the email address: {email} not found.");
            }
            else
            {
                int choice = 0;

                do
                {
                    Console.WriteLine("Deleting a borrower will result in erasing all information related to the borrower.");
                    Console.WriteLine("Are you sure you'd like to proceed?\n1. Proceed\n2. Cancel");
                    choice = IO.GetPositiveInteger("Enter choice: ");
                    if (choice != 1 && choice != 2)
                    {
                        Console.WriteLine("Invalid choice. Please enter either 1 or 2.");
                        continue;
                    }
                    break;
                } while (true);

                switch (choice)
                {
                    case 1:
                        await client.DeleteBorrowerAsync(borrower.BorrowerID);
                        Console.WriteLine("Borrower successfully deleted.");
                        break;
                    case 2:
                        Console.WriteLine("Delete Process cancelled.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n Error: {ex.Message}");
        }

        IO.AnyKey();
    }
}
