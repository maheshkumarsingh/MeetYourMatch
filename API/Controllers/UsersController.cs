using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseAPIController
{
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
        _context = context;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.AppUsers.ToListAsync();
        if (users == null || !users.Any())
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [HttpGet("get-user/{id}")]
    //[Route("get-a-user/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.AppUsers.FindAsync(id);
        if (user == null)
        {
            return NotFound("No user found.");
        }
        return Ok(user);
    }
}
