using System.Collections.Concurrent;
using chatEtInvitation.Core.Interfaces.Hub;

public class UserConnectionManager : IUserConnectionManager
{
    // Clé : userId, Valeur : liste des connexions de l'utilisateur
    private static readonly ConcurrentDictionary<int, List<string>> _userConnections = new();

    public void KeepUserConnection(int userId, string connectionId)
    {
        _userConnections.AddOrUpdate(
            userId,
            new List<string> { connectionId },
            (key, existingList) =>
            {
                lock (existingList)
                {
                    if (!existingList.Contains(connectionId))
                        existingList.Add(connectionId);
                }
                return existingList;
            }
        );
    }

    public void RemoveUserConnection(string connectionId)
    {
        foreach (var kvp in _userConnections)
        {
            var userId = kvp.Key;
            var connectionList = kvp.Value;

            lock (connectionList)
            {
                if (connectionList.Contains(connectionId))
                {
                    connectionList.Remove(connectionId);
                    if (connectionList.Count == 0)
                    {
                        _userConnections.TryRemove(userId, out _);
                    }
                    break;
                }
            }
        }
    }

    public string GetUserGroupName(int userId)
    {
        return $"user_{userId}";
    }
}
