using System;
using Xunit;

namespace Nexaas.ID.Client.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var url1 = "http://localhost".AddQueryStringParameter("test", "123456");
            
            var url2 = "http://localhost?".AddQueryStringParameter("test", "12 / 3456");
        }
    }
}