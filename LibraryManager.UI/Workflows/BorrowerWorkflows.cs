using LibraryManager.UI.Interfaces;

namespace LibraryManager.UI.Workflows;

public static class BorrowerWorkflows
{
    public static void ListAllBorrowers(IBorrowerAPIClient client)
    {
        Console.Clear();

        var result = client.GetAllBorrowers();

        if (!result.Ok)
        {
            Console.WriteLine(result.Message);
        }
        else if (result.Data.Any())
        {
            IO.PrintBorrowerList(result.Data);
        }
        else
        {
            Console.WriteLine("Currently, there's no registered borrower.");
        }

        IO.AnyKey();
    }

    public static void ViewBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        var email = IO.GetRequiredString("Enter borrower email: ");

        var getBorrowerResult = client.GetBorrower(email);
        if (!getBorrowerResult.Ok)
        {
            Console.WriteLine(getBorrowerResult.Message);
            IO.AnyKey();
            return;
        }

        IO.PrintBorrowerInformation(getBorrowerResult.Data);

        var getCheckoutLogsResult = client.GetCheckoutLogsByBorrower(getBorrowerResult.Data);
        if (!getCheckoutLogsResult.Ok)
        {
            Console.WriteLine(getCheckoutLogsResult.Message);
        }
        else
        {
            IO.PrintBorrowerCheckoutLog(getCheckoutLogsResult.Data);
        }

        IO.AnyKey();
    }

    public static void AddBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        Borrower newBorrower = new Borrower
        {
            FirstName = IO.GetRequiredString("First Name: "),
            LastName = IO.GetRequiredString("Last Name: "),
            Email = IO.GetRequiredString("Email: "),
            Phone = IO.GetRequiredString("Phone: ")
        };

        var addResult = client.AddBorrower(newBorrower);

        if (addResult.Ok)
        {
            Console.WriteLine($"New Borrower successfully registered.");
        }
        else
        {
            Console.WriteLine(addResult.Message);
        }

        IO.AnyKey();
    }

    public static void EditBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        var getBorrowerResult = client.GetBorrower(IO.GetRequiredString("Enter the Email of the Borrower to be edited: "));

        if (!getBorrowerResult.Ok)
        {
            Console.WriteLine(getBorrowerResult.Message);
        }
        else
        {
            Borrower b = getBorrowerResult.Data;
            int option = 0;

            while (true)
            {
                Console.WriteLine("\nYou have the following edit options.");
                Menus.DisplayEditBorrowerOptions();
                option = IO.GetPositiveInteger("Enter edit options (1-5) or return (6): ");

                if (option >= 1 && option <= 6)
                {
                    break;
                }

                Console.WriteLine("Invalid option.");
                IO.AnyKey();
            }


            switch (option)
            {
                case 1:
                    b.FirstName = IO.GetRequiredString("Enter new First Name: ");
                    break;
                case 2:
                    b.LastName = IO.GetRequiredString("Enter new Last Name: ");
                    break;
                case 3:
                    while (true)
                    {
                        string newEmail = IO.GetRequiredString("Enter new Email address: ");
                        var duplicateResult = client.GetBorrower(newEmail);
                        if (duplicateResult.Data == null)
                        {
                            b.Email = newEmail;
                            break;
                        }
                        Console.WriteLine($"{newEmail} has already been taken.");
                    }
                    break;
                case 4:
                    b.Phone = IO.GetRequiredString("Enter new phone number: ");
                    break;
                case 5:
                    b.FirstName = IO.GetRequiredString("Enter new First name: ");
                    b.LastName = IO.GetRequiredString("Enter new Last name:");
                    while (true)
                    {
                        string newEmail = IO.GetRequiredString("Enter new Email address: ");
                        var duplicateResult = client.GetBorrower(newEmail);
                        if (duplicateResult.Data == null)
                        {
                            b.Email = newEmail;
                            break;
                        }
                        Console.WriteLine($"{newEmail} has already been taken.");
                    }
                    b.Phone = IO.GetRequiredString("Enter new phone number:");
                    break;
                case 6:
                    return;
            }

            var updateResult = client.UpdateBorrower(b);

            if (updateResult.Ok)
            {
                Console.WriteLine($"Borrower successfully edited.");
            }
            else
            {
                Console.WriteLine(updateResult.Message);
            }
        }

        IO.AnyKey();
    }

    public static void DeleteBorrower(IBorrowerAPIClient client)
    {
        Console.Clear();

        var getResult = client.GetBorrower(IO.GetRequiredString("Enter the Email of the borrower to be deleted: "));

        if (!getResult.Ok)
        {
            Console.WriteLine(getResult.Message);
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
                    var deleteResult = client.DeleteBorrower(getResult.Data);
                    if (deleteResult.Ok)
                    {
                        Console.WriteLine("Borrower successfully deleted.");
                    }
                    else
                    {
                        Console.WriteLine(deleteResult.Message);
                    }
                    break;
                case 2:
                    Console.WriteLine("Delete Process cancelled.");
                    break;
            }
        }

        IO.AnyKey();
    }
}
