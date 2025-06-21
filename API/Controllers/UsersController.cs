using API.Data;
using API.DTOs;
using API.Entities;
using API.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseAPIController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var members = await _userRepository.GetMembersAsync();
        return Ok(members);
    }

    # region comments
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
}
