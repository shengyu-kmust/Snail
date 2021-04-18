﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    /// <summary>
    /// 只要有身份验证，不需要授予
    /// </summary>
    public class OnlyAuthenticationRequirement: IAuthorizationRequirement
    {
    }
}
