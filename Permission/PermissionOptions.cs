namespace Snail.Permission
{
    public class PermissionOptions
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
    }
}
