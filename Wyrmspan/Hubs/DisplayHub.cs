using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class DisplayHub : Hub
{
    private static int _nextPlayerId = 0;
    private static ConcurrentDictionary<string, int> _players = new();

    public override async Task OnConnectedAsync()
    {
        int assignedId = _nextPlayerId;

        _players[Context.ConnectionId] = assignedId;
        _nextPlayerId++;

        await Clients.Caller.SendAsync("assignPlayer", assignedId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _players.TryRemove(Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task PushUpdate(object data)
    {
        await Clients.All.SendAsync("updateDisplay", data);
    }
    
}