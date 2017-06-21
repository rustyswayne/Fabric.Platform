using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fabric.Platform.Shared.Configuration.Docker;
using Fabric.Platform.UnitTests.Mocks;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Fabric.Platform.UnitTests.Shared
{
    public class DockerSecretsConfigProviderTests
    {
        private readonly DockerSecretsConfigProvider _dockerSecretsConfigProvider;

        public DockerSecretsConfigProviderTests()
        {
            var fileProvider = new EmbeddedFileProvider(typeof(DockerSecretsConfigProviderTests).GetTypeInfo().Assembly, "Fabric.Platform.UnitTests.Secrets");
            _dockerSecretsConfigProvider = new DockerSecretsConfigProvider(fileProvider, typeof(IAppConfiguration));
            _dockerSecretsConfigProvider.Load();
        }
        [Fact]
        public void Load_LoadsSecrets_Successfully()
        {
            var hasPassword = _dockerSecretsConfigProvider.TryGet("CouchDbSettings:Password", out string password);
            Assert.True(hasPassword);
            Assert.Equal("password", password);
        }

        [Fact]
        public void Load_IgnoresSecrets_WithoutCorrespondingSettings()
        {
            var hasNonExistantSetting =
                _dockerSecretsConfigProvider.TryGet("NonExistantSetting", out string nonExistantSetting);
            Assert.False(hasNonExistantSetting);
            Assert.Null(nonExistantSetting);
        }
    }
}
