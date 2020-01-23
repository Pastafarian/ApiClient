using System.Net.Http;

namespace ApiClient
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient();
    }
}
