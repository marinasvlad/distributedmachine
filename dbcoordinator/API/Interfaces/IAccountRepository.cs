using API.Entities;

namespace API.Interfaces;

public interface IAccountRepository
{
    Task<User> GetUserByEmailAsync(string email);

    Task<User> RegisterUser(User user); 
}
