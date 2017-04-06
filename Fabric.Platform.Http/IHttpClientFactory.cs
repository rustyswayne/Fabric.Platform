using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fabric.Platform.Http
{
    public interface IHttpClientFactory
    {
        Task<HttpClient> Create(Uri uri, string requestScope);
        Task<HttpClient> CreateWithAccessToken(Uri uri, string accessToken);
    }
}
