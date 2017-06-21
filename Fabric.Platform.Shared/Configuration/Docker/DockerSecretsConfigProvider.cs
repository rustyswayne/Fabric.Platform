using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Fabric.Platform.Shared.Configuration.Docker
{
    public class DockerSecretsConfigProvider : ConfigurationProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly Type _configurationType;

        public DockerSecretsConfigProvider(IFileProvider fileProvider, Type configurationType)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _configurationType = configurationType ?? throw new ArgumentNullException(nameof(configurationType));
        }

        public override void Load()
        {
            Data = new Dictionary<string, string>();
            var contents = _fileProvider.GetDirectoryContents("");
            foreach (var item in contents)
            {
                if (item.IsDirectory) continue;

                using (var secretStream = item.CreateReadStream())
                using (var streamReader = new StreamReader(secretStream))
                {
                    var value = streamReader.ReadToEnd().Trim();
                    var key = item.Name.Replace("__", ConfigurationPath.KeyDelimiter);
                    if (!string.IsNullOrEmpty(value) && ConfigSettingExists(key))
                    {
                        Data.Add(key, value);
                    }
                }
            }
        }

        private bool ConfigSettingExists(string key)
        {
            var type = _configurationType;
            var parts = key.Split(':');
            foreach (var part in parts)
            {
                var property = type.GetRuntimeProperty(part);
                if (property == null) return false;
                type = property.PropertyType;
            }
            return true;
        }
    }
}
