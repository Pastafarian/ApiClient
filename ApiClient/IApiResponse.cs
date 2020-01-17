using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Avalara.ApiClient
{
    /// <summary>
    /// The API response.
    /// </summary>
    public interface IApiResponse
	{
		/// <summary>
		/// Response HTTP status code.
		/// </summary>
		HttpStatusCode StatusCode { get; }

		/// <summary>
		/// Underlying response message.
		/// </summary>
		HttpResponseMessage HttpResponseMessage { get; }

		/// <summary>
		/// Returns the response content from HttpResponseMessage as a string.
		/// </summary>
		string ResponseContent { get; }

        /// <summary>
        /// Throws an exception if the IsSuccessStatusCode property for the underlying HttpResponseMessage is false.
        /// </summary>
        /// <returns></returns>
	    IApiResponse EnsureSuccessStatusCode();
	}

	/// <summary>
	/// The Api response with generics for strongly typed deserialization.
	/// </summary>
	/// <typeparam name="T">Type of data to deserialize to</typeparam>
	public interface IApiResponse<T> : IApiResponse
	{
		/// <summary>
		/// Type deserialized from response.
		/// </summary>
		/// <exception cref="JsonReaderException">Thrown if there are issues deserializeing the resopnse as JSON.</exception>
		T Data { get; set; }

	    /// <summary>
	    /// Throws an exception if the IsSuccessStatusCode property for the underlying HttpResponseMessage is false.
	    /// </summary>
	    /// <returns></returns>
	    new IApiResponse<T> EnsureSuccessStatusCode();
	}

	
}
