namespace Snail.Core.Permission
{
    /// <summary>
    /// 登录dto
    /// </summary>
    public class LoginDto
    {
        private bool _ignorePwd = false;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 是否登录到cookie
        /// </summary>
        public bool SignIn { get; set; }
        /// <summary>
        /// 租户id，无则为空
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// 免密登录，用于系统自动登录用户
        /// </summary>
        public bool IgnorePwd => _ignorePwd;
        public void SetIgnorePwd(bool ignorePwd)
        {
            _ignorePwd = ignorePwd;
        }
    }
}
