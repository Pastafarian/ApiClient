using System.Net.Http;
using System.Threading.Tasks;

namespace Avalara.ApiClient.Authorization
{
    /// <summary>
    /// Implentation of <see cref="IRequestAuthorization"/> that does nothing.
    /// </summary>
    public class NoRequestAuthorization : IRequestAuthorization
    {
        public Task Authorize(HttpRequestMessage requestMessage)
        {
            return Task.CompletedTask;
        }

        public Task Expire()
        {
            return Task.CompletedTask;
        }
    }
}