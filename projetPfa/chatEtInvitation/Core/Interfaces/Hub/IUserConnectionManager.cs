namespace chatEtInvitation.Core.Interfaces.Hub
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(int userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        string GetUserGroupName(int userId);
    }
}
