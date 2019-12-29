using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    public interface IUser:IHasKeyAndName
    {
        string GetAccount();
        string GetPassword();
    }
}
