using System;
using System.Collections.Generic;
using System.Text;

namespace Fabric.Platform.Shared.Exceptions
{
    public class FabricConfigurationException : Exception
    {
        public FabricConfigurationException()
        { }

        public FabricConfigurationException(string message) : base(message)
        { }

        public FabricConfigurationException(string message, Exception inner) : base(message, inner)
        { }
    }
}
