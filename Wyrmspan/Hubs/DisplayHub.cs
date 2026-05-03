using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class DisplayHub : Hub
{
    private const int MaxPlayers = 4;

    private static ConcurrentDictionary<string, int> _clientToPlayer = new();

    private static ConcurrentDictionary<string, string> _connections = new();

    private static HashSet<int> _availableSlots = new(new[] { 0, 1, 2, 3 });

    private static object _lock = new();

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        string? clientId = httpContext?.Request.Query["clientId"];

        if (string.IsNullOrEmpty(clientId))
        {
            clientId = Guid.NewGuid().ToString();
        }

        int assignedPlayerId;

        lock (_lock)
        {
            if (_clientToPlayer.TryGetValue(clientId, out assignedPlayerId))
            {
                // Reuse existing player ID
            }
            else
            {
                if (_availableSlots.Count == 0)
                {
                    assignedPlayerId = -1; // indicate spectator / full game
                }
                else
                {
                    assignedPlayerId = _availableSlots.First();
                    _availableSlots.Remove(assignedPlayerId);

                    _clientToPlayer[clientId] = assignedPlayerId;
                }
            }

            _connections[Context.ConnectionId] = clientId;
        }

        await Clients.Caller.SendAsync("assignPlayer", assignedPlayerId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connections.TryRemove(Context.ConnectionId, out string? clientId))
        {
            lock (_lock)
            {
                if (_clientToPlayer.TryGetValue(clientId, out int playerId))
                {
                    _clientToPlayer.TryRemove(clientId, out _);
                    _availableSlots.Add(playerId);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}