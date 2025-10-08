using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.Hub;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IUserConnectionManager _userConnectionManager;

    public ChatHub(IUserConnectionManager userConnectionManager)
    {
        _userConnectionManager = userConnectionManager;
    }

    public async Task JoinUserGroup(int userId)
    {
        _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        Console.WriteLine($"[Hub] User {userId} added to user group");
    }

    public async Task JoinTeamGroup(int teamId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"team_{teamId}");
        Console.WriteLine($"[Hub] User added to team group {teamId}");
    }

    public async Task SendAmisMessage(AmisMessageDTO message)
    {
        var receiverGroup = $"user_{message.RecepteurId}";
        var senderGroup = $"user_{message.Emetteur}";

        await Clients.Group(receiverGroup).SendAsync("ReceiveAmisMessage", message);
        await Clients.Group(senderGroup).SendAsync("ReceiveAmisMessage", message);
        Console.WriteLine($"[Hub] AmisMessage sent to {receiverGroup} and {senderGroup}");
    }

    public async Task SendTeamMessage(TeamMessageDTO message)
    {
        await Clients.Group($"team_{message.teamId}").SendAsync("ReceiveTeamMessage", message);
        Console.WriteLine($"[Hub] TeamMessage sent to team_{message.teamId}");
    }

    public async Task MarkMessagesAsSeen(int[] messageIds, int seenByUserId)
    {
        await Clients.All.SendAsync("MessagesSeen", messageIds, seenByUserId);
        Console.WriteLine($"[Hub] Messages marked as seen by {seenByUserId}");
    }

    public async Task NotifyTyping(bool isTyping, bool isTeamChat, int chatId, int userId, int receiverId ,string? username)
    {
        Console.WriteLine($"[Hub][Typing] Received - User:{userId}, Typing:{isTyping}, TeamChat:{isTeamChat}, ChatId:{chatId}, Receiver:{receiverId}");

        if (isTeamChat)
        {
            // Envoyer à tous les membres de l'équipe sauf l'émetteur
            await Clients.GroupExcept($"team_{chatId}", Context.ConnectionId)
                .SendAsync("UserTyping", userId, isTyping, isTeamChat, chatId , username);
            Console.WriteLine($"[Hub][Typing] Sent to team_{chatId} except {Context.ConnectionId}");
        }
        else
        {
            // Envoyer seulement au receveur
            await Clients.Group($"user_{receiverId}")
                .SendAsync("UserTyping", userId, isTyping, isTeamChat, chatId);
            Console.WriteLine($"[Hub][Typing] Sent to user_{receiverId}");
        }
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext?.Request.Query["userId"];

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            Console.WriteLine($"[Hub] OnConnected - User {userId} added to user group");

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} added to chat group");

        }

        await base.OnConnectedAsync();
    }

   
}












