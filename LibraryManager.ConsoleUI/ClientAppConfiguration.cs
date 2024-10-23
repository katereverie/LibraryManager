using LibraryManager.UI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LibraryManager.UI;

public class ClientAppConfiguration : IClientAppConfiguration
{
    private IConfiguration _configuration;

    public ClientAppConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddUserSecrets<Program>()
            .Build();
    }

    public string GetBaseUrl()
    {
        return _configuration["BaseUrl"] ?? throw new Exception("Base URL configuration key missing");
    }
}
