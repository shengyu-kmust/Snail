using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snail.Permission
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <remarks>
    /// * 本类参考Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore和Microsoft.AspNetCore.Identity.UserStoreBase类
    /// * 整个identity用到了SigninManager和UserManager两个核心类类，而UserManger依赖于IUserStore。只要PermissionUserStore实现了IUserStore接口就行，并根据项目的需要，继承其它功能接口，如IUserPasswordStore
    /// * 可扩展的接口及说明
    ///   IUserLoginStore：提供外部系统登录功能
    ///   IUserClaimStore：提供存储User和Claim的对应关系的功能
    ///   IUserPasswordStore：提供用户密码加密的功能
    ///   IUserEmailStore：提供用户登录时需激活邮箱等功能
    ///   IUserPhoneNumberStore：提供用户必需激活手机号等功能
    ///   IUserLockoutStore：提供用户在输入多少密码后锁定账号的功能
    ///   IUserAuthenticationTokenStore：存储用户各样登录方式的token
    ///   IUserAuthenticatorKeyStore：存储用户用于校验时的key
    /// </remarks>
    public class PermissionUserStore<TUser, TContext,TKey> : IUserStore<TUser> ,IUserPasswordStore<TUser>
        where TUser : User<TKey>
        where TContext:DbContext
    {
        private TContext _db;
        private DbSet<TUser> _users;
        private bool _disposed;

        public PermissionUserStore(TContext db)
        {
            _db = db;
            _users = _db.Set<TUser>();
        }

        #region 实现IUserStore的接口
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _db.Add(user);
            await SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _db.Remove(user);
            await SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            var id = ConvertIdFromString(userId);
            return await _users.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            return await _users.FirstOrDefaultAsync(u => u.LoginName == normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LoginName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(ConvertIdToString(user.Id));
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.LoginName = normalizedName;
            user.UserName = user.UserName ?? normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.LoginName = userName;
            user.UserName = user.UserName ?? userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _db.Attach(user);
            user.UpdateTime = DateTime.Now;
            _db.Update(user);
            await SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        #endregion

        #region 实现IUserPasswordStore，实现此接口后，identity将会有设置用户的密码为hash格式的功能
        /// <summary>
        /// 设置用户密码，会设置用户user对象的密码字段（不会保存到数据库）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            //没有必要savechange，UserManager.CreateAsync(TUser user, string password)会调用数据库save
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.Pwd = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckCancelAndDispose(cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Pwd);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Pwd != null);
        }

        #endregion

        #region 内部帮助性方法
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private void CheckCancelAndDispose(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
        }

        private TKey ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(TKey);
            }
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
        private string ConvertIdToString(TKey id)
        {
            if (object.Equals(id, default(TKey)))
            {
                return null;
            }
            return id.ToString();
        }
        #endregion

        #region 其它和业务逻辑无关的方法
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;

            //是否要_db.Dispose()，请参考https://blog.jongallant.com/2012/10/do-i-have-to-call-dispose-on-dbcontext/的文章，文章的大概意思就是可以当不必要dispose。我的做法是不dispose
        }
        #endregion



    }
}
