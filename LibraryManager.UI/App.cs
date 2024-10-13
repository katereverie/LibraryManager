using LibraryManager.UI.Workflows;
using LibraryManager.UI.Interfaces;
using LibraryManager.UI.API;

namespace LibraryManager.UI;

public class App
{
    private IClientAppConfiguration _clientConfig;
    private HttpClient _httpClient;
    
    private IBorrowerAPIClient _borrowerAPIClient;
    private IMediaAPIClient _mediaAPIClient;
    private ICheckoutAPIClient _checkoutAPIClient;

    public App()
    {
        _clientConfig = new ClientAppConfiguration();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_clientConfig.GetBaseUrl())
        };

        _borrowerAPIClient = APIClientFactory.GetBorrowerClient(_httpClient);
        _mediaAPIClient = APIClientFactory.GetMediaClient(_httpClient);
        _checkoutAPIClient = APIClientFactory.GetCheckoutClient(_httpClient);
    }

    public void Run()
    {
        do
        {
            Console.Clear();
            Menus.DisplayMainMenu();

            int choice = IO.GetPositiveInteger("Enter Menu Option (1-4): ");
            switch (choice)
            {
                case 1:
                    ManageBorrower();
                    break;
                case 2:
                    ManageMedia();
                    break;
                case 3:
                    ManageCheckout();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private void ManageBorrower()
    {
        do
        {
            Menus.DisplayBorrowerManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-6): ");
            switch (choice)
            {
                case 1:
                    BorrowerWorkflows.ListAllBorrowers(_serviceFactory.CreateBorrowerService());
                    break;
                case 2:
                    BorrowerWorkflows.ViewBorrower(_serviceFactory.CreateBorrowerService());
                    break;
                case 3:
                    BorrowerWorkflows.EditBorrower(_serviceFactory.CreateBorrowerService());
                    break;
                case 4:
                    BorrowerWorkflows.AddBorrower(_serviceFactory.CreateBorrowerService());
                    break;
                case 5:
                    BorrowerWorkflows.DeleteBorrower(_serviceFactory.CreateBorrowerService());
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private void ManageMedia()
    {
        do
        {
            Menus.DisplayMediaManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-7): ");
            switch (choice)
            {
                case 1:
                    MediaWorkflows.ListMedia(_serviceFactory.CreateMediaService());
                    break;
                case 2:
                    MediaWorkflows.AddMedia(_serviceFactory.CreateMediaService());
                    break;
                case 3:
                    MediaWorkflows.EditMedia(_serviceFactory.CreateMediaService());
                    break;
                case 4:
                    MediaWorkflows.ArchiveMedia(_serviceFactory.CreateMediaService());
                    break;
                case 5:
                    MediaWorkflows.ViewArchive(_serviceFactory.CreateMediaService());
                    break;
                case 6:
                    MediaWorkflows.GetMostPopularMediaReport(_serviceFactory.CreateMediaService());
                    break;
                case 7:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private void ManageCheckout()
    {
        do
        {
            Menus.DisplayCheckoutManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-4): ");
            switch (choice)
            {
                case 1:
                    CheckoutWorkflows.Checkout(_serviceFactory.CreateCheckoutService());
                    break;
                case 2:
                    CheckoutWorkflows.Return(_serviceFactory.CreateCheckoutService());
                    break;
                case 3:
                    CheckoutWorkflows.CheckoutLog(_serviceFactory.CreateCheckoutService());
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }
}
