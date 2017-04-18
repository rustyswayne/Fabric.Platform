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

        private readonly AppFunc _noOp = env => Task.FromResult(0);

        [Theory, MemberData(nameof(RequestData))]
        public void Inject_ReturnsCorrectStatus(string path, string queryString, int expectedStatusCode, LogEventLevel expectedLogLevel)
        {
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString(path),
                    Method = "GET",
                    QueryString = new QueryString(queryString)
                }
            };

            var levelSwitch = new LoggingLevelSwitch();

            var pipeline = new DiagnosticsMiddleware(_noOp, levelSwitch);
            pipeline.Inject(ctx.Environment);
            Assert.Equal(expectedStatusCode, ctx.Response.StatusCode);
            Assert.Equal(expectedLogLevel, levelSwitch.MinimumLevel);
        }

        public static IEnumerable<object[]> RequestData => new[]
        {
            new object[] { "/_diagnostics", "LogLevel=Verbose", 204, LogEventLevel.Verbose },
            new object[] { "/_diagnostics", "LogLevel=Error", 204, LogEventLevel.Error },
            new object[] { "/_diagnostics", "LogLevel=BadLevel", 400, LogEventLevel.Information }
        };
    }
}
