using System.Net.Http;
using System.Threading.Tasks;
using ApiClient.Authorization;

namespace ApiClient
{
    /// <summary>
    /// Simple REST client helper interface.
    /// </summary>
    public interface IRestHttpClient
    {
        /// <summary>
        /// For REST GET requests.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse> GetAsync(string endpoint);

        /// <summary>
        /// For REST GET requests.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse<TResponse>> GetAsync<TResponse>(string endpoint);

        /// <summary>
        /// For REST POST requests.
        /// </summary>
        /// <param name="data">
        /// Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse> PostAsync(object data, string endpoint);

        /// <summary>
        /// For REST POST requests.
        /// </summary>
        /// <param name="data">Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse<TResponse>> PostAsync<TResponse>(object data, string endpoint);

        /// <summary>
        /// For REST PUT requests.
        /// </summary>
        /// <param name="data">Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse> PutAsync(object data, string endpoint);

        /// <summary>
        /// For REST PUT requests.
        /// </summary>
        /// <param name="data">Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse<TResponse>> PutAsync<TResponse>(object data, string endpoint);

        /// <summary>
        /// For REST DELETE requests.
        /// </summary>
        /// <param name="data">Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        Task<IApiResponse> DeleteAsync(object data, string endpoint);

        /// <summary>
        /// For REST DELETE requests.
        /// </summary>
        /// <param name="data">Data to post.
        /// <para>The value will be serialized unless it is of type (or dervied from) HttpContent.</para>
        /// </param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<IApiResponse<TResponse>> DeleteAsync<TResponse>(object data, string endpoint);

        /// <summary>
        /// Downloads as a file.
        /// </summary>
        /// <param name="filePath">The file path of the file to create.</param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="AuthorizationException">Thrown if there was an error during authorization.</exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<string> DownloadAsync(string filePath, string endpoint);
    }
}