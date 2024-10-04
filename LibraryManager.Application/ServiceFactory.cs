using LibraryManager.Application.Services;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.Data.Repositories.Dapper;
using LibraryManager.Data.Repositories.EntityFramework;

namespace LibraryManager.Application;

public class ServiceFactory
{
    private IAppConfiguration _appConfig;

    public ServiceFactory(IAppConfiguration appConfig)
    {
        _appConfig = appConfig;
    }

    public IBorrowerService CreateBorrowerService()
    {
        switch (_appConfig.GetDatabaseAccessMode())
        {
            case DatabaseAccessMode.ORM:
                return new BorrowerService(new EFBorrowerRepository(_appConfig.GetConnectionString()));
            default:
                return new BorrowerService(new DBorrowerRepository(_appConfig.GetConnectionString()));
        }
    }

    public IMediaService CreateMediaService()
    {
        switch (_appConfig.GetDatabaseAccessMode())
        {
            case DatabaseAccessMode.ORM:
                return new MediaService(new EFMediaRepository(_appConfig.GetConnectionString()));
            default:
                return new MediaService(new DMediaRepository(_appConfig.GetConnectionString()));
        }
    }

    public ICheckoutService CreateCheckoutService()
    {
        switch (_appConfig.GetDatabaseAccessMode())
        {
            case DatabaseAccessMode.ORM:
                return new CheckoutService(new EFCheckoutRepository(_appConfig.GetConnectionString()), new EFMediaRepository(_appConfig.GetConnectionString()));
            default:
                return new CheckoutService(new DCheckoutRepository(_appConfig.GetConnectionString()), new DMediaRepository(_appConfig.GetConnectionString()));
        }
    }
}
