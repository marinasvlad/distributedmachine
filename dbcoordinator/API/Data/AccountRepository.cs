using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;
    public AccountRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> RegisterUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
    }
}
