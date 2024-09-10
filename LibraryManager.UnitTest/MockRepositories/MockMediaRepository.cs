using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManagement.Test.MockRepos
{
    public class MockMediaRepo : IMediaRepository
    {
        public int Add(Media newMedia)
        {
            throw new NotImplementedException();
        }

        public void Archive(int mediaID)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Media? GetByID(int mediaId)
        {
            throw new NotImplementedException();
        }

        public List<Media> GetByType(int mediaTypeID)
        {
            throw new NotImplementedException();
        }

        public List<TopThreeMedia> GetTopThreeMostPopularMedia()
        {
            throw new NotImplementedException();
        }

        public List<Media> GetUnarchivedByType(int typeID)
        {
            throw new NotImplementedException();
        }

        public void Update(Media request)
        {
            throw new NotImplementedException();
        }
    }
}
