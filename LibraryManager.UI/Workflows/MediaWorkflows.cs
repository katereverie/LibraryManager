using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using LibraryManager.UI.Utilities;

namespace LibraryManager.UI.Workflows;

public static class MediaWorkflows
{
    public static async Task ListMedia(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var mediaTypes = await client.GetMediaTypesAsync();

            if (mediaTypes.Any())
            {
                IO.PrintMediaTypeList(mediaTypes);
                int typeID = IO.GetMediaTypeID(mediaTypes);
                var selectedMedia = await client.GetMediaByTypeAsync(typeID);
                Console.Clear();
                IO.PrintMediaList(selectedMedia);
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task AddMedia(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var mediaTypes = await client.GetMediaTypesAsync();

            if (mediaTypes.Any())
            {
                IO.PrintMediaTypeList(mediaTypes);
                int typeID = IO.GetMediaTypeID(mediaTypes);
                AddMediaRequest media = new()
                {
                    MediaTypeID = typeID,
                    Title = IO.GetRequiredString("Enter media title: "),
                };

                await client.AddMediaAsync(media);
                Console.WriteLine($"new Media item {media.Title} added successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task EditMedia(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var mediaTypes = await client.GetMediaTypesAsync();
            if (mediaTypes.Any())
            {
                IO.PrintMediaTypeList(mediaTypes);
                int typeID = IO.GetMediaTypeID(mediaTypes);
                var selectedMedias = await client.GetMediaByTypeAsync(typeID);
                Console.Clear();
                IO.PrintMediaList(selectedMedias.FindAll(m => m.IsArchived == false));
                int mediaID = IO.GetMediaID(selectedMedias, "Enter the ID of the media to edit: ");
                var mediaToEdit = selectedMedias.Single(m => m.MediaID == mediaID);
                mediaToEdit.Title = IO.GetRequiredString("Enter new title: ");
                mediaToEdit.MediaTypeID = IO.GetMediaTypeID(mediaTypes, "Enter new type ID: ");

                await client.EditMediaAsync(mediaToEdit);
                Console.WriteLine("Media successfully updated.");
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task ArchiveMedia(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var mediaTypes = await client.GetMediaTypesAsync();
            if (mediaTypes.Any())
            {
                IO.PrintMediaTypeList(mediaTypes);
                int typeID = IO.GetMediaTypeID(mediaTypes);
                var selectedMedias = await client.GetMediaByTypeAsync(typeID);
                Console.Clear();
                IO.PrintMediaList(selectedMedias);
                int mediaID = IO.GetMediaID(selectedMedias, "Enter the ID of the media to edit: ");
                var mediaToArchive = selectedMedias.Single(m => m.MediaID == mediaID);
                mediaToArchive.IsArchived = true;

                await client.ArchiveMediaAsync(mediaToArchive);
                Console.WriteLine("Media successfully archived.");
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task ViewArchive(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var archivedMedias = await client.GetArchivedMediaAsync();

            if (archivedMedias.Any())
            {
                IO.PrintMediaArchive(archivedMedias);
            }
            else
            {
                Console.WriteLine("No archived media items found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }

    public static async Task GetMostPopularMediaReport(IMediaAPIClient client)
    {
        Console.Clear();

        try
        {
            var topMedias = await client.GetMostPopularMediaAsync();

            if (topMedias.Any())
            {
                IO.PrintMediaReport(topMedias);
            }
            else
            {
                Console.WriteLine("No top media items found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed.\n{ex.Message}");
        }

        IO.AnyKey();
    }
}
