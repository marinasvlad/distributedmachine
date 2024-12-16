
using System.IO.Compression;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ImagesController : BaseApiController
{
    private readonly IImagesRepository _imagesRepo;
    public ImagesController(IImagesRepository imagesRepository)
    {
        _imagesRepo = imagesRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ImageA>>> Process()
    {
        var images = await _imagesRepo.GetAllImages();

        var imagesF = images.Select(image => new
        {
            id = image.Id,
            base64 = $"data:image/jpeg;base64,{Convert.ToBase64String(image.ImageData)}"
        }).ToList();
        return Ok(imagesF);
    }

    [HttpPost]
    public async Task<ActionResult<int>> ProcessImages(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No archive uploaded.");
        }

        try
        {
            var processedImagesCount = 0;

            using (var zipStream = file.OpenReadStream())
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    if (IsImageFile(entry.FullName))
                    {
                        using (var entryStream = entry.Open())
                        {
                            var processedImage = await ProcessImageToGrayscale(entryStream);

                            var image = new ImageA
                            {
                                ImageData = processedImage
                            };
                            await _imagesRepo.SaveImage(image);

                        }
                    }
                }
            }

            return Ok(processedImagesCount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    private bool IsImageFile(string fileName)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
        return validExtensions.Contains(Path.GetExtension(fileName).ToLower());
    }
    private async Task<byte[]> ProcessImageToGrayscale(Stream inputStream)
    {
        using var image = System.Drawing.Image.FromStream(inputStream);
        using var bitmap = new System.Drawing.Bitmap(image);

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                var originalColor = bitmap.GetPixel(x, y);
                var grayscale = (originalColor.R + originalColor.G + originalColor.B) / 3;
                var newColor = System.Drawing.Color.FromArgb(grayscale, grayscale, grayscale);
                bitmap.SetPixel(x, y, newColor);
            }
        }

        using var memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        return memoryStream.ToArray();
    }

}
