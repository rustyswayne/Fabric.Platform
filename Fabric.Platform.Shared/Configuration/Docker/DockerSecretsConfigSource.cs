using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Fabric.Platform.Shared.Configuration.Docker
{
    public class DockerSecretsConfigSource : IConfigurationSource
    {
        private readonly IFileProvider _fileProvider;
        private readonly Type _configurationType;

        public DockerSecretsConfigSource(IFileProvider fileProvider, Type configurationType)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _configurationType = configurationType ?? throw new ArgumentNullException(nameof(configurationType));
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DockerSecretsConfigProvider(_fileProvider, _configurationType);
        }
    }
}
