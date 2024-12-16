using System.IO.Compression;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ImagesController : BaseApiController
{
    private readonly IImageProcessorService _imageProcessorService;

    public ImagesController(IImageProcessorService imageProcessorService)
    {
        _imageProcessorService = imageProcessorService;

    }

    [HttpGet]
    public async Task<ActionResult> Process()
    {

        List<Task> listOfTask = new List<Task>();

        var result = await _imageProcessorService.Process();
        var task1 = Task.Run(() => _imageProcessorService.Process());
        listOfTask.Add(task1);
        var task2 = Task.Run(() => _imageProcessorService.Process());
        listOfTask.Add(task2);
        var task3 = Task.Run(() => _imageProcessorService.Process());
        listOfTask.Add(task3);

        await Task.WhenAll(listOfTask);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var imagesList = new List<byte[]>();
        const int chunkSize = 10;
        var imageChunks = new List<List<byte[]>>();

        try
        {
            using (var zipStream = file.OpenReadStream())
            {
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (IsImageFile(entry.FullName))
                        {
                            using (var imageStream = entry.Open())
                            {
                                byte[] imageBytes = ConvertStreamToByteArray(imageStream);
                                imagesList.Add(imageBytes);
                            }
                        }
                    }
                }
            }
            imageChunks = CreateListOfListOfBytes(imagesList, 10);
            var listOfTask = new List<Task>();

            foreach (var list in imageChunks)
            {

                if (listOfTask.Count >= 5)
                {
                    int indexTask = Task.WaitAny(listOfTask.ToArray());
                    listOfTask.Remove(listOfTask[indexTask]);
                }
                var task = Task.Run(() => _imageProcessorService.ProcessImages(list));
                listOfTask.Add(task);
            }
            Task.WaitAll(listOfTask.ToArray());
            return Ok(new
            {
                Message = "File uploaded and images processed successfully.",
                TotalImages = imagesList.Count,
                ChunksCreated = imageChunks.Count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    private List<List<byte[]>> CreateListOfListOfBytes(List<byte[]> imagini, int batchSize)
    {
        List<List<byte[]>> listOfListOfBytes = new List<List<byte[]>>();
        for (int i = 0; i < imagini.Count / batchSize; i++)
        {
            List<byte[]> listOfbytes = new List<byte[]>();
            for (int j = i * batchSize; j < i * batchSize + batchSize; j++)
            {
                listOfbytes.Add(imagini[j]);
            }
            listOfListOfBytes.Add(listOfbytes);
        }
        if (imagini.Count % batchSize != 0)
        {
            List<byte[]> finalList = new List<byte[]>();
            for (int i = listOfListOfBytes.Count * batchSize; i < imagini.Count; i++)
            {
                finalList.Add(imagini[i]);
            }
            listOfListOfBytes.Add(finalList);
        }
        return listOfListOfBytes;
    }

    private bool IsImageFile(string fileName)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }

    private byte[] ConvertStreamToByteArray(Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
