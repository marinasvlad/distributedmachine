using API.Entities;

namespace API.Interfaces;

public interface IImagesRepository
{
    Task SaveImage(ImageA image);

    Task<IReadOnlyList<ImageA>> GetAllImages();
}
