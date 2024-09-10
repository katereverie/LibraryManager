namespace LibraryManagement.UI.Utilities
{
    public class Menus
    {
        public static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Library Manager Main Menu");
            Console.WriteLine("=========================");
            Console.WriteLine("1. Borrower Management");
            Console.WriteLine("2. Media Management");
            Console.WriteLine("3. Checkout Management");
            Console.WriteLine("4. Exit\n");
        }

        public static void DisplayBorrowerManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Borrower Management");
            Console.WriteLine("===================");
            Console.WriteLine("1. List all borrowers");
            Console.WriteLine("2. View a borrower");
            Console.WriteLine("3. Edit a borrower");
            Console.WriteLine("4. Add a borrower");
            Console.WriteLine("5. Delete a borrower");
            Console.WriteLine("6. Go back to previous menu\n");
        }

        public static void DisplayMediaManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Media Management");
            Console.WriteLine("================");
            Console.WriteLine("1. List Media");
            Console.WriteLine("2. Add Media");
            Console.WriteLine("3. Edit Media");
            Console.WriteLine("4. Archive Media");
            Console.WriteLine("5. View Archive");
            Console.WriteLine("6. Most Popular Media Report");
            Console.WriteLine("7. Go back to previous menu\n");
        }

        public static void DisplayCheckoutManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Checkout Management");
            Console.WriteLine("===================");
            Console.WriteLine("1. Checkout");
            Console.WriteLine("2. Return");
            Console.WriteLine("3. Checkout Log");
            Console.WriteLine("4. Go back to previous Menu\n");
        }

        public static void DisplayEditBorrowerOptions()
        {
            Console.WriteLine();
            Console.WriteLine("Edit Options");
            Console.WriteLine("============");
            Console.WriteLine("1. First Name");
            Console.WriteLine("2. Last Name");
            Console.WriteLine("3. Email");
            Console.WriteLine("4. Phone Number");
            Console.WriteLine("5. Edit All Information");
            Console.WriteLine("6. Go back to previous menu\n");
        }

        public static void DisplayCheckoutOptions()
        {
            Console.WriteLine();
            Console.WriteLine("1. Check out another item");
            Console.WriteLine("2. Exit\n");
        }

        public static void DisplayReturnOptions()
        {
            Console.WriteLine();
            Console.WriteLine("1. Return another item");
            Console.WriteLine("2. Exit\n");
        }
    }
}
