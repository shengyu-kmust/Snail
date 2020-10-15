using Snail.Core.Dto;
using Snail.Web.Entities;
using System.Collections.Generic;

namespace ApplicationCore.IServices
{
    public interface IConfigService : IBaseService<Config>
    {
        List<KeyValueDto> GetConfigKeyValue(string parentKey);
    }
}
