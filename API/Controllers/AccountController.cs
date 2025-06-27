using API.Data;
using API.DTOs;
using API.Entities;
using API.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository, IMapper mapper, UserManager<AppUser> userManager)
    {
        _context = context;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO request)
    {
        if (await UserExists(request.UserName))
        {
            return BadRequest("UserName is taken");
        }

        var user = _mapper.Map<AppUser>(request);

        user.UserName = request.UserName.ToLower();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender,
        };
        return Ok(userDTO);
    }
    private async Task<bool> UserExists(string userName)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO request)
    {
        var user = await _userManager.Users
                    .Include(p=>p.Photos)
                    .FirstOrDefaultAsync(x=> x.NormalizedUserName == request.UserName.ToUpper());


        if (user is null || user.UserName is null)
            return Unauthorized("Invalid username");

        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            return Unauthorized("Password is wrong.");

        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
        return Ok(userDTO);
    }
}
