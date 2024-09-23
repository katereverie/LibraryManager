﻿using Dapper;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace LibraryManager.Data.Repositories.Dapper
{
    public class DMediaRepository : IMediaRepository
    {
        private readonly string _connectionString;

        public DMediaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(Media newMedia)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                var command = @"INSERT INTO Media (MediaTypeID, Title, IsArchived)
                                VALUES (@MediaTypeID, @Title, @IsArchived);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    newMedia.MediaTypeID,
                    newMedia.Title,
                    newMedia.IsArchived
                };

                return cn.ExecuteScalar<int>(command, parameters);
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
            throw new NotImplementedException();
        }

        public List<Media> GetAllArchived()
        {
            throw new NotImplementedException();
        }

        public List<MediaType> GetAllMediaTypes()
        {
            throw new NotImplementedException();
        }

        public List<Media> GetAllUnarchived()
        {
            List<Media> list = new();

            using (var cn = new SqlConnection(_connectionString))
            {
                var command = @"SELECT * 
                                FROM Media
                                WHERE IsArchived = 0";

                list = cn.Query<Media>(command).ToList();
            }

            return list;
        }

        public Media? GetByID(int mediaID)
        {
            Media? media;

            using (var cn = new SqlConnection(_connectionString))
            {
                var command = @"SELECT * 
                                FROM Media 
                                WHERE MediaID = @MediaID";

                media = cn.QueryFirstOrDefault<Media>(command, new { mediaID });
            }

            return media;
        }

        public List<Media> GetByType(int mediaTypeID)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var command = @"SELECT * 
                                FROM Media m
                                INNER JOIN MediaType mt On mt.MediaTypeID = m.MediaTypeID
                                WHERE m.MediaTypeID = @MediaTypeID";

                var media = cn.Query<Media, MediaType, Media>(
                    command,
                    (m, mt) =>
                    {
                        m.MediaType = mt;
                        return m;
                    },
                    new { mediaTypeID },
                    splitOn: "MediaTypeID"
                ).ToList();

                return media;
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

                var media = cn.Query<Media, MediaType, Media>(
                    command,
                    (m, mt) =>
                    {
                        m.MediaType = mt;
                        return m;
                    },
                    new { mediaTypeID },
                    splitOn: "MediaTypeID"
                ).ToList();

                return media;
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
}