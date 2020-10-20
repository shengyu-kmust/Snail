namespace Snail.Core.Permission
{
    public interface IUserRole
    {
        string GetUserKey();
        string GetRoleKey();
        void SetUserKey(string userKey);
        void SetRoleKey(string roleKey);
    }
}
