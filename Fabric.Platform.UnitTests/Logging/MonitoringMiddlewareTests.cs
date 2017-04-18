using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;

namespace Fabric.Platform.UnitTests.Logging
{
    public class MonitoringMiddlewareTests
    {
        private AppFunc noOp = env => Task.FromResult(0);

        [Theory, MemberData(nameof(RequestData))]
        public void Inject_ReturnsNoContentStatus(string path, int statusCode, Func<Task<bool>> healthCheck)
        {
            var ctx = new OwinContext();
            ctx.Request.Scheme = LibOwin.Infrastructure.Constants.Https;
            ctx.Request.Path = new PathString(path); //"/_monitor/shallow"
            ctx.Request.Method = "GET";

            var pipeline = (new MonitoringMiddleware(noOp, healthCheck).Inject(ctx.Environment));
            Assert.Equal(statusCode, ctx.Response.StatusCode);
        }

        public static IEnumerable<object[]> RequestData
        {
            get
            {
                return new[]
                {
                    new object[] {"/_monitor/shallow", 204, (Func<Task<bool>>)(() => Task.FromResult(true)) },
                    new object[] {"/_monitor/deep", 204, (Func<Task<bool>>)(() => Task.FromResult(true)) },
                    new object[] {"/_monitor/deep", 503, (Func<Task<bool>>)(() => Task.FromResult(false)) },
                    new object[] {"/_monitor/nonexisting", 404, (Func<Task<bool>>)(() => Task.FromResult(true)) }
                };
            }
        }
    }
}
