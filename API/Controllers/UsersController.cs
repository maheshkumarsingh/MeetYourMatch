using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseAPIController
{
    //private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService, IUnitOfWork unitOfWork)
    {
        //_userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
        _unitOfWork = unitOfWork;
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParams userParams)
    {
        userParams.CurrentUserName = User.GetUserName();
        var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
        Response.AddPaginationHeader(users);

        return Ok(users);
    }

    #region comments
    // Keep [Authorize] for this method to ensure it requires authentication.
    //[HttpGet("get-user/{id}")]
    #endregion
    //[Authorize(Roles ="Member")]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var memberDTO = await _unitOfWork.UserRepository.GetMemberAsync(username);
        if (memberDTO == null)
        {
            return NotFound("No user found.");
        }
        return Ok(memberDTO);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdate)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUserName());
        if (user is null)
            return BadRequest("Couldnot find user");
        if (memberUpdate.KnownAs is null)
            memberUpdate.KnownAs = user.KnownAs;
        _mapper.Map(memberUpdate, user);
        if (await _unitOfWork.Complete())
            return NoContent();
        return BadRequest("Failed to update a user");
    }
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUserName());
        if (user is null)
            return BadRequest("Cannot update user");
        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error is not null)
            return BadRequest($"Error: {result.Error}");
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        if (user.Photos.Count == 0)
            photo.IsMain = true;
        user.Photos.Add(photo);
        if (await _unitOfWork.Complete())
            return Ok(_mapper.Map<PhotoDTO>(photo));
        // Check why this code is giving error for.
        //return CreatedAtAction(nameof(GetUser), 
        //new
        //{
        //    username = user.UserName
        //},
        //_mapper.Map<PhotoDTO>(photo));
        return BadRequest("Problem adding photos");
    }
    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUserName());
        if (user is null)
            return BadRequest($"Unable to set main photo");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo is null || photo.IsMain)
            return BadRequest("cannot use this as main photo");
        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain is not null)
        {
            currentMain.IsMain = false;
        }
        photo.IsMain = true;
        if (await _unitOfWork.Complete())
            return NoContent();
        return BadRequest("Problem setting main photo");
    }
    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUserName());
        if (user is null)
            return BadRequest("User not found");
        var photo = user?.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo is null || photo.IsMain)
            return BadRequest("Photo cannot be deleted");
        if (photo.PublicId is null)
            return BadRequest("No publicId present");

        var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        if (result.Error is not null)
            return BadRequest($"Error: {result.Error.Message}");

        user.Photos.Remove(photo);
        if (await _unitOfWork.Complete())
            return NoContent();
        return BadRequest("Photo not deleted");
    }
}
