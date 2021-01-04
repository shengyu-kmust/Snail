﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Dto
{

    public class DefaultBaseDto : BaseAuditSoftDeleteDto<string>
    {
    }

    public class DefaultBaseDtoWithTenant : BaseAuditSoftDeleteTenantDto<string>
    {
    }
    /// <summary>
    /// 用于只传id对象的dto
    /// </summary>
    public class IdsDto
    {
        public List<string> Ids { get; set; }
    }


    /// <summary>
    /// key-value值对dto
    /// </summary>
    public class KeyValueDto
    {
        /// <summary>
        /// 一般将下拉的隐藏值做为key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 一般将下拉能看到的值做为value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string ExtraInfo { get; set; }
    }
}
