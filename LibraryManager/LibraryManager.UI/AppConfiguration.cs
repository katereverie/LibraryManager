using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI
{
    public class AppConfiguration : IAppConfiguration
    {
        private IConfiguration _configuration;

        public AppConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
        }

        public string GetConnectionString()
        {
            return _configuration["LibraryDb"] ?? "";
        }

        public DatabaseAccessMode GetDatabaseAccessMode()
        {
            switch (_configuration["DatabaseMode"])
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
}
