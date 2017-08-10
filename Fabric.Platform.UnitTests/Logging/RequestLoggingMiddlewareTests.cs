using System.Threading.Tasks;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using LibOwin;
using Moq;

namespace Fabric.Platform.UnitTests.Logging
{
    public class RequestLoggingMiddlewareTests
    {
        private readonly AppFunc _noOp = env => Task.FromResult(0);
        [Fact]
        public void RequestLoggingMiddleware_Inject_CallsLogger()
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
            //ctx.Request.Headers.Add("TestHeader", new[] {"TestHeaderValue"});

            var loggerMock = new Mock<Platform.Logging.ILogger>();
            
            loggerMock.Setup(logger => logger.Information(It.IsAny<string>(), () => new object[] { It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>() })).Verifiable();
            loggerMock.Setup(logger => logger.Information(It.IsAny<string>(), () => new object[] { It.IsAny<int>(), It.IsAny<object>() })).Verifiable();

            //Act
            var pipeline = RequestLoggingMiddleware.Inject(_noOp, loggerMock.Object);
            pipeline(ctx.Environment);
            
            //Assert
            loggerMock.Verify();
        }
    }
}
