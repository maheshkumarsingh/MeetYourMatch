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
public class MessagesController : BaseAPIController
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO messageDTO)
    {
        var userName = User.GetUserName();
        if (userName.ToLower() == messageDTO.RecipientUsername.ToLower())
            return BadRequest("You cannot message yourself");
        var sender = await _userRepository.GetUserByUsernameAsync(userName);
        var recipient = await _userRepository.GetUserByUsernameAsync(messageDTO.RecipientUsername);
        if (recipient is null || sender is null || sender.UserName is null || recipient.UserName is null)
            return BadRequest("Cannot send a message this time");
        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUserName = recipient.UserName,
            Content = messageDTO.Content
        };

        _messageRepository.AddMessage(message);
        if(await _messageRepository.SaveAllAsync())
            return Ok(_mapper.Map<MessageDTO>(message));
        return BadRequest("Failed to send message");
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.UserName = User.GetUserName();
        var messages = await _messageRepository.GetMessagesForUser(messageParams);
        Response.AddPaginationHeader(messages);
        return Ok(messages);
    }
    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
    {
        var currentUserName = User.GetUserName();
        var messages = await _messageRepository.GetMessageThread(currentUserName, username);
        return Ok(messages);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUserName();
        var message = await _messageRepository.GetMessage(id);
        if (message == null) return BadRequest("Cannot delete this message");

        if (message.SenderUserName != username && message.RecipientUserName != username)
            return Forbid("You are authenticated that doesnot means you will do anything.");

        if(message.SenderUserName == username)
            message.SenderDeleted = true;
        if(message.RecipientUserName == username)
            message.RecipientDeleted = true;
        if(message is { SenderDeleted:true, RecipientDeleted:true})
        {
            _messageRepository.DeleteMessage(message);
        }
        if (await _messageRepository.SaveAllAsync())
            return Ok();
        return BadRequest("Failed to delete the message");
    }
}
