using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ImagesRepository : IImagesRepository
{
    private readonly DataContext _context;
    public ImagesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ImageA>> GetAllImages()
    {
        return await _context.Images.ToListAsync();
    }

    public async Task SaveImage(ImageA image)
    {
        await _context.AddAsync(image);
        await _context.SaveChangesAsync();
    }
}
