using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;


    public TokenService(IConfiguration config) 
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
        _config = config;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Nume),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["Token:Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);   
    }
}
