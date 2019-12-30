using System;
using Xunit;

namespace Snail.Common.Test
{
    public class RSAHelperTest
    {
        [Fact]
        public void Test1()
        {
            var keys=RSAHelper.GeneratePemRsaKeys();
        }
    }
}
