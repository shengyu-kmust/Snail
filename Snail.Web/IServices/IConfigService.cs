using Snail.Core.Dto;
using Snail.Web.Entities;
using System.Collections.Generic;

namespace Snail.Web.IServices
{
    public interface IConfigService : IBaseService<Config,string>
    {
        List<KeyValueDto> GetConfigKeyValue(string parentKey);
    }
}
