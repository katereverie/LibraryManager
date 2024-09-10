using Dapper;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace LibraryManager.Data.Repositories.Dapper
{
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
            List<Borrower> list = new List<Borrower>();


            using (var cn = new SqlConnection(_connectionString))
            {
                var command = "SELECT * FROM Borrower";
                list = cn.Query<Borrower>(command).ToList();
            }

            return list;
        }

        public Borrower? GetByEmail(string email)
        {
            Borrower? borrower = null;


            using (var cn = new SqlConnection(_connectionString))
            {
                var command = @"SELECT *
                                FROM Borrower
                                WHERE Email = @Email";

                borrower = cn.QueryFirstOrDefault<Borrower>(command, new { Email = email });
            }

            return borrower;
        }

        public List<CheckoutLog> GetCheckoutLogs(Borrower borrower)
        {
            List<CheckoutLog> logList = new List<CheckoutLog>();


            using (var cn = new SqlConnection(_connectionString))
            {
                var command = @"SELECT cl.CheckoutLogID, cl.BorrowerID, cl.MediaID, cl.CheckoutDate, cl.DueDate, cl.ReturnDate,
                                        m.MediaID, m.MediaTypeID, m.Title, m.IsArchived
                                FROM CheckoutLog cl
                                INNER JOIN Media m ON m.MediaID = cl.MediaID
                                WHERE cl.BorrowerID = @BorrowerID";

                logList = cn.Query<CheckoutLog, Media, CheckoutLog>(
                    command,
                    (cl, m) =>
                    {
                        cl.Media = m;
                        return cl;
                    },
                    new { borrower.BorrowerID },
                    splitOn: "MediaID"
                ).ToList();
            }


            return logList;
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
}
