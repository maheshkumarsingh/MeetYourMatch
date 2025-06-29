using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.ServiceContracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public void AddGroup(Group group)
    {
        _dataContext.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        _dataContext.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _dataContext.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _dataContext.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await _dataContext.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections
            .Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await _dataContext.Messages.FindAsync(id);
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await _dataContext.Groups
                    .Include(x => x.Connections)
                    .FirstOrDefaultAsync(x => x.Name == groupName);
    }

    public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _dataContext.Messages.OrderByDescending(x => x.CreatedAt)
                    .AsQueryable();
        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted == false),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && x.SenderDeleted == false),
            _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null
                        && x.RecipientDeleted == false),
        };
        var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);
        return await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName)
    {
        var query = _dataContext.Messages
            //.Include(x => x.Sender).ThenInclude(x => x.Photos)
            //.Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(x => (x.RecipientUserName == currentUserName && x.RecipientDeleted == false && x.SenderUserName == recipientUserName) ||
                        (x.SenderUserName == currentUserName && x.SenderDeleted == false && x.RecipientUserName == recipientUserName))
            .OrderBy(x => x.CreatedAt)
            .AsQueryable();
            //.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
            //.ToListAsync();

        var unreadMessages = query
            .Where(x => x.RecipientUserName == currentUserName && x.DateRead == null)
            .ToList();

        if (unreadMessages.Count > 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.Now);
            //await _dataContext.SaveChangesAsync();
        }

        //return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        return await query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public void RemoveConnection(Connection connection)
    {
        _dataContext.Connections.Remove(connection);
    }

    //public async Task<bool> SaveAllAsync()
    //{
    //    return await _dataContext.SaveChangesAsync() > 0;
    //}
}
