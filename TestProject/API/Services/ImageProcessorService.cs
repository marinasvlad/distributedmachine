
using System.IO.Compression;

namespace API.Services;

public class ImageProcessorService : IImageProcessorService
{
    private readonly HttpClient _httpClient;
    public ImageProcessorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> Process()
    {
        var response = await _httpClient.GetAsync("api/images");
        var content = await response.Content.ReadAsStringAsync();
        return int.Parse(content);
    }

    public async Task ProcessImages(List<byte[]> images)
    {
    using (var memoryStream = new MemoryStream())
    {
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < images.Count; i++)
            {
                var entry = zipArchive.CreateEntry($"image{i}.jpg", CompressionLevel.Fastest);
                using (var entryStream = entry.Open())
                {
                    await entryStream.WriteAsync(images[i], 0, images[i].Length);
                }
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        using (var formData = new MultipartFormDataContent())
        {
            var zipContent = new ByteArrayContent(memoryStream.ToArray());
            zipContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/zip");
            formData.Add(zipContent, "file", "images.zip");

            var response = await _httpClient.PostAsync("api/images", formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error uploading archive: {response.ReasonPhrase}");
            }
        }
    }
    }
}
