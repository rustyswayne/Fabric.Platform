using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibOwin;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using Fabric.Platform.Shared;

namespace Fabric.Platform.UnitTests.Logging
{
    public class CorrelationTokenMiddlewareTests
    {
        private readonly AppFunc _noOp = env => Task.FromResult(0);
        [Fact]
        public void CorrelationTokenMiddleware_Inject_AddsCorrelationTokenWhenNotPresent()
        {
            //Arrange
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString("/"),
                    Method = "GET"
                }
            };

            //Act
            var pipeline = CorrelationTokenMiddleware.Inject(_noOp);
            pipeline(ctx.Environment);

            //Assert
            var isGuid = Guid.TryParse(ctx.Environment[Constants.FabricLogContextProperties.CorrelationTokenContextName] as string, out Guid correlationToken);

            Assert.True(isGuid);
            Assert.NotNull(correlationToken);

        }

        [Fact]
        public void CorrelationTokenMiddleware_Inject_UsesExistingCorrelationTokenWhenPresent()
        {
            //Arrange
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString("/"),
                    Method = "GET",
                }
            };
            var incomingCorrelationToken = Guid.NewGuid();
            ctx.Request.Headers.Append(Constants.FabricHeaders.CorrelationTokenHeaderName, incomingCorrelationToken.ToString());

            //Act
            var pipeline = CorrelationTokenMiddleware.Inject(_noOp);
            pipeline(ctx.Environment);

            //Assert
            var isGuid = Guid.TryParse(
                ctx.Environment[Constants.FabricLogContextProperties.CorrelationTokenContextName] as string,
                out Guid correlationToken);
            Assert.True(isGuid);
            Assert.Equal(incomingCorrelationToken, correlationToken);
        }
    }
}
