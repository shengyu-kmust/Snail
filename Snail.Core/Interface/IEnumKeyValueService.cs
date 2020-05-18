using Snail.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    public interface IEnumKeyValueService
    {
        List<KeyValueDto> GetKeyValues(string enumName);
    }
}
