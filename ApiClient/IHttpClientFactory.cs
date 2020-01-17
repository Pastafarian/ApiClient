using System.Net.Http;

namespace Avalara.ApiClient
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient();
    }
}
