using API.Data;
using API.DTOs;
using API.Entities;
using API.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseAPIController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var members = await _userRepository.GetMembersAsync();
        return Ok(members);
    }

    #region comments
    // Keep [Authorize] for this method to ensure it requires authentication.
    //[HttpGet("get-user/{id}")]
    #endregion
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var memberDTO = await _userRepository.GetMemberAsync(username);
        if (memberDTO == null)
        {
            return NotFound("No user found.");
        }
        return Ok(memberDTO);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdate)
    {
        var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userName is null)
            return BadRequest("No userName is found in claims");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user is null)
            return BadRequest("Couldnot find user");
        _mapper.Map(memberUpdate, user);
        if (await _userRepository.SaveAllAsync())
            return NoContent();
        return BadRequest("Failed to update a user");
    }
}
