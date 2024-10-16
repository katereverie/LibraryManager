using LibraryManager.UI.Models;

namespace LibraryManager.UI.Utilities;

public static class IO
{
    public static void AnyKey()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static int GetPositiveInteger(string prompt)
    {
        int result;

        do
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result))
            {
                if (result > 0)
                {
                    return result;
                }
            }

            Console.WriteLine("Invalid input, must be a positive integer!");
            AnyKey();
        } while (true);
    }

    public static string GetRequiredString(string prompt)
    {
        string? input;

        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("Input is required.");
            AnyKey();
        } while (true);
    }

    public static string GetEditedString(string prompt, string originalString)
    {
        string? input;

        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return originalString;
            }

            return input;
        } while (true);
    }

    public static int GetMediaTypeID(List<MediaType> typeList, string prompt = "Enter media type ID: ")
    {
        do
        {
            int input = GetPositiveInteger(prompt);
            if (typeList.Any(mt => mt.MediaTypeID == input))
            {
                return input;
            }
            Console.WriteLine("Invalid media type ID.");
        } while (true);
    }

    public static int GetMediaID(List<Media> mediaList, string prompt = "Enter media ID: ")
    {
        do
        {
            int input = GetPositiveInteger(prompt);
            if (mediaList.Any(m => m.MediaID == input))
            {
                return input;
            }
            Console.WriteLine("Invalid media ID.");
        } while (true);
    }

    public static int GetCheckoutLogID(List<CheckoutLog> clList, string prompt = "Enter checkout log ID: ")
    {
        do
        {
            int input = GetPositiveInteger(prompt);
            if (clList.Any(cl => cl.CheckoutLogID == input))
            {
                return input;
            }
            Console.WriteLine("Invalid checkout log ID.");
        } while (true);
    }

    public static int GetCheckoutOption()
    {
        do
        {
            Menus.DisplayCheckoutOptions();
            int input = GetPositiveInteger("Enter choice (1-2): ");
            if (input >= 1 && input <= 2)
            {
                return input;
            }
            Console.WriteLine("Invalid choice.");
        } while (true);
    }

    public static int GetReturnOption()
    {
        do
        {
            Menus.DisplayReturnOptions();
            int input = GetPositiveInteger("Enter choice (1-2): ");
            if (input >= 1 && input <= 2)
            {
                return input;
            }
            Console.WriteLine("Invalid choice.");
        } while (true);
    }

    public static void PrintBorrowerList(List<Borrower> list)
    {
        PrintHeader(" Borrower List ");
        Console.WriteLine($"{"ID",-5} {"Name",-32} Email");
        Console.WriteLine(new string('=', 100));
        foreach (var b in list)
        {
            Console.WriteLine($"{b.BorrowerID,-5} {b.LastName + ", " + b.FirstName,-32} {b.Email}");
        }
        Console.WriteLine();
    }

    public static void PrintBorrowerInformation(ViewBorrowerDTO borrowerDTO)
    {
        PrintHeader(" Borrower Information ");
        Console.WriteLine($"Id: {borrowerDTO.BorrowerID}");
        Console.WriteLine($"Name: {borrowerDTO.LastName}, {borrowerDTO.FirstName}");
        Console.WriteLine($"Email: {borrowerDTO.Email}");
        Console.WriteLine();

        if (borrowerDTO.CheckoutLogs == null)
        {
            Console.WriteLine("No checkout records.\n");
            return;
        }

        var logDTOs = borrowerDTO.CheckoutLogs.FindAll(cl => cl.ReturnDate == null);
        PrintHeader(" Checkout Record ");
        Console.WriteLine($"{"Media ID",-10} {"Title",-40} {"Checkout Date",-20} {"Return Date",-20}");
        Console.WriteLine(new string('=', 100));
        foreach (var log in logDTOs)
        {
            Console.WriteLine($"{log.MediaID,-10} " +
                $"{log.Title,-40} " +
                $"{log.CheckoutDate,-20:MM/dd/yyyy} " +
                $"{(log.ReturnDate == null ? "Unreturned" : log.ReturnDate),-20:MM/dd/yyyy}");
        }
        Console.WriteLine();
    }

    public static void PrintMediaList(List<Media> list)
    {
        PrintHeader($" {list[0].MediaType.MediaTypeName} List ");
        Console.WriteLine($"{"Media ID",-10} {"Type ID",-10} {"Title",-35} {"Status",-15}");
        Console.WriteLine(new string('=', 100));
        foreach (var m in list)
        {
            Console.WriteLine($"{m.MediaID,-10} " +
                              $"{m.MediaTypeID,-10} " +
                              $"{m.Title,-35} " +
                              $"{(m.IsArchived ? "Archived" : "Available"),-15}");
        }
        Console.WriteLine();
    }

    public static void PrintMediaArchive(List<Media> list)
    {
        PrintHeader($" Media Archive ");
        Console.WriteLine($"{"Media ID",-10} {"Type ID",-10} {"Title",-35}");
        Console.WriteLine(new string('=', 100));
        foreach (var m in list)
        {
            Console.WriteLine($"{m.MediaID,-10} " +
                              $"{m.MediaTypeID,-10} " +
                              $"{m.Title,-35} ");
        }
        Console.WriteLine();
    }

    public static void PrintMediaTypeList(List<MediaType> list)
    {
        PrintHeader($" Media Type List ");
        Console.WriteLine($"{"Type ID",-10} {"Type Name",-20}");
        Console.WriteLine(new string('=', 100));
        foreach (var mt in list)
        {
            Console.WriteLine($"{mt.MediaTypeID,-10} " +
                              $"{mt.MediaTypeName,-20} ");
        }
        Console.WriteLine();
    }

    public static void PrintAvailableMedia(List<Media> list)
    {
        PrintHeader($" Available Media List ");
        Console.WriteLine($"{"Media ID",-10} {"Type ID",-10} {"Title",-35}");
        Console.WriteLine(new string('=', 100));
        foreach (var m in list)
        {
            Console.WriteLine($"{m.MediaID,-10} " +
                              $"{m.MediaTypeID,-10} " +
                              $"{m.Title,-35} ");
        }
        Console.WriteLine();
    }

    public static void PrintMediaReport(List<TopThreeMedia> list)
    {
        PrintHeader(" Top 3 Most Popular Media List ");
        Console.WriteLine($"\n{"Media ID",-10} {"Type",-20} {"Title",-35} {"Checkout Count",-15}");
        Console.WriteLine(new string('=', 100));
        foreach (var m in list)
        {
            Console.WriteLine($"{m.MediaID,-10} " +
                    $"{m.MediaTypeName,-20} " +
                    $"{m.Title,-35} " +
                    $"{m.CheckoutCount,-15}");
        }
    }

    public static void PrintCheckoutLogList(List<CheckoutLog> list)
    {
        PrintHeader($" Checkout Log List");
        Console.WriteLine($"{"Name",-20} {"Email",-30} {"Title",-30} {"Checkout Date",-20} {"Due Date",-20}");
        Console.WriteLine(new string('=', 120));
        foreach (var log in list)
        {
            Console.Write($"{log.Borrower.LastName + ", " + log.Borrower.FirstName,-20} " +
                          $"{log.Borrower.Email,-30} " +
                          $"{log.Media.Title,-30} " +
                          $"{log.CheckoutDate,-20:MM/dd/yyyy} ");
            if (log.DueDate < DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{log.DueDate,-20:MM/dd/yyyy}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{log.DueDate,-20:MM/dd/yyyy}");
            }
        }
        Console.WriteLine();
    }

    public static void PrintBorrowersCheckedoutMedia(List<CheckoutLog> list)
    {
        PrintHeader($" Checkedout Media List ");
        Console.WriteLine($"{"Log ID",-10} {"Title",-30}");
        Console.WriteLine(new string('=', 100));
        foreach (var cl in list)
        {
            Console.WriteLine($"{cl.CheckoutLogID,-10} " +
                              $"{cl.Media.Title,-30} ");
        }
        Console.WriteLine();
    }

    public static void PrintHeader(string header)
    {
        string headerSpace = new string(' ', (100 - header.Length) / 2);
        Console.WriteLine("\n" + headerSpace + header + headerSpace + "\n");
    }
}
