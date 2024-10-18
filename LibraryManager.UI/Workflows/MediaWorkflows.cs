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
                IO.PrintMediaList(selectedMedia);
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
            Console.WriteLine(ex.Message);
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
                if (!selectedMedias.Any(m => m.IsArchived == false))
                {
                    Console.Clear();
                    Console.WriteLine("No available media of this type found.");
                }
                else
                {
                    var editableMedias = selectedMedias.FindAll(m => m.IsArchived == false);
                    IO.PrintAvailableMedia(editableMedias);
                    int mediaID = IO.GetMediaID(editableMedias, "\nEnter the ID of the media to edit: ");
                    var mediaToEdit = editableMedias.Single(m => m.MediaID == mediaID);
                    mediaToEdit.Title = IO.GetEditedString("Enter new title: ", mediaToEdit.Title);
                    mediaToEdit.MediaTypeID = IO.GetMediaTypeID(mediaTypes, "Enter new type ID: ");

                    await client.EditMediaAsync(mediaToEdit);
                    Console.WriteLine("Media successfully updated.");
                }
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
                if (!selectedMedias.Any(m => m.IsArchived == false))
                {
                    Console.Clear();
                    Console.WriteLine("No available media of this type found.");
                }
                else
                {
                    var availableMedias = selectedMedias.FindAll(m => m.IsArchived == false);
                    IO.PrintAvailableMedia(availableMedias);
                    int mediaID = IO.GetMediaID(availableMedias, "\nEnter the ID of the media to archive: ");
                    var mediaToArchive = availableMedias.Single(m => m.MediaID == mediaID);
                    mediaToArchive.IsArchived = true;

                    await client.ArchiveMediaAsync(mediaToArchive);
                    Console.WriteLine("Media successfully archived.");
                }
            }
            else
            {
                Console.WriteLine("No media type found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
            Console.WriteLine(ex.Message);
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
            Console.WriteLine(ex.Message);
        }

        IO.AnyKey();
    }
}
