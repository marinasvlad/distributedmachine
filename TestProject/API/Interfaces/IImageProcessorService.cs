namespace API.Services;

public interface IImageProcessorService
{
        Task<int> Process();

        Task ProcessImages(List<byte[]> images);
}
