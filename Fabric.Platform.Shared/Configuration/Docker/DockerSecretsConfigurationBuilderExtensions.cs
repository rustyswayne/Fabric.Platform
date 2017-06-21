using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Fabric.Platform.Shared.Configuration.Docker
{
    public static class DockerSecretsConfigurationBuilderExtensions
    {
        private static readonly string DockerSecretsDirectory = @"/run/secrets";
        public static IConfigurationBuilder AddDockerSecrets(this IConfigurationBuilder configurationBuilder, Type configurationType)
        {
            if (Directory.Exists(DockerSecretsDirectory))
            {
                var fileProvider = new PhysicalFileProvider(DockerSecretsDirectory);
                return configurationBuilder.Add(new DockerSecretsConfigSource(fileProvider, configurationType));
            }

            return configurationBuilder;
        }
    }
}
