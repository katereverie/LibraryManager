using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Application.Services
{
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

        public Result<int> AddBorrower(Borrower newBorrower)
        {
            try
            {
                var duplicate = _borrowerRepository.GetByEmail(newBorrower.Email);
                if (duplicate != null)
                {
                    return ResultFactory.Fail<int>($"{newBorrower.Email} has already been taken!");
                }

                int newID = _borrowerRepository.Add(newBorrower);
                switch (newID)
                {
                    case -1:
                        return ResultFactory.Fail<int>("New Borrower Registration failed.");
                    default:
                        return ResultFactory.Success(newID);
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<int>(ex.Message);
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

        public Result<List<CheckoutLog>> GetCheckoutLogsByBorrower(Borrower borrower)
        {
            try
            {
                var list = _borrowerRepository.GetCheckoutLogs(borrower);

                return list.Any()
                    ? ResultFactory.Success(list)
                    : ResultFactory.Fail<List<CheckoutLog>>("Borrower has no checkout logs records.");
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
            }
        }
    }
}
