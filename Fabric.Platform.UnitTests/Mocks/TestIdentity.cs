using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Fabric.Platform.UnitTests.Mocks
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }
}
