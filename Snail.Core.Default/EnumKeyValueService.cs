using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Snail.Core.Attributes;
using Snail.Core.Dto;
using Snail.Core.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Snail.Core.Default
{
    /// <summary>
    /// 注册成单例
    /// </summary>
    public class EnumKeyValueService : IEnumKeyValueService
    {
        // 如果用ImmutableList，会在每次使用时创建新的对象，如何在不用ImmutableList时让外部不能修改list的值 // todo 不用ImmutableList实现readonly list
        private ConcurrentDictionary<string, List<KeyValueDto>> _keyValuePairs;
        private bool _hasLoad = false;
        private static object _locker = new object();
        private IOptionsMonitor<EnumKeyValueServiceOptions> _options;
        public EnumKeyValueService(IOptionsMonitor<EnumKeyValueServiceOptions> options)
        {
            _keyValuePairs = new ConcurrentDictionary<string, List<KeyValueDto>>(StringComparer.OrdinalIgnoreCase);
            _options = options;
        }
        public List<KeyValueDto> GetKeyValues(string code)
        {
            EnsureLoaded();
            return _keyValuePairs.TryGetValue(code, out List<KeyValueDto> result) ? result : new List<KeyValueDto>();
        }

        private void EnsureLoaded()
        {
            if (!_hasLoad)
            {
                lock (_locker)
                {
                    if (!_hasLoad)
                    {
                        if (_options?.CurrentValue?.InitKeyValues != null && _options.CurrentValue.InitKeyValues.Count() > 0)
                        {
                            _options.CurrentValue.InitKeyValues.ToList().ForEach(item =>
                            {
                                _keyValuePairs.TryAdd(item.Key, item.Value);
                            });
                        }
                        if (_options?.CurrentValue?.Assemblies != null && _options?.CurrentValue?.Assemblies.Count() > 0)
                        {
                            var enums = _options.CurrentValue.Assemblies.SelectMany(a => a.GetTypes().Where(i => i.IsEnum && i.IsDefined(typeof(EnumKeyValueAttribute))));
                            enums.ToList().ForEach(a =>
                            {
                                _keyValuePairs.TryAdd(a.Name, a.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Select(i => new KeyValueDto { Key = i.Name, Value = i.GetCustomAttribute<DescriptionAttribute>()?.Description ?? i.Name }).ToList());
                            });
                        }
                        _hasLoad = true;
                    }
                }
            }
        }


    }

    public class EnumKeyValueServiceOptions
    {
        public List<Assembly> Assemblies { get; set; }
        public Dictionary<string, List<KeyValueDto>> InitKeyValues { get; set; }
    }

    

    public static class EnumKeyValueServiceExtenssion
    {
        public static void AddEnumKeyValueService(this IServiceCollection services, Action<EnumKeyValueServiceOptions> action)
        {
            services.AddSingleton<IEnumKeyValueService, EnumKeyValueService>();
            services.Configure(action);
        }
    }

}
