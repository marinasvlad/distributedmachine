using API.DTOS;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITokenService _tokenService;
    public AccountController(IAccountRepository accountRepo, ITokenService tokenService)
    {
        _accountRepo = accountRepo;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await _accountRepo.GetUserByEmailAsync(loginDTO.Email);

        if (user == null)
        {
            return BadRequest("Email gresit!");
        }

        if (loginDTO.Password != user.Parola)
        {
            return BadRequest("Parola gresita!");
        }

        return new UserDTO
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.Nume
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(LoginDTO loginDTO)
    {
        
        var u = await _accountRepo.GetUserByEmailAsync(loginDTO.Email);
        if(u != null)
        {
            return BadRequest("Email deja existent!");
        }

        User user = new User();
        user.Nume = loginDTO.Nume;
        user.Parola = loginDTO.Password;
        user.Email = loginDTO.Email;
        var usReg = await _accountRepo.RegisterUser(user);

        return new UserDTO
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.Nume
        };
    }    
}
