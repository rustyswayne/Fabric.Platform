using System;
using System.Threading.Tasks;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using LibOwin;
using Moq;

namespace Fabric.Platform.UnitTests.Logging
{
    public class PerformanceLoggingMiddlewareTests
    {
        private readonly AppFunc _noOp = env => Task.FromResult(0);
        [Fact]
        public void PerformanceLoggingMiddleware_Inject_RecordsPerfLogs()
        {
            //Arrange
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString("/logtest"),
                    Method = "GET"
                }
            };

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(logger => logger.Information(It.IsAny<string>(), () => new object[] { It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Int64>() })).Verifiable();

            //Act
            var pipeline = PerformanceLoggingMiddleware.Inject(_noOp, loggerMock.Object);
            pipeline(ctx.Environment);
            
            loggerMock.Verify();
        }
    }
}
