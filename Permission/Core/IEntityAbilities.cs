using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    /// <summary>
    /// 主键，如用户、角色、等的唯一键，可为account、name、id等
    /// </summary>
    public interface HasKey
    {
        string GetKey();
    }
    /// <summary>
    /// 是否有父键
    /// </summary>
    public interface HasParent
    {
        string GetParent();
    }

    public interface ResourceKey
    {
        string BuildResourceKey(object obj);
    }
}
