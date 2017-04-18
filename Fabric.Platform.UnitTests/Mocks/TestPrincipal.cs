using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Fabric.Platform.UnitTests.Mocks
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }
}
