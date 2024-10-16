using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Application.Services;

public class BorrowerService : IBorrowerService
{
    private readonly IBorrowerRepository _borrowerRepository;
    
    public BorrowerService(IBorrowerRepository borrowerRepository)
    {
        _borrowerRepository = borrowerRepository;
    }

    public Result<List<Borrower>> GetAllBorrowers()
    {
        try
        {
            var list = _borrowerRepository.GetAll();
            return ResultFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Borrower>>(ex.Message);
        }
    }

    public Result<Borrower> GetBorrower(string email)
    {
        try
        {
            var borrower = _borrowerRepository.GetByEmail(email);

            return borrower != null
                ? ResultFactory.Success(borrower)
                : ResultFactory.Fail<Borrower>($"No Borrower registered with {email} was found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Borrower>(ex.Message);
        }
    }

    public Result<ViewBorrowerDTO> GetBorrowerWithLogs(string email)
    {
        try
        {
            var borrowerDTO = _borrowerRepository.GetByEmailWithLogs(email);

            return borrowerDTO != null
                ? ResultFactory.Success(borrowerDTO)
                : ResultFactory.Fail<ViewBorrowerDTO>($"No borrower registered with {email} was found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<ViewBorrowerDTO>(ex.Message);
        }
    }

    public Result UpdateBorrower(Borrower borrower)
    {
        try
        {
            _borrowerRepository.Update(borrower);

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result AddBorrower(Borrower newBorrower)
    {
        try
        {
            var duplicate = _borrowerRepository.GetByEmail(newBorrower.Email);
            if (duplicate != null)
            {
                return ResultFactory.Fail($"{newBorrower.Email} has already been taken!");
            }

            _borrowerRepository.Add(newBorrower);
            return ResultFactory.Success();
           
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result DeleteBorrower(Borrower borrower)
    {
        try
        {
            _borrowerRepository.Delete(borrower);

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }

    }

    //public Result<List<CheckoutLog>> GetCheckoutLogsByEmail(string email)
    //{
    //    try
    //    {
    //        var list = _borrowerRepository.GetCheckoutLogsByEmail(email);

    //        return list.Any()
    //            ? ResultFactory.Success(list)
    //            : ResultFactory.Fail<List<CheckoutLog>>("Borrower has no checkout logs records.");
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
    //    }
    //}
}
