using LibraryManager.UI.Workflows;
using LibraryManager.UI.Interfaces;
using LibraryManager.UI.API;
using LibraryManager.UI.Utilities;

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

    public async Task Run()
    {
        do
        {
            Console.Clear();
            Menus.DisplayMainMenu();

            int choice = IO.GetPositiveInteger("Enter Menu Option (1-4): ");
            switch (choice)
            {
                case 1:
                    await ManageBorrower();
                    break;
                case 2:
                    await ManageMedia();
                    break;
                case 3:
                    await ManageCheckout();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private async Task ManageBorrower()
    {
        do
        {
            Menus.DisplayBorrowerManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-6): ");
            switch (choice)
            {
                case 1:
                    await BorrowerWorkflows.ListAllBorrowers(_borrowerAPIClient);
                    break;
                case 2:
                    await BorrowerWorkflows.ViewBorrower(_borrowerAPIClient);
                    break;
                case 3:
                    await BorrowerWorkflows.EditBorrower(_borrowerAPIClient);
                    break;
                case 4:
                    await BorrowerWorkflows.AddBorrower(_borrowerAPIClient);
                    break;
                case 5:
                    await BorrowerWorkflows.DeleteBorrower(_borrowerAPIClient);
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private async Task ManageMedia()
    {
        do
        {
            Menus.DisplayMediaManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-7): ");
            switch (choice)
            {
                case 1:
                    await MediaWorkflows.ListMedia(_mediaAPIClient);
                    break;
                case 2:
                    await MediaWorkflows.AddMedia(_mediaAPIClient);
                    break;
                case 3:
                    await MediaWorkflows.EditMedia(_mediaAPIClient);
                    break;
                case 4:
                    await MediaWorkflows.ArchiveMedia(_mediaAPIClient);
                    break;
                case 5:
                    await MediaWorkflows.ViewArchive(_mediaAPIClient);
                    break;
                case 6:
                    await MediaWorkflows.GetMostPopularMediaReport(_mediaAPIClient);
                    break;
                case 7:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

        } while (true);
    }

    private async Task ManageCheckout()
    {
        do
        {
            Menus.DisplayCheckoutManagementMenu();

            int choice = IO.GetPositiveInteger("Enter Management Option (1-4): ");
            switch (choice)
            {
                case 1:
                    await CheckoutWorkflows.Checkout(_checkoutAPIClient);
                    break;
                case 2:
                    await CheckoutWorkflows.Return(_checkoutAPIClient);
                    break;
                case 3:
                    await CheckoutWorkflows.CheckoutLog(_checkoutAPIClient);
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
