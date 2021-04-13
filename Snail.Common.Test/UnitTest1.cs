using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Snail.Common.Extenssions;
using Xunit;

namespace Snail.Common.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            try
            {
                var pred = ExpressionExtensions.True<A>();
                pred.AndIf(true,a => a.Name == "1");
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }
        public class A
        {
            public string Name { get; set; }
        }

    }
}
