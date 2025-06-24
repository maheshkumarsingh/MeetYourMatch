using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController:BaseAPIController
{
    private readonly ILikesRepository _likesRepository;

    public LikesController(ILikesRepository likesRepository)
    {
        _likesRepository = likesRepository;
    }
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();
        if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");
        var existingLike = await _likesRepository.GetUserLike(sourceUserId, targetUserId);
        if(existingLike is null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };
            _likesRepository.AddLike(like);
        }
        else
        {
            _likesRepository.DeleteLike(existingLike);
        }
        if (await _likesRepository.SaveChanges())
            return Ok();
        return BadRequest("Failed to update like");
    }
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await _likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _likesRepository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(users);
        return Ok(users);
    }
}
