using System;
using Fabric.Platform.Shared.Exceptions;

namespace Fabric.Platform.Shared.Configuration
{
    public class ElasticSearchSettings
    {
        public string Scheme { get; set; }
        public string Server { get; set; }

        public string Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string BufferBaseFilename { get; set; } = "./logs/buffer";

        public Uri GetElasticSearchUri()
        {
            if (string.IsNullOrEmpty(Scheme) || string.IsNullOrEmpty(Server) ||
                string.IsNullOrEmpty(Port))
            {
                throw new FabricConfigurationException($"You must specify the {nameof(Scheme)}, {nameof(Server)} and {nameof(Port)} settings for elastic search.");
            }

            if (!string.IsNullOrEmpty(Username) &&
                !string.IsNullOrEmpty(Password))
            {
                return new Uri(
                    $"{Scheme}://{Username}:{Password}@{Server}:{Port}");
            }

            return new Uri($"{Scheme}://{Server}:{Port}");
        }

    }
}
