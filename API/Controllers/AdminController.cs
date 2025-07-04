﻿using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController : BaseAPIController
{
    private readonly UserManager<AppUser> _userManager;

    public AdminController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUserWithRoles()
    {
        var users = await _userManager.Users.OrderBy(x => x.Id)
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.UserName,
                                                Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                                            })
                                            .ToListAsync();
        return Ok(users);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string userName, string roles)
    {
        if (string.IsNullOrEmpty(roles))
            return BadRequest("You must select atleast one role");
        var selectedRoles = roles.Split(',').ToArray();
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return BadRequest("user not found");
        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded)
            return BadRequest("Failed to add to roles");
        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded)
            return BadRequest("Failed to remove from roles");
        return Ok(await _userManager.GetRolesAsync(user));
    }
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photo-for-moderation")]
    public ActionResult GetPhotoForModeration()
    {
        return Ok("Only admin/moderator can see this");
    }
}
