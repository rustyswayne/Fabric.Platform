using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LibOwin;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Logging;
using Fabric.Platform.Shared;
using Fabric.Platform.UnitTests.Mocks;

namespace Fabric.Platform.UnitTests.Logging
{
    public class SubjectCorrelationMiddlewareTests
    {
        private readonly AppFunc _noOp = env => Task.FromResult(0);
        private const string TestUser = "testuser";
        [Fact]
        public void UserIdMiddleware_Inject_AddsUserIdFromContext()
        {
            //Arrange
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString("/"),
                    Method = "GET",
                    User = new TestPrincipal(new Claim(SubjectCorrelationMiddleware.SubClaim, TestUser))
                }
            };

            //Act
            var pipeline = SubjectCorrelationMiddleware.Inject(_noOp);
            pipeline(ctx.Environment);

            //Assert
            var actualUserId = ctx.Environment[Constants.FabricHeaders.IdTokenHeader];
            Assert.Equal(TestUser, actualUserId);

        }

        [Fact]
        public void UserIdMiddleware_Inject_DoesNotAddUserIdWhenNotPresent()
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

            //Act
            var pipeline = SubjectCorrelationMiddleware.Inject(_noOp);
            pipeline(ctx.Environment);

            //Assert
            var hasUserId = ctx.Environment.ContainsKey(Constants.FabricHeaders.IdTokenHeader);
            Assert.False(hasUserId);

        }

        [Fact]
        public void UserIdCorrelationMiddleware_Inject_UsesExistingUserIdWhenPresent()
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
            ctx.Request.Headers.Append(Constants.FabricHeaders.IdTokenHeader, TestUser);

            //Act
            var pipeline = SubjectCorrelationMiddleware.Inject(_noOp);
            pipeline(ctx.Environment);

            //Assert
            var actualUserId = ctx.Environment[Constants.FabricHeaders.IdTokenHeader];
            Assert.Equal(TestUser, actualUserId);
        }
    }
}
