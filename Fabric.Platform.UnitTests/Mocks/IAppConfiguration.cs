using System;
using System.Collections.Generic;
using System.Text;

namespace Fabric.Platform.UnitTests.Mocks
{
    public interface IAppConfiguration
    {
        CouchDbSettings CouchDbSettings { get; set; }
    }
}
