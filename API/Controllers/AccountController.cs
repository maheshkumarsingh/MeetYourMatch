using API.Data;
using API.DTOs;
using API.Entities;
using API.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AccountController : BaseAPIController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO request)
    {
        if (await UserExists(request.UserName))
        {
            return BadRequest("UserName is taken");
        }
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = request.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };

        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();
        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(userDTO);
    }
    private async Task<bool> UserExists(string userName)
    {
        return await _context.AppUsers.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO request)
    {
        var user = _context.AppUsers
                           .AsEnumerable()
                           .FirstOrDefault(x => x.UserName.Equals(request.UserName,
                            StringComparison.CurrentCultureIgnoreCase));
        if (user is null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512()
        {
            Key = user.PasswordSalt
        };
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(userDTO);
    }
}
