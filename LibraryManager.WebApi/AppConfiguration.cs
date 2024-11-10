using LibraryManager.Core;
using LibraryManager.Core.Interfaces;
using LibraryManager.WebAPI;

namespace LibraryManagement.API;

/// <summary>
/// Represents the application's configuration settings, providing methods to access connection strings and database access modes.
/// </summary>
public class AppConfiguration : IAppConfiguration
{
    private IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppConfiguration"/> class.
    /// Loads configuration settings from the "appsettings.json" file and user secrets.
    /// </summary>
    public AppConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();
    }

    /// <summary>
    /// Retrieves the connection string for the library database.
    /// </summary>
    /// <returns>The connection string for the library database.</returns>
    public string GetConnectionString()
    {
        return _configuration["LibraryDb"] ?? "";
    }

    /// <summary>
    /// Retrieves the database access mode based on the configuration settings.
    /// </summary>
    /// <returns>A <see cref="DatabaseAccessMode"/> indicating the configured mode for database access.</returns>
    /// <exception cref="Exception">Thrown when the database access mode is not correctly configured.</exception>
    public DatabaseAccessMode GetDatabaseAccessMode()
    {
        switch (_configuration["DatabaseAccessMode"])
        {
            case "ORM":
                return DatabaseAccessMode.ORM;
            case "SQL":
                return DatabaseAccessMode.DirectSQL;
            default:
                throw new Exception("DatabaseMode configuration key not found!");
        }
    }
}
