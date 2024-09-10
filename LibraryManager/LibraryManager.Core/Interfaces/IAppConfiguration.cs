using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface IAppConfiguration
    {
        string GetConnectionString();
        DatabaseAccessMode GetDatabaseAccessMode();
    }
}
