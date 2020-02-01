using System;
using System.Linq;
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
                var id = SnowflakeId.Create();
                var array = new string[10000];

                Parallel.For(0, 10000, i =>
                {
                    var id = IdGenerator.Generate<string>();
                    array[i] = id;
                });

                Assert.True(array.Distinct().Count() == 10000);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }


    }
}
