using API.DTOs;
using API.Entities;
using API.Extensions;
using API.ServiceContracts;
using API.SignalR.PresenceHub;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR.MessageHub;

public class MessageHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHubContext<API.SignalR.PresenceHub.PresenceHub> _presenceHub;

    public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, IHubContext<API.SignalR.PresenceHub.PresenceHub> presenceHub)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _presenceHub = presenceHub;
    }
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext.Request.Query["user"];
        if (Context.User is null || string.IsNullOrEmpty(otherUser)) throw new HubException("cannot join group");
        var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
        
        var messages = await _messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!);
        
        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessage(CreateMessageDTO messageDTO)
    {
        var userName = Context.User?.GetUserName() ?? throw new HubException("could not get user");

        if (userName == messageDTO.RecipientUsername.ToLower())
            throw new HubException("you cannot message yourself");

        var sender = await _userRepository.GetUserByUsernameAsync(userName);
        var recipient = await _userRepository.GetUserByUsernameAsync(messageDTO.RecipientUsername);
        if (recipient is null || sender is null || sender.UserName is null || recipient.UserName is null)
            throw new HubException("Cannot send a message this time");
        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUserName = recipient.UserName,
            Content = messageDTO.Content
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);
        var group = await _messageRepository.GetMessageGroup(groupName);

        if(group is not null && group.Connections.Any(x=>x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if(connections != null && connections?.Count != null)
            {
                await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
            }
        }
            _messageRepository.AddMessage(message);

        if (await _messageRepository.SaveAllAsync())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
        }

    }
    private async Task<Group> AddToGroup(string groupName)
    {
        var username = Context.User?.GetUserName() ?? throw new HubException("Cannot get username");
        var group = await _messageRepository.GetMessageGroup(groupName);
        var connection = new Connection
        {
            ConnectionId = Context.ConnectionId,
            Username = username
        };
        if (group is null)
        {
            group = new Group { Name = groupName };
            _messageRepository.AddGroup(group);
        }
        group.Connections.Add(connection);
        if(await _messageRepository.SaveAllAsync())
            return group;
        throw new HubException("failed to join group");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
        var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

        if (connection is not null && group is not null)
        {
            _messageRepository.RemoveConnection(connection);
            if(await _messageRepository.SaveAllAsync())
                return group;
        }
        throw new HubException("failed to remove from group");
    }
    private string GetGroupName(string caller, string? other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

}
