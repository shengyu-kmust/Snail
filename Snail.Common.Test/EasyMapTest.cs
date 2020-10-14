using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Snail.Common.Test
{
    public class EasyMapTest
    {

        /// <summary>
        /// 对一百万条数据的属性映射进行性能测试，测试如下三种方法
        /// 1、用automap
        /// 2、objectExtenssion的反射方法
        /// 3、objectPropertyMap的表达式方法
        /// 下面是测试结果
        /// 方法       耗时ticks
        /// automap	   2160054		
		/// 反射	   45007174		
        /// 表达式     2206023		
        /// </summary>
        [Fact]
        public void MapListPerformanceTest()
        {
            try
            {
                var times = 1000000;
                var sources = Enumerable.Range(1, 100 * 100 * 100).Select(a => new Source { Name = "1", Age = 1, Birth = DateTime.Now, NoMatch1 = "noMatchField", Week = DayOfWeek.Wednesday }).ToList();

                //MapperConfiguration cfg = new MapperConfiguration(c =>
                //{
                //    c.CreateMap<Source, Target>();
                //});
                //var autoMap = cfg.CreateMapper();


                //Stopwatch autoMapWatch = Stopwatch.StartNew();
                //var autoMapTargets = autoMap.Map<List<Source>, List<Target>>(sources);
                //autoMapWatch.Stop();


                Stopwatch expressionWatch = Stopwatch.StartNew();
                var expressionsTargets = EasyMap.MapToList<Source, Target>(sources);
                expressionWatch.Stop();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }


        /// <summary>
        /// 对一百万条数据的属性映射进行性能测试，测试如下三种方法
        /// 1、用automap
        /// 2、objectExtenssion的反射方法
        /// 3、objectPropertyMap的表达式方法
        /// 下面是测试结果
        /// 方法       耗时ticks
        /// automap	   2490322	
		/// 反射	   41694873	
        /// 表达式     458560	
        /// </summary>
        [Fact]
        public void MapSinglePerformanceTest()
        {
            try
            {
                var times = 1000000;
                var source = new Source { Name = "1", Age = 1, Birth = DateTime.Now, NoMatch1 = "noMatchField", Week = DayOfWeek.Wednesday };
                var target = new Target();

                //MapperConfiguration cfg = new MapperConfiguration(c =>
                //{
                //    c.CreateMap<Source, Target>();
                //});
                //var autoMap = cfg.CreateMapper();


                //Stopwatch autoMapWatch = Stopwatch.StartNew();
                //for (int i = 0; i < times; i++)
                //{
                //    autoMap.Map(source, target, typeof(Source), typeof(Target));
                //}
                //autoMapWatch.Stop();



                var sourceType = typeof(Source);
                var targetType = typeof(Target);
                Stopwatch expressionWatch = Stopwatch.StartNew();
                var mapAction = EasyMap.GetMapAction<Source, Target>();
                for (int i = 0; i < times; i++)
                {
                    mapAction(source, target, null);// 最快，比automap快5倍
                    //EasyMap.Map(source, target,null); // 和automap差不多
                    //EasyMap.Map(sourceType, targetType, source, target, null); // 为automap的5倍时间

                }
                expressionWatch.Stop();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        [Fact]
        public void MapTest()
        {
            try
            {
                var sourceType = typeof(Source);
                var targetType = typeof(Target);
                var includeFields = new List<string> { "Name", "Age", "Birth", "Week", "ExceptField" };
                Expression<Func<Target, object>> includeSelector = t => new { t.Name, t.Age, t.Birth, t.Week };
                var exceptFields = new List<string> { "ExceptField" };
                Expression<Func<Target, object>> exceptSelector = t => new { t.ExceptField };
                var source = new Source { Name = "zhoujing", Age = 30, Birth = new DateTime(1989, 1, 1), NoMatch1 = "未匹配的字段，不会赋值到target", Week = DayOfWeek.Wednesday, ExceptField = "这个是排除的字段", NotIncludeField = "这个是不包含字段" };
                var target = new Target();

                target.Clear();
                EasyMap.Map(sourceType, targetType, source, target, includeFields);
                Assert.True(IsMatch(source, target, false, true));

                target.Clear();
                EasyMap.Map(source, target);
                Assert.True(IsMatch(source, target, false, false));

                target.Clear();
                EasyMap.Map(source, target, includeFields);
                Assert.True(IsMatch(source, target, false, true));

                target.Clear();
                EasyMap.Map(source, target, includeSelector, exceptSelector);
                Assert.True(IsMatch(source, target, true, true));

                target.Clear();
                EasyMap.Map(source, target, includeFields, exceptFields);
                Assert.True(IsMatch(source, target, true, true));
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        [Fact]
        public void MapToNewTest()
        {
            try
            {
                var sourceType = typeof(Source);
                var targetType = typeof(Target);
                var includeFields = new List<string> { "Name", "Age", "Birth", "Week", "ExceptField" };
                Expression<Func<Target, object>> includeSelector = t => new { t.Name, t.Age, t.Birth, t.Week };
                var exceptFields = new List<string> { "ExceptField" };
                Expression<Func<Target, object>> exceptSelector = t => new { t.ExceptField };
                var source = new Source { Name = "zhoujing", Age = 30, Birth = new DateTime(1989, 1, 1), NoMatch1 = "未匹配的字段，不会赋值到target", Week = DayOfWeek.Wednesday, ExceptField = "这个是排除的字段", NotIncludeField = "这个是不包含字段" };
                var target = new Target();

                target.Clear();
                target = (Target)EasyMap.MapToNew(sourceType, targetType, source, includeFields);
                Assert.True(IsMatch(source, target, false, true));

                target.Clear();
                target = EasyMap.MapToNew<Target>(source);
                Assert.True(IsMatch(source, target, false, false));

                target.Clear();
                target = EasyMap.MapToNew<Source, Target>(source);
                Assert.True(IsMatch(source, target, false, false));

                target.Clear();
                target = EasyMap.MapToNew<Source, Target>(source, includeFields);
                Assert.True(IsMatch(source, target, false, true));


                target.Clear();
                target = EasyMap.MapToNew(source, includeSelector, exceptSelector);
                Assert.True(IsMatch(source, target, true, true));

                target.Clear();
                target = EasyMap.MapToNew<Source, Target>(source, includeFields, exceptFields);
                Assert.True(IsMatch(source, target, true, true));

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Fact]
        public void MapToListTest()
        {
            try
            {
                var sourceType = typeof(Source);
                var targetType = typeof(Target);
                var includeFields = new List<string> { "Name", "Age", "Birth", "Week", "ExceptField" };
                Expression<Func<Target, object>> includeSelector = t => new { t.Name, t.Age, t.Birth, t.Week };
                var exceptFields = new List<string> { "ExceptField" };
                Expression<Func<Target, object>> exceptSelector = t => new { t.ExceptField };
                var sources = new List<Source> { new Source { Name = "zhoujing", Age = 30, Birth = new DateTime(1989, 1, 1), NoMatch1 = "未匹配的字段，不会赋值到target", Week = DayOfWeek.Wednesday, ExceptField = "这个是排除的字段", NotIncludeField = "这个是不包含字段" } };
                Target target;


                var targets = EasyMap.MapToList<Source, Target>(sources);
                Assert.True(targets.All(target => IsMatch(sources[0], target, false, false)));

                targets.ForEach(target => target.Clear());
                targets = EasyMap.MapToList<Source, Target>(sources, includeFields);
                Assert.True(targets.All(target => IsMatch(sources[0], target, false, true)));

                targets.ForEach(target => target.Clear());
                targets = EasyMap.MapToList(sources, includeSelector, exceptSelector);
                Assert.True(targets.All(target => IsMatch(sources[0], target, true, true)));


                targets.ForEach(target => target.Clear());
                targets = EasyMap.MapToList<Source, Target>(sources, includeFields, exceptFields);
                Assert.True(targets.All(target => IsMatch(sources[0], target, true, true)));


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public bool IsMatch(Source source, Target target, bool hasException, bool hasInclude)
        {
            var result = source.Name == target.Name && source.Age == target.Age && source.Birth == target.Birth && source.Week == target.Week && target.NoMatch2 == "初始值" && (hasException ? target.ExceptField == "初始值" : source.ExceptField == target.ExceptField)
                && (hasInclude ? target.NotIncludeField == "初始值" : source.NotIncludeField == target.NotIncludeField)
                ;
            return result;
        }
    }

    public class Source
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public DateTime? Birth { get; set; }
        public DayOfWeek? Week { get; set; }
        public string? NoMatch1 { get; set; }
        public string ExceptField { get; set; }
        public string NotIncludeField { get; set; }
    }
    public class Target
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public DayOfWeek Week { get; set; }
        public string NoMatch2 { get; set; } = "初始值";
        public string ExceptField { get; set; } = "初始值";
        public string NotIncludeField { get; set; } = "初始值";
        public void Clear()
        {
            Name = default;
            Age = default;
            Birth = default;
            Week = default;
            NoMatch2 = "初始值";
            ExceptField = "初始值";
            NotIncludeField = "初始值";
        }
    }
}
