using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Snail.Common
{
    /// <summary>
    /// 对象属性映射赋值类
    /// </summary>
    /// <remarks>
    /// 默认映射逻辑:字段名相同（忽略大小写），且类型相同（支持类型的可空与非可空映射）
    /// 如默认逻辑不符合需求，可通过RegisterAction注册映射逻辑
    /// 可用于替代automapper，实现大多数情况下的属性赋值操作，简单，免配置，性能高
    /// </remarks>
    public static class EasyMap
    {
        private static ConcurrentDictionary<string, Delegate> _mapActions = new ConcurrentDictionary<string, Delegate>();
        private static object locker = new object();

        /// <summary>
        /// 注册属性映射，如果默认的映射规则不满足需求，可自定义映射逻辑
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="mapAction">属性映射action</param>
        public static void RegisterAction<TSource, TTarget>(Action<TSource, TTarget, List<string>> mapAction)
            where TSource : class, new()
            where TTarget : class, new()
        {
            var mapKey = GetMapKey<TSource, TTarget>();
            _mapActions.AddOrUpdate(mapKey, mapAction, (k, old) => mapAction);
        }

        #region EasyMap的核心方法
        /// <summary>
        /// 未知类型的对象属性映射
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="source">源类型值</param>
        /// <param name="target">目标类型值</param>
        /// <param name="includeFields">包含字段</param>
        /// <remarks>
        /// 当类型是已知时，推荐用 Map<TSource,TTarget>(TSource source, TTarget target, List<string> includeFields)，速度会更快
        /// </remarks>
        public static void Map(Type sourceType, Type targetType, object source, object target, List<string> includeFields)
        {
            var mapKey = GetMapKey(sourceType, targetType);
            EnsureMap(sourceType, targetType, mapKey);
            var mapAction = _mapActions[mapKey];
            mapAction.DynamicInvoke(source, target, includeFields);// 注意，DynamicInvloke用的是反射，会比Invoke慢很多
        }

        /// <summary>
        /// 已知类型的对象属性映射（推荐）
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型值</param>
        /// <param name="target">目标类型值</param>
        /// <param name="includeFields">包含字段</param>
        public static void Map<TSource, TTarget>(TSource source, TTarget target, List<string> includeFields)
        where TSource : class, new()
        where TTarget : class, new()
        {
            var mapKey = GetMapKey<TSource, TTarget>();
            EnsureMap<TSource, TTarget>(mapKey);
            var mapAction = (Action<TSource, TTarget, List<string>>)_mapActions[mapKey];
            mapAction.Invoke(source, target, includeFields);
        }


        /// <summary>
        /// 从源类型创建一个新的目标类型（推荐）
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <returns></returns>
        public static TTarget MapToNew<TSource, TTarget>(TSource source, List<string> includeFields)
           where TSource : class, new()
           where TTarget : class, new()
        {
            var target = new TTarget();
            Map(source, target, includeFields);
            return target;
        }

        /// <summary>
        /// 从源类型创建一个新的目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <returns></returns>
        public static object MapToNew(Type sourceType, Type targetType, object source, List<string> includeFields)
        {
            var target = Activator.CreateInstance(targetType);
            Map(sourceType, targetType, source, target, includeFields);
            return target;
        }


        /// <summary>
        /// 将源类型列表转成目标类型列表（推荐）
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="sources">源类型列表变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <returns>生成的目标类型列表</returns>
        public static List<TTarget> MapToList<TSource, TTarget>(List<TSource> sources, List<string> includeFields)
            where TSource : class, new()
            where TTarget : class, new()
        {
            var result = new List<TTarget>();
            var mapKey = GetMapKey<TSource, TTarget>();
            EnsureMap<TSource, TTarget>(mapKey);
            var mapAction = (Action<TSource, TTarget, List<string>>)_mapActions[mapKey];
            foreach (var source in sources)
            {
                var temp = new TTarget();
                mapAction(source, temp, includeFields);
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// 获取根据源类型修改目标类型的属性值的委托
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <returns></returns>
        public static Action<TSource, TTarget, List<string>> GetMapAction<TSource, TTarget>()
        where TSource : class
        where TTarget : class
        {
            var mapKey = GetMapKey<TSource, TTarget>();
            EnsureMap<TSource, TTarget>(mapKey);
            var mapAction = (Action<TSource, TTarget, List<string>>)_mapActions[mapKey];
            return mapAction;
        }

        /// <summary>
        /// 获取映射委托的key
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <returns></returns>
        public static string GetMapKey<TSource, TTarget>()
        {
            return GetMapKey(typeof(TSource), typeof(TTarget));
        }

        /// <summary>
        /// 获取映射委托的key
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static string GetMapKey(Type sourceType, Type targetType)
        {
            return $"{sourceType.Name}_{targetType.Name}";
        }

        /// <summary>
        /// 默认的源类型和目标类型的字段映射委托（已知类型）
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <returns>映射委托</returns>
        private static Action<TSource, TTarget, List<string>> GenerateMapAction<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            var sourcePa = Expression.Parameter(typeof(TSource), "source");
            var targetPa = Expression.Parameter(typeof(TTarget), "target");
            var fieldsPa = Expression.Parameter(typeof(List<string>), "fields");
            var ex = Expression.Lambda<Action<TSource, TTarget, List<string>>>(
                Expression.Block(GetSourceTargetAssignExpressions<TSource, TTarget>(sourcePa, targetPa, fieldsPa)),
                sourcePa, targetPa, fieldsPa);
            var func = ex.Compile();
            return func;
        }

        /// <summary>
        /// 默认的源类型和目标类型的字段映射委托(未知类型)
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>映射委托</returns>
        private static Delegate GenerateMapAction(Type sourceType, Type targetType)
        {
            var sourcePa = Expression.Parameter(sourceType, "source");
            var targetPa = Expression.Parameter(targetType, "target");
            var fieldsPa = Expression.Parameter(typeof(List<string>), "fields");
            var ex = Expression.Lambda(
                Expression.Block(GetSourceTargetAssignExpressions(sourceType, targetType, sourcePa, targetPa, fieldsPa)),
                sourcePa, targetPa, fieldsPa);
            var func = ex.Compile();
            return func;
        }


        /// <summary>
        /// 获取默认的源类型到目标类型的赋值表达式
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="sourcePa">源类型变量表达示</param>
        /// <param name="targetPa">目标类型变量表达示</param>
        /// <param name="fieldsPa">包含字段</param>
        /// <returns>映射表达式Body</returns>
        /// <remarks>
        /// 生成如target.Field1=source.Field1;target.Field2=source.Field2的表达式
        /// </remarks>
        private static Expression[] GetSourceTargetAssignExpressions<TSource, TTarget>(ParameterExpression sourcePa, ParameterExpression targetPa, ParameterExpression fieldsPa)
        {
            return GetSourceTargetAssignExpressions(typeof(TSource), typeof(TTarget), sourcePa, targetPa, fieldsPa);
        }

        /// <summary>
        /// 获取默认的源类型到目标类型的赋值表达式
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="sourcePa">源类型变量表达示</param>
        /// <param name="targetPa">目标类型变量表达示</param>
        /// <param name="fieldsPa">包含字段</param>
        /// <returns>映射表达式Body</returns>
        private static Expression[] GetSourceTargetAssignExpressions(Type sourceType, Type targetType, ParameterExpression sourcePa, ParameterExpression targetPa, ParameterExpression fieldsPa)
        {
            var expressions = new List<Expression>();
            var sourceProperties = sourceType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).ToList();
            var targetProperties = targetType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).ToList();
            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.FirstOrDefault(a => a.Name.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase));

                if (targetProperty != null && targetProperty.CanWrite && sourceProperty != null)
                {
                    // 可空和非可空的映射
                    var targetPropertyIsNullable = targetProperty.PropertyType.IsGenericType && targetProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var sourcePropertyIsNullable = sourceProperty.PropertyType.IsGenericType && sourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var targetBasicType = targetPropertyIsNullable
                            ? targetProperty.PropertyType.GetGenericArguments()[0]
                            : targetProperty.PropertyType;
                    var sourceBasicType = sourcePropertyIsNullable
                        ? sourceProperty.PropertyType.GetGenericArguments()[0]
                        : sourceProperty.PropertyType;

                    // 只有当字段名和基础类型都相等时才匹配映射关系
                    if (targetBasicType == sourceBasicType)
                    {
                        var sourcePropertyExpression = Expression.PropertyOrField(sourcePa, sourceProperty.Name);
                        var targetPropertyExpression = Expression.PropertyOrField(targetPa, targetProperty.Name);
                        var condition1 = Expression.Equal(fieldsPa, Expression.Constant(null)); // 包段字段是否为null的判断
                        var condition2 = Expression.Call(fieldsPa, typeof(List<string>).GetMethod("Contains"), Expression.Constant(targetProperty.Name));
                        BinaryExpression assignExpression; // 目标字段是否在包含字段里的判断
                        Expression leftExpression;
                        if (sourcePropertyIsNullable && !targetPropertyIsNullable)
                        {
                            // 可空到非可空的赋值
                            leftExpression = Expression.Condition(
                                Expression.PropertyOrField(sourcePropertyExpression, "HasValue"),
                                Expression.PropertyOrField(sourcePropertyExpression, "Value"),
                                Expression.Default(targetBasicType));// 编译成source.field.HasValue?source.field.Value:default
                        }
                        else if (!sourcePropertyIsNullable && targetPropertyIsNullable)
                        {
                            // 非可空到可空的赋值
                            leftExpression = Expression.Convert(sourcePropertyExpression, targetPropertyExpression.Type);
                        }
                        else
                        {
                            // 两个类型完成相同
                            leftExpression = sourcePropertyExpression;
                        }
                        assignExpression = Expression.Assign(targetPropertyExpression, leftExpression);
                        var thisFieldAssignExpression = Expression.IfThen(Expression.OrElse(condition1, condition2), assignExpression);//用orElse而不是or来提高性能
                        expressions.Add(thisFieldAssignExpression);
                    }
                }
            }
            return expressions.ToArray();
        }
        #endregion

        #region 重载方法
        #region map重载
        public static void Map<TSource, TTarget>(TSource source, TTarget target)
     where TSource : class, new()
     where TTarget : class, new()
        {
            Map(source, target, null);
        }

        /// <summary>
        /// 从源类型设置目标类型的属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="target">目标类型变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <remarks>
        /// 1、请不要循环调用，请改用GenerateMapAction或是MapToList
        /// </remarks>
        public static void Map<TSource, TTarget>(TSource source, TTarget target, List<string> includeFields, List<string> exceptFields)
            where TSource : class, new()
            where TTarget : class, new()
        {
            Map(source, target, GetIncludeFields<TTarget>(includeFields, exceptFields));
        }

        /// <summary>
        /// 从源类型设置目标类型的属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="target">目标类型变量</param>
        /// <param name="includeSelector">包含字段表达示，TTarget类型的这些字段将被从源类型赋值，如t=>new {t.Field1,t.Field2}</param>
        /// <param name="exceptSelector">排除字段表达示，TTarget类型的这些字段的值将不会改变，如t=>new {t.Field1,t.Field2}</param>
        public static void Map<TSource, TTarget>(TSource source, TTarget target, Expression<Func<TTarget, object>> includeSelector, Expression<Func<TTarget, object>> exceptSelector)
           where TSource : class, new()
           where TTarget : class, new()
        {
            Map(
                source,
                target,
                includeSelector == null ? null : GetAnonymousTypeFields(includeSelector),
                exceptSelector == null ? null : GetAnonymousTypeFields(exceptSelector)
                );
        }

        #endregion

        #region MapToNew重载
        /// <summary>
        /// 从源类型创建一个新的目标类型(推荐)
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <returns></returns>
        public static TTarget MapToNew<TSource, TTarget>(TSource source)
           where TSource : class, new()
           where TTarget : class, new()
        {
            return MapToNew<TSource, TTarget>(source, null);
        }

        /// <summary>
        /// 从源类型创建一个新的目标类型
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget MapToNew<TTarget>(object source)
           where TTarget : class, new()
        {
            return (TTarget)MapToNew(source.GetType(), typeof(TTarget), source, null);
        }

        /// <summary>
        /// 从源类型创建一个新的目标类型(推荐)
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <returns></returns>
        public static TTarget MapToNew<TSource, TTarget>(TSource source, List<string> includeFields, List<string> exceptFields)
           where TSource : class, new()
           where TTarget : class, new()
        {
            return MapToNew<TSource, TTarget>(source, GetIncludeFields<TTarget>(includeFields, exceptFields));
        }

        /// <summary>
        /// 从源类型创建一个新的目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源类型变量</param>
        /// <param name="includeSelector">包含字段表达示，TTarget类型的这些字段将被从源类型赋值，如t=>new {t.Field1,t.Field2}</param>
        /// <param name="exceptSelector">排除字段表达示，TTarget类型的这些字段的值将赋值为默认值，如t=>new {t.Field1,t.Field2}</param>
        /// <returns></returns>
        public static TTarget MapToNew<TSource, TTarget>(TSource source, Expression<Func<TTarget, object>> includeSelector, Expression<Func<TTarget, object>> exceptSelector)
          where TSource : class, new()
          where TTarget : class, new()
        {
            return MapToNew<TSource, TTarget>(
                source,
                includeSelector == null ? null : GetAnonymousTypeFields(includeSelector),
                exceptSelector == null ? null : GetAnonymousTypeFields(exceptSelector)
                );
        }
        #endregion
        #region MapToList重载
        public static List<TTarget> MapToList<TSource, TTarget>(List<TSource> sources)
           where TSource : class, new()
           where TTarget : class, new()
        {
            return MapToList<TSource, TTarget>(sources, null);
        }

        /// <summary>
        /// 将源类型列表转成目标类型列表
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="sources">源类型列表变量</param>
        /// <param name="includeFields">TTarget类型的这些字段将被从源类型赋值，为null时为所有匹配的字段都将被赋值</param>
        /// <param name="exceptFields">排除字段，为null或空时则不排除字段</param>
        /// <returns>生成的目标类型列表</returns>
        public static List<TTarget> MapToList<TSource, TTarget>(List<TSource> sources, List<string> includeFields, List<string> exceptFields)
            where TSource : class, new()
            where TTarget : class, new()
        {
            return MapToList<TSource, TTarget>(sources, GetIncludeFields<TTarget>(includeFields, exceptFields));
        }

        /// <summary>
        /// 将源类型列表转成目标类型列表
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="sources">源类型列表变量</param>
        /// <param name="includeSelector">包含字段表达示，TTarget类型的这些字段将被从源类型赋值，如t=>new {t.Field1,t.Field2}</param>
        /// <param name="exceptSelector">排除字段表达示，TTarget类型的这些字段的值将赋值为默认值，如t=>new {t.Field1,t.Field2}</param>
        /// <returns>生成的目标类型列表</returns>
        public static List<TTarget> MapToList<TSource, TTarget>(List<TSource> sources, Expression<Func<TTarget, object>> includeSelector, Expression<Func<TTarget, object>> exceptSelector)
        where TSource : class, new()
        where TTarget : class, new()
        {
            return MapToList<TSource, TTarget>(
                sources,
                includeSelector == null ? null : GetAnonymousTypeFields(includeSelector),
                exceptSelector == null ? null : GetAnonymousTypeFields(exceptSelector)
                );
        }

        #endregion
        #endregion


        /// <summary>
        /// EnsureMap
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="mapKey"></param>
        private static void EnsureMap<TSource, TTarget>(string mapKey)
            where TSource : class
            where TTarget : class
        {
            if (!_mapActions.ContainsKey(mapKey))
            {
                lock (locker)
                {
                    var mapAction = GenerateMapAction<TSource, TTarget>();
                    if (!_mapActions.ContainsKey(mapKey))
                    {
                        _mapActions.TryAdd(mapKey, mapAction);
                    }
                }
            }
        }

        /// <summary>
        /// EnsureMap
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <param name="mapKey"></param>
        private static void EnsureMap(Type sourceType, Type targetType, string mapKey)
        {
            if (!_mapActions.ContainsKey(mapKey))
            {
                lock (locker)
                {
                    var mapAction = GenerateMapAction(sourceType, targetType);
                    if (!_mapActions.ContainsKey(mapKey))
                    {
                        _mapActions.TryAdd(mapKey, mapAction);
                    }
                }
            }
        }

        /// <summary>
        /// 获取包含字段
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="includeFields">包含字段</param>
        /// <param name="exceptFields">排除字段</param>
        /// <returns></returns>
        private static List<string> GetIncludeFields<TTarget>(List<string> includeFields, List<string> exceptFields)
       where TTarget : class

        {

            if (includeFields == null && exceptFields == null)
            {
                return null;
            }
            else if (includeFields == null && exceptFields != null)
            {
                var allTargetFields = typeof(TTarget).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).Select(a => a.Name);
                return allTargetFields.Except(exceptFields).ToList();
            }
            else if (includeFields != null && exceptFields == null)
            {
                return includeFields;
            }
            else
            {
                //(includeFields != null && exceptFields != null)
                return includeFields.Except(exceptFields).ToList();
            }
        }

        /// <summary>
        /// 获取匿名类的字段，如获取target=>new {target.field1,target.field2}表达式中的匿名类字段field1,field2
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="selector">字段选择表达示</param>
        /// <returns></returns>
        private static List<string> GetAnonymousTypeFields<TTarget>(Expression<Func<TTarget, object>> selector)
        {
            if (selector.Body.NodeType == ExpressionType.New)
            {
                var expression = selector.Body as NewExpression;
                var fields = expression.Arguments.Where(a => a.NodeType == ExpressionType.MemberAccess)
                    .Select(a => (a as MemberExpression).Member.Name).ToList();
                return fields;
            }
            else
            {
                throw new ArgumentException(nameof(selector));
            }
        }

    }

}
