using Dapper;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace LibraryManager.Data.Repositories.Dapper;

public class DBorrowerRepository : IBorrowerRepository
{
    private readonly string _connectionString;

    public DBorrowerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Add(Borrower newBorrower)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"INSERT INTO Borrower (FirstName, LastName, Email, Phone)
                                VALUES (@FirstName, @LastName, @Email, @Phone)
                                SELECT SCOPE_IDENTITY()";
            var parameters = new
            {
                newBorrower.FirstName,
                newBorrower.LastName,
                newBorrower.Email,
                newBorrower.Phone
            };

            try
            {
                return cn.ExecuteScalar<int>(command, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }

    public void Delete(Borrower borrower)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            cn.Open();

            using (var transaction = cn.BeginTransaction())
            {

                var deleteCheckoutLogsCommand = "DELETE FROM CheckoutLog WHERE BorrowerID = @BorrowerID";
                cn.Execute(deleteCheckoutLogsCommand, new { borrower.BorrowerID }, transaction);

                var deleteBorrowerCommand = "DELETE FROM Borrower WHERE BorrowerID = @BorrowerID";
                cn.Execute(deleteBorrowerCommand, new { borrower.BorrowerID }, transaction);

                transaction.Commit();
            }
        }
    }

    public List<Borrower> GetAll()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = "SELECT * FROM Borrower";

            return cn.Query<Borrower>(command).ToList();
        }
    }

    public Borrower? GetByEmail(string email)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT *
                            FROM Borrower
                            WHERE Email = @Email";

            return cn.QueryFirstOrDefault<Borrower>(command, new { Email = email });
        }
    }

    public ViewBorrowerDTO? GetByEmailWithLogs(string email)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"
                SELECT b.BorrowerID, b.FirstName, b.LastName, b.Email,
                       cl.CheckoutDate, cl.ReturnDate, cl.MediaID, m.Title
                FROM Borrower b
                LEFT JOIN CheckoutLog cl ON b.BorrowerID = cl.BorrowerID
                LEFT JOIN Media m ON cl.MediaID = m.MediaID
                WHERE b.Email = @Email";

            var borrowerDict = new Dictionary<int, ViewBorrowerDTO>();

            connection.Query<ViewBorrowerDTO, CheckoutLogDTO, ViewBorrowerDTO>(
                sql,
                (borrower, checkoutLog) =>
                {
                    if (!borrowerDict.TryGetValue(borrower.BorrowerID, out var borrowerEntry))
                    {
                        borrowerEntry = borrower;
                        borrowerEntry.CheckoutLogs = new List<CheckoutLogDTO>();
                        borrowerDict[borrower.BorrowerID] = borrowerEntry;
                    }

                    if (checkoutLog != null)
                    {
                        borrowerEntry.CheckoutLogs.Add(checkoutLog);
                    }

                    return borrowerEntry;
                },
                new { Email = email },
                splitOn: "CheckoutDate"
            );

            return borrowerDict.Values.SingleOrDefault();
        }
    }

    public void Update(Borrower request)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"UPDATE [Borrower] SET
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        Email = @Email,
                                        Phone = @Phone
                                WHERE BorrowerID = @BorrowerID";

            var parameters = new
            {
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.BorrowerID
            };

            cn.Execute(command, parameters);
        }
    }
}
