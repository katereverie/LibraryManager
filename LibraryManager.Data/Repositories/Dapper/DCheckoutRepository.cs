﻿using Dapper;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace LibraryManager.Data.Repositories.Dapper;

public class DCheckoutRepository : ICheckoutRepository
{
    private readonly string _connectionString;

    public DCheckoutRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Add(CheckoutLog newCheckoutLog)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"INSERT INTO CheckoutLog (BorrowerID, MediaID, CheckoutDate, DueDate, ReturnDate)
                                VALUES (@BorrowerID, @MediaID, @CheckoutDate, @DueDate, @ReturnDate)
                                SELECT SCOPE_IDENTITY()";

            var parameters = new
            {
                newCheckoutLog.BorrowerID,
                newCheckoutLog.MediaID,
                newCheckoutLog.CheckoutDate,
                newCheckoutLog.DueDate,
                newCheckoutLog.ReturnDate
            };

            return cn.ExecuteScalar<int>(command, parameters);
        }
    }

    public List<CheckoutLog> GetAllCheckedoutMedia()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT cl.CheckoutLogID, cl.BorrowerID, cl.MediaID, cl.CheckoutDate, cl.DueDate, cl.ReturnDate,
                                        b.BorrowerID, b.FirstName, b.LastName, b.Email, b.Phone,
                                        m.MediaID, m.MediaTypeID, m.Title, m.IsArchived
                                FROM CheckoutLog cl
                                INNER JOIN Borrower b ON b.BorrowerID = cl.BorrowerID
                                INNER JOIN Media m ON m.MediaID = cl.MediaID
                                WHERE cl.ReturnDate IS NULL";


            return cn.Query<CheckoutLog, Borrower, Media, CheckoutLog>(
                            command,
                            (cl, borrower, media) =>
                            {
                                cl.Borrower = borrower;
                                cl.Media = media;
                                return cl;
                            },
                            splitOn: "BorrowerID,MediaID"
                            ).ToList();
        }
    }

    public List<CheckoutLog> GetCheckoutLogsByBorrowerEmail(string email)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT *
                                FROM CheckoutLog cl
                                INNER JOIN Borrower b ON b.BorrowerID = cl.BorrowerID
                                WHERE b.Email = @email";

            return cn.Query<CheckoutLog>(command, new { email }).ToList();
        }
    }

    public List<Media> GetAvailableMedia()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT m.*
                                FROM Media m
                                LEFT JOIN (
                                    SELECT cl.MediaID, MAX(cl.CheckoutLogID) AS LatestCheckoutLogID
                                    FROM CheckoutLog cl
                                    GROUP BY cl.MediaID
                                ) LatestLog ON m.MediaID = LatestLog.MediaID
                                LEFT JOIN CheckoutLog cl ON LatestLog.LatestCheckoutLogID = cl.CheckoutLogID
                                WHERE m.IsArchived = 0
                                AND (LatestLog.MediaID IS NULL OR cl.ReturnDate IS NOT NULL)";

            return cn.Query<Media>(command).ToList();
        }
    }

    public void Update(int checkoutLogID)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"UPDATE [CheckoutLog] SET
                                        ReturnDate = @ReturnDate
                                WHERE CheckoutLogID = @CheckoutLogID";

            var parameters = new
            {
                ReturnDate = DateTime.Now,
                checkoutLogID
            };

            cn.Execute(command, parameters);
        }
    }
}
