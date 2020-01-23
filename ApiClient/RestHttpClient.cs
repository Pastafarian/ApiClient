using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiClient.Authorization;
using ApiClient.Serialization;

namespace ApiClient
{
    /// <summary>
    /// Simple REST client implementation.
    /// </summary>
    public class RestHttpClient : IRestHttpClient
    {
        private readonly IRequestAuthorization _requestAuthorization;
        private readonly bool _retryIfUnauthorized;
        private readonly ISerializer _serializer;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="requestAuthorization"></param>
        /// <param name="retryIfUnauthorized"></param>
        /// <param name="serializer">Provide overrides to the <see cref="DefaultSerializer"/> if required.</param>
        /// <param name="clientFactory">Optional factory interface to Create HttpClient. For .Net Core 2.1 consumers this could leverage HttpClientFactory.</param>
        /// <remarks>Note that all internal awaits are implemented using Task.ConfigureAwait(false) to avoid deadlocs when called from ASP.Net request
        /// threads or UI threads. This basically means you won't get a deadlock waiting on the result from a UI thread, but as a best practice and for
        /// efficiency try and await all the way up the call chain.</remarks>
        public RestHttpClient(IRequestAuthorization requestAuthorization, bool retryIfUnauthorized, ISerializer serializer = null, IHttpClientFactory clientFactory = null)
        {
            _requestAuthorization = requestAuthorization;
            _retryIfUnauthorized = retryIfUnauthorized;
            _serializer = serializer ?? new DefaultSerializer();
            _clientFactory = clientFactory;
        }

        public async Task<IApiResponse> GetAsync(string endpoint)
        {
            return new ApiResponse
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Get).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse<TResponse>> GetAsync<TResponse>(string endpoint)
        {
            return new ApiResponse<TResponse>(_serializer)
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Get).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse> PostAsync(object data, string endpoint)
        {
            return new ApiResponse
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Post, data).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse<TResponse>> PostAsync<TResponse>(object data, string endpoint)
        {
            return new ApiResponse<TResponse>(_serializer)
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Post, data).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse> PutAsync(object data, string endpoint)
        {
            return new ApiResponse
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Put, data).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse<TResponse>> PutAsync<TResponse>(object data, string endpoint)
        {
            return new ApiResponse<TResponse>(_serializer)
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Put, data).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse> DeleteAsync(object data, string endpoint)
        {
            return new ApiResponse
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Delete, data).ConfigureAwait(false)
            };
        }

        public async Task<IApiResponse<TResponse>> DeleteAsync<TResponse>(object data, string endpoint)
        {
            return new ApiResponse<TResponse>(_serializer)
            {
                HttpResponseMessage = await SendAsync(endpoint, HttpMethod.Delete, data).ConfigureAwait(false)
            };
        }

        private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, object data = null)
        {
            var httpClient = GetHttpClient();
            var request = await GetRequestMessage(endpoint, method, data);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (_retryIfUnauthorized && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _requestAuthorization.Expire();
                request = await GetRequestMessage(endpoint, method, data);
                response = await httpClient.SendAsync(request).ConfigureAwait(false);
            }

            return response;
        }

        public async Task<string> DownloadAsync(string filePath, string endpoint)
        {
            using (var httpResponseMessage = await SendAsync(endpoint, HttpMethod.Get).ConfigureAwait(false))
            {
                var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return httpResponseMessage.Content.Headers.ContentDisposition.FileName;
            }
        }

        private HttpClient GetHttpClient()
        {
            return _clientFactory == null ? new HttpClient() : _clientFactory.GetHttpClient();
        }

        private async Task<HttpRequestMessage> GetRequestMessage(string endpoint, HttpMethod method, object data = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(endpoint),
                Method = method
            };

            if (data != null)
            {
                request.Content = data is HttpContent content ? content : HttpContentFromObject(data);
            }

            try
            {
                await _requestAuthorization.Authorize(request);
            }
            catch (AuthorizationException) { throw; }
            catch (Exception ex)
            {
                throw new AuthorizationException("Exception authorizing request. See inner exception for details.", ex);
            }

            return request;
        }

        private HttpContent HttpContentFromObject(object data)
        {
            return new StringContent(_serializer.SerializeObject(data), Encoding.UTF8, "application/json");
        }
    }
}
