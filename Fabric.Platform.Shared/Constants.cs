namespace Fabric.Platform.Shared
{
    public static class Constants
    {
        public static class FabricHeaders
        {
            public const string SubjectNameHeader = "fabric-end-user-subject-id";
            public const string IdTokenHeader = "fabric-end-user";
            public const string CorrelationTokenHeaderName = "correlation-token";
            public const string AuthenticationHeaderPrefix = "Bearer";
        }

        public static class FabricLogContextProperties
        {
            public const string CorrelationTokenContextName = "CorrelationToken";
            public const string SubjectName = "FabricEndUserSubjectId";
        }

        public static class HttpMethods
        {
            public const string Get = "GET";
            public const string Put = "PUT";
            public const string Post = "POST";
            public const string Delete = "DELETE";
            public const string Options = "OPTIONS";
        }

        public static class FabricScopes
        {
            public const string AuthorizationRead = "fabric/authorization.read";
            public const string AuthorizationWrite = "fabric/authorization.write";
            public const string AuthorizationManageClients = "fabric/authorization.manageclients";
        }

        public static class FabricClaimTypes
        {
            public const string Groups = "groups";
        }
    }
}
