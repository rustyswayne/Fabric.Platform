using System;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using LibOwin;
using Moq;
using Serilog;

namespace Fabric.Platform.UnitTests.Logging
{
    public class GlobalErrorLoggingMiddlewareTests
    {
        private readonly AppFunc _noOp = env => throw new Exception();

        [Fact]
        public void GloblaErrorLoggingMiddleware_Inject_RecordsException()
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
            loggerMock.Setup(logger => logger.ForContext<GlobalErrorLoggingMiddleware>()).Returns(() => loggerMock.Object);
            loggerMock.Setup(logger => logger.Error(It.IsAny<Exception>(), It.IsAny<string>())).Verifiable();

            //Act
            var pipeline = GlobalErrorLoggingMiddleware.Inject(_noOp, loggerMock.Object);
            pipeline(ctx.Environment);

            loggerMock.Verify();
        }
    }
}
