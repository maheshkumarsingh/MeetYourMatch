﻿using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR.PresenceHub;

[Authorize]
public class PresenceHub :Hub
{
    private readonly PresenceTracker _presenceTracker;

    public PresenceHub(PresenceTracker presenceTracker)
    {
        _presenceTracker = presenceTracker;
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.User is null)
            throw new HubException("Cannot get current user claim");

        var isOnline = await _presenceTracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
        if(isOnline)
            await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetUserName());
        
        var currentUsers = await _presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User is null)
            throw new HubException("Cannot get current user claim");
        var isOffline = await _presenceTracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);
        if(isOffline)
            await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUserName());
        
        //var currentUsers = await _presenceTracker.GetOnlineUsers();
        //await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        
        await base.OnDisconnectedAsync(exception);
    }
}
