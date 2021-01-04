using Newtonsoft.Json;
using Snail.Common;
using Snail.Core.Default.Service;
using Snail.Core.Dto;
using Snail.Core.Entity;
using Snail.Web.IServices;
using System.Collections.Generic;
using System.Linq;

namespace Snail.Web.Services
{
    public class ConfigService : BaseService<Config, string>, IConfigService
    {
        ///// <summary>
        ///// 配置父key为parentKey的所有子项生成List<KeyValueDto>时，KeyValueDto.Key/Value是取自Config的哪个字段
        ///// 如果在ConfigKeyValueFieldSetting里找不到parentKey，则默认key为Config.Id,value为Config.value
        ///// keyField的值只能是如下之一：Key,Id
        ///// valueField的值只能是如下之一：Value,Name
        ///// </summary>
        //public static List<(string parentKey, string keyField, string valueField)> ConfigKeyValueFieldSetting = new List<(string parentKey, string keyField, string valueField)>
        //{
        //    ("Unit","Key","Key")
        //};

        public ConfigService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        /// <summary>
        /// 获取父key为parentKey的所有子config记录的的key-value对值
        /// 约定：key为存储在数据库里的值，value是用于展示给用户看的值；
        /// 默认key为config.id,value为config.value，如有特殊修改，设置ConfigKeyValueFieldSetting
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        public List<KeyValueDto> GetConfigKeyValue(string parentKey)
        {
            var allConfig = entityCacheManager.Get<Config>();
            var parent = allConfig.AsQueryable().WhereIf(serviceContext.HasTenant(out string tenantId),a=>a.TenantId=="" || a.TenantId==null || a.TenantId==tenantId)
                .FirstOrDefault(a => a.Key == parentKey);
            string keyField=string.Empty, valueField=string.Empty;
            if (parent == null)
            {
                return new List<KeyValueDto>();
            }
            if (!string.IsNullOrEmpty(parent.ExtraInfo))
            {
                var extraInfoDic= JsonConvert.DeserializeObject<Dictionary<string, string>>(parent.ExtraInfo);
                extraInfoDic.TryGetValue(SnailWebConst.ConfigKeyField, out string keyFieldTmp);
                extraInfoDic.TryGetValue(SnailWebConst.ConfigKeyField, out string valueFieldTmp);
                keyField = keyFieldTmp;
                valueField = valueFieldTmp;
            }
            return allConfig.Where(a => a.ParentId == parent.Id).Select(a =>
            {
                return new KeyValueDto
                {
                    Key = (keyField == "Key") ? a.Key : a.Id,//默认key为config.id
                    Value = (valueField == "Name") ? a.Name : a.Value,//默认value为config.value
                    ExtraInfo = a.ExtraInfo
                };
            }).ToList();
        }
    }
}
