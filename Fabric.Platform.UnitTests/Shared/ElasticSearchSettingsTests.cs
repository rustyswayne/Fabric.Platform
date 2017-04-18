using Xunit;
using Fabric.Platform.Shared.Configuration;
using Fabric.Platform.Shared.Exceptions;
using System.Collections.Generic;
using System;

namespace Fabric.Platform.UnitTests.Shared
{
    public class ElasticSearchSettingsTests
    {
        [Theory, MemberData(nameof(MalformedData))]
        public void GetElasticSearchUri_InvalidSettings_Throws(ElasticSearchSettings elasticSearchSettings)
        {
            Assert.Throws<FabricConfigurationException>(() => elasticSearchSettings.GetElasticSearchUri());
        }

        [Theory, MemberData(nameof(ValidData))]
        public void GetElasticSearchUri_ValidSettings_CreatesUriSuccessfully(ElasticSearchSettings settings, Uri expectedUri)
        {
            Assert.Equal(expectedUri, settings.GetElasticSearchUri());
        }

        public static IEnumerable<object[]> MalformedData => new[]
        {
            new object[] {new ElasticSearchSettings() },
            new object[] { new ElasticSearchSettings { Server = "localhost", Port = "9200" } },
            new object[] {new ElasticSearchSettings { Scheme = "http", Port = "9200" } },
            new object[] { new ElasticSearchSettings { Scheme = "localhost", Server = "localhost" } }
        };

        public static IEnumerable<object[]> ValidData => new[]
        {
            new object[] { new ElasticSearchSettings { Scheme = "http", Server = "localhost", Port = "9200" }, new Uri("http://localhost:9200") },
            new object[] { new ElasticSearchSettings { Scheme = "http", Server = "localhost", Port = "9200", Username = "user", Password = "password" }, new Uri("http://username:password@localhost:9200") },
            new object[] { new ElasticSearchSettings { Scheme = "http", Server = "localhost", Port = "9200", Username = "user" }, new Uri("http://localhost:9200") },
            new object[] { new ElasticSearchSettings { Scheme = "http", Server = "localhost", Port = "9200", Username = "user", Password = "password" }, new Uri("http://localhost:9200") }
        };
    }



}
