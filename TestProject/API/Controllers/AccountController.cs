using System.Text.Json;
using API.DTOS;
using API.Errors;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IDBApiService _dbApiService;

    public AccountController(IDBApiService dbApiService)
    {
        _dbApiService = dbApiService;

    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var result = await _dbApiService.Login(loginDTO);

        if(result == "Email gresit!")
        {
            return BadRequest(new ApiResponse(400, "Email gresit!"));
        }
        else if(result == "Parola gresita!")
        {
            return BadRequest(new ApiResponse(400, "Parola gresita!"));
        }
        else
        {
            var user = JsonSerializer.Deserialize<UserDTO>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
            return user;
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(LoginDTO loginDTO)
    {
        var result = await _dbApiService.Register(loginDTO);

        if(result == "Email deja existent!")
        {
            return BadRequest(new ApiResponse(400, "Email deja existent!"));
        }
        else
        {
            var user = JsonSerializer.Deserialize<UserDTO>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
            return user;
        }
    }
}
