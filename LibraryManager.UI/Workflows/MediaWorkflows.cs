using LibraryManager.UI.Interfaces;

namespace LibraryManager.UI.Workflows;

public class MediaWorkflows
{
    public static async Task ListMedia(IMediaAPIClient client)
    {
        Console.Clear();

        var getTypeResult = client.GetAllMediaTypes();
        if (!getTypeResult.Ok)
        {
            Console.WriteLine(getTypeResult.Message);
            return;
        }

        var typeList = getTypeResult.Data;

        IO.PrintMediaTypeList(typeList);

        int typeID = IO.GetMediaTypeID(typeList);

        var result = client.GetMediaByType(typeID);

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

    public static async Task AddMedia(IMediaAPIClient client)
    {
        Console.Clear();

        var getTypeResult = client.GetAllMediaTypes();
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

        var result = client.AddMedia(newMedia);

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

    public static async Task EditMedia(IMediaAPIClient client)
    {
        Console.Clear();

        var getTypeResult = client.GetAllMediaTypes();
        if (!getTypeResult.Ok)
        {
            Console.WriteLine(getTypeResult.Message);
            return;
        }
        var typeList = getTypeResult.Data;

        IO.PrintMediaTypeList(typeList);
        int typeID = IO.GetMediaTypeID(typeList);

        var getResult = client.GetUnarchivedMediaByType(typeID);
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

            var editResult = client.EditMedia(mediaToEdit);
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

    public static async Task ArchiveMedia(IMediaAPIClient client)
    {
        Console.Clear();

        var getTypeResult = client.GetAllMediaTypes();
        if (!getTypeResult.Ok)
        {
            Console.WriteLine(getTypeResult.Message);
            return;
        }

        var typeList = getTypeResult.Data;
        IO.PrintMediaTypeList(typeList);
        int typeID = IO.GetMediaTypeID(typeList);

        var getMediaResult = client.GetUnarchivedMediaByType(typeID);

        if (!getMediaResult.Ok)
        {
            Console.WriteLine(getMediaResult.Message);
        }
        else
        {
            var mediaList = getMediaResult.Data;
            IO.PrintMediaList(mediaList);
            int mediaID = IO.GetMediaID(mediaList, "Enter the ID of the media to be archived: ");

            var archiveResult = client.ArchiveMedia(mediaID);

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

    public static async Task ViewArchive(IMediaAPIClient client)
    {
        Console.Clear();

        var result = client.GetAllArchivedMedia();

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

    public static async Task GetMostPopularMediaReport(IMediaAPIClient client)
    {
        Console.Clear();

        var result = client.GetTop3MostPopularMedia();

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
