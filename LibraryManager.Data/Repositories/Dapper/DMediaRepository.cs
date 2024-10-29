using Dapper;
using LibraryManager.Core.DTOs;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace LibraryManager.Data.Repositories.Dapper;

public class DMediaRepository : IMediaRepository
{
    private readonly string _connectionString;

    public DMediaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(Media newMedia)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            cn.Open();

            var command = @"INSERT INTO Media (MediaTypeID, Title, IsArchived)
                                VALUES (@MediaTypeID, @Title, @IsArchived);
                                SELECT SCOPE_IDENTITY();";

            var parameters = new
            {
                newMedia.MediaTypeID,
                newMedia.Title,
                newMedia.IsArchived
            };

            var newID = cn.ExecuteScalar<int>(command, parameters);
            newMedia.MediaID = newID;
        }
    }

    public void Archive(int mediaID)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"UPDATE [Media] SET
                                        IsArchived = @IsArchived
                                WHERE MediaID = @MediaID";

            var parameters = new
            {
                IsArchived = true,
                mediaID
            };

            cn.Execute(command, parameters);
        }
    }
    public List<Media> GetAll()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = "SELECT * FROM Media";

            return cn.Query<Media>(command).ToList();
        }
    }

    public List<Media> GetAllArchived()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT m.*, mt.*
                                FROM Media m
                                INNER JOIN MediaType mt ON mt.MediaTypeID = m.MediaTypeID
                                WHERE m.IsArchived = 1
                                ORDER BY m.MediaTypeID, m.Title";

            return cn.Query<Media>(command).ToList();
        }
    }

    public List<MediaType> GetAllMediaTypes()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT * FROM MediaType";

            return cn.Query<MediaType>(command).ToList();
        }
    }

    public List<Media> GetAllUnarchived()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT * 
                                FROM Media
                                WHERE IsArchived = 0";

            return cn.Query<Media>(command).ToList();
        }
    }

    public Media? GetByID(int mediaID)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT * 
                                FROM Media 
                                WHERE MediaID = @MediaID";

            return cn.QueryFirstOrDefault<Media>(command, new { mediaID });
        }
    }

    public List<Media> GetByType(int mediaTypeID)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT * 
                                FROM Media m
                                INNER JOIN MediaType mt On mt.MediaTypeID = m.MediaTypeID
                                WHERE m.MediaTypeID = @MediaTypeID";

            return cn.Query<Media, MediaType, Media>(
                            command,
                            (m, mt) =>
                            {
                                m.MediaType = mt;
                                return m;
                            },
                            new { mediaTypeID },
                            splitOn: "MediaTypeID"
                            ).ToList();
        }
    }

    public List<TopThreeMedia> GetTopThreeMostPopularMedia()
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT TOP 3 m.MediaID, m.Title, mt.MediaTypeName, COUNT(cl.MediaID) AS CheckoutCount
                                FROM Media m 
                                INNER JOIN MediaType mt ON mt.MediaTypeID = m.MediaTypeID
                                LEFT JOIN CheckoutLog cl ON cl.MediaID = m.MediaID
                                GROUP BY m.MediaID, m.Title, mt.MediaTypeName
                                ORDER BY CheckoutCount DESC";

            return cn.Query<TopThreeMedia>(command).ToList();
        }
    }

    public List<Media> GetUnarchivedByType(int mediaTypeID)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"SELECT m.*, mt.* 
                                FROM Media m
                                INNER JOIN MediaType mt ON mt.MediaTypeID = m.MediaTypeID
                                WHERE m.IsArchived = 0 
                                AND m.MediaTypeID = @MediaTypeID";

            return cn.Query<Media, MediaType, Media>(
                command,
                (m, mt) =>
                {
                    m.MediaType = mt;
                    return m;
                },
                new { mediaTypeID },
                splitOn: "MediaTypeID"
            ).ToList();
        }
    }

    public void Update(Media request)
    {
        using (var cn = new SqlConnection(_connectionString))
        {
            var command = @"UPDATE [Media] SET
                                    MediaTypeID = @MediaTypeID,
                                    Title = @Title
                                WHERE MediaID = @MediaID";
            var parameters = new
            {
                request.MediaTypeID,
                request.Title,
                request.MediaID,
            };

            cn.Execute(command, parameters);
        }
    }
}
