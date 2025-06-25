﻿using API.Data;
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

    public void AddMesage(Message message)
    {
        _dataContext.Messages.Add(message);
    }

    public void DeleteMesage(Message message)
    {
        _dataContext.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await _dataContext.Messages.FindAsync(id);
    }

    public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _dataContext.Messages.OrderByDescending(x => x.CreatedAt)
                    .AsQueryable();
        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted==false),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && x.SenderDeleted == false),
            _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null
                        && x.RecipientDeleted==false ),
        };
        var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);
        return await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName)
    {
        var messages = await _dataContext.Messages
            .Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(x => (x.RecipientUserName == currentUserName && x.RecipientDeleted==false && x.SenderUserName == recipientUserName) ||
                        (x.SenderUserName == currentUserName && x.SenderDeleted == false && x.RecipientUserName == recipientUserName))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        var unreadMessages = messages
            .Where(x => x.RecipientUserName == currentUserName && x.DateRead == null)
            .ToList();

        if (unreadMessages.Count > 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.Now);
            await _dataContext.SaveChangesAsync();
        }

        return _mapper.Map<IEnumerable<MessageDTO>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}
