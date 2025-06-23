using API.Entities;
using API.ServiceContracts;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(AppUser user)
    {
        var tokenKey = _configuration["TokenKey"];
        if(string.IsNullOrEmpty(tokenKey))
        {
            throw new ArgumentNullException("TokenKay is not configured in appsettings.");
        }
        else if(tokenKey.Length<64)
        {
            throw new ArgumentException("TokenKay must be at least 64 characters long.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
