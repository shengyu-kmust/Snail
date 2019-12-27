using System;
using Snail.Common.Extenssions;
using Xunit;

namespace Snail.Common.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var str = "AREYouOkAHOU";
            var str1 = str.ToSnakeCase();
        }
    }
}
