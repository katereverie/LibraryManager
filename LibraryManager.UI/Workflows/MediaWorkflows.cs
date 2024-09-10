using LibraryManagement.UI.Utilities;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.UI.Workflows
{
    public class MediaWorkflows
    {
        public static void ListMedia(IMediaService service)
        {
            Console.Clear();

            var getTypeResult = service.GetAllMediaTypes();
            if (!getTypeResult.Ok)
            {
                Console.WriteLine(getTypeResult.Message);
                return;
            }

            var typeList = getTypeResult.Data;

            IO.PrintMediaTypeList(typeList);

            int typeID = IO.GetMediaTypeID(typeList);

            var result = service.GetMediaByType(typeID);

            if (!result.Ok)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                IO.PrintMediaList(result.Data);
            }

            IO.AnyKey();
        }

        public static void AddMedia(IMediaService service)
        {
            Console.Clear();

            var getTypeResult = service.GetAllMediaTypes();
            if (!getTypeResult.Ok)
            {
                Console.WriteLine(getTypeResult.Message);
                return;
            }

            var typeList = getTypeResult.Data;

            IO.PrintMediaTypeList(typeList);

            Media newMedia = new Media
            {
                MediaTypeID = IO.GetMediaTypeID(typeList),
                Title = IO.GetRequiredString("Enter media title: "),
                IsArchived = false
            };

            var result = service.AddMedia(newMedia);

            if (result.Ok)
            {
                Console.WriteLine($"new Media with ID: {result.Data} added successfully.");
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            IO.AnyKey();
        }

        public static void EditMedia(IMediaService service)
        {
            Console.Clear();

            var getTypeResult = service.GetAllMediaTypes();
            if (!getTypeResult.Ok)
            {
                Console.WriteLine(getTypeResult.Message);
                return;
            }
            var typeList = getTypeResult.Data;

            IO.PrintMediaTypeList(typeList);
            int typeID = IO.GetMediaTypeID(typeList);

            var getResult = service.GetUnarchivedMediaByType(typeID);
            if (!getResult.Ok)
            {
                Console.WriteLine(getResult.Message);
            }
            else
            {
                var mediaList = getResult.Data;

                IO.PrintMediaList(mediaList);
                int mediaID = IO.GetMediaID(mediaList, "Enter the ID of the media to edit: ");

                var mediaToEdit = mediaList.Find(m => m.MediaID == mediaID);

                mediaToEdit.Title = IO.GetRequiredString("Enter new title: ");
                mediaToEdit.MediaTypeID = IO.GetMediaTypeID(typeList, "Enter new type ID: ");

                var editResult = service.EditMedia(mediaToEdit);
                if (editResult.Ok)
                {
                    Console.WriteLine("Media successfully updated.");
                }
                else
                {
                    Console.WriteLine(editResult.Message);
                }
            }

            IO.AnyKey();
        }

        public static void ArchiveMedia(IMediaService service)
        {
            Console.Clear();

            var getTypeResult = service.GetAllMediaTypes();
            if (!getTypeResult.Ok)
            {
                Console.WriteLine(getTypeResult.Message);
                return;
            }

            var typeList = getTypeResult.Data;
            IO.PrintMediaTypeList(typeList);
            int typeID = IO.GetMediaTypeID(typeList);

            var getMediaResult = service.GetUnarchivedMediaByType(typeID);

            if (!getMediaResult.Ok)
            {
                Console.WriteLine(getMediaResult.Message);
            }
            else
            {
                var mediaList = getMediaResult.Data;
                IO.PrintMediaList(mediaList);
                int mediaID = IO.GetMediaID(mediaList, "Enter the ID of the media to be archived: ");

                var archiveResult = service.ArchiveMedia(mediaID);

                if (archiveResult.Ok)
                {
                    Console.WriteLine("Media successfully archived.");
                }
                else
                {
                    Console.WriteLine(archiveResult.Message);
                }
            }

            IO.AnyKey();
        }

        public static void ViewArchive(IMediaService service)
        {
            Console.Clear();

            var result = service.GetAllArchivedMedia();

            if (result.Ok)
            {
                IO.PrintMediaArchive(result.Data);
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            IO.AnyKey();
        }

        public static void GetMostPopularMediaReport(IMediaService service)
        {
            Console.Clear();

            var result = service.GetTop3MostPopularMedia();

            if (result.Ok)
            {
                IO.PrintMediaReport(result.Data);

            }
            else
            {
                Console.WriteLine(result.Message);
            }

            IO.AnyKey();
        }
    }
}
