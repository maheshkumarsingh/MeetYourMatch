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
    private readonly IUserRepository _userRepository;

    public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository)
    {
        _context = context;
        _tokenService = tokenService;
        _userRepository = userRepository;
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
            PasswordSalt = hmac.Key,
            KnownAs = "Mahesh",
            Gender = "male",
            City = "Pune",
            Country = "India",
            Photos = new List<Photo>
            {
                new Photo
                {
                    Url = "https://randomuser.me/api/portraits/men/20.jpg",
                    IsMain = true,
                }
            }
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
        var user = await _userRepository.GetUserByUsernameAsync(request.UserName);
        if (user is null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
            return Unauthorized("Invalid password");

        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(userDTO);
    }
}
