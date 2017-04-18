using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using Serilog.Core;
using Serilog.Events;

namespace Fabric.Platform.UnitTests.Logging
{
    public class DiagnosticsMiddlewareTests
    {

        private AppFunc noOp = env => Task.FromResult(0);

        [Theory, MemberData(nameof(RequestData))]
        public void Inject_ReturnsCorrectStatus(string path, string queryString, int expectedStatusCode, LogEventLevel expectedLogLevel)
        {
            var ctx = new OwinContext();
            ctx.Request.Scheme = LibOwin.Infrastructure.Constants.Https;
            ctx.Request.Path = new PathString(path); //"/_monitor/shallow"
            ctx.Request.Method = "GET";
            ctx.Request.QueryString = new QueryString(queryString);

            var levelSwitch = new LoggingLevelSwitch();

            var pipeline = (new DiagnosticsMiddleware(noOp, levelSwitch).Inject(ctx.Environment));
            Assert.Equal(expectedStatusCode, ctx.Response.StatusCode);
            Assert.Equal(expectedLogLevel, levelSwitch.MinimumLevel);
        }

        public static IEnumerable<object[]> RequestData
        {
            get
            {
                return new[]
                {
                    new object[] { "/_diagnostics", "LogLevel=Verbose", 204, LogEventLevel.Verbose },
                    new object[] { "/_diagnostics", "LogLevel=Error", 204, LogEventLevel.Error },
                    new object[] { "/_diagnostics", "LogLevel=BadLevel", 400, LogEventLevel.Information }
                };
            }
        }
    }
}
