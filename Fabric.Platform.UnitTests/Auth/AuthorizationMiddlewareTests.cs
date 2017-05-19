using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabric.Platform.Auth;
using Fabric.Platform.UnitTests.Mocks;
using LibOwin;
using Xunit;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.UnitTests.Auth
{
    public class AuthorizationMiddlewareTests
    {
        private readonly AppFunc _noOp = env => Task.FromResult(0);

        [Theory, MemberData(nameof(RequestUser))]
        public void AuthorizationMiddleware_Inject_ReturnsForbiddenResponse(ClaimsPrincipal claimsPrincipal, string method, HttpStatusCode statusCode)
        {
            var ctx = new OwinContext
            {
                Request =
                {
                    Scheme = LibOwin.Infrastructure.Constants.Https,
                    Path = new PathString("/authtest"),
                    Method = method,
                    User = claimsPrincipal
                }
            };


            var pipeline = AuthorizationMiddleware.Inject(_noOp, new[] { "api1.read", "api1.write" });
            pipeline(ctx.Environment);
            Assert.Equal((int)statusCode, ctx.Response.StatusCode);
        }

        public static IEnumerable<object[]> RequestUser => new[]
        {
            new object[] { new TestPrincipal(new Claim("scope", "api1.read")), "GET", HttpStatusCode.OK },
            new object[] { new TestPrincipal(), "GET", HttpStatusCode.Forbidden },
            new object[] { new TestPrincipal(), "OPTIONS", HttpStatusCode.OK }
        };
    }
}
