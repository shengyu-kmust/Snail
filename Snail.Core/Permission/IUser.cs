namespace Snail.Core.Permission
{
    public interface IUser:IHasKeyAndName
    {
        string GetAccount();
        string GetPassword();
        void SetAccount(string account);
        void SetPassword(string pwd);
    }
}
