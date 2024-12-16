using API.DTOS;

namespace API.Interfaces;

public interface IDBApiService
{
    Task<string> Login(LoginDTO loginDTO);

    Task<string> Register(LoginDTO loginDTO);


}
