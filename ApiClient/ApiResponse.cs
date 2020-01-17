using System;
using System.Net;
using System.Net.Http;
using Avalara.ApiClient.Serialization;
using Newtonsoft.Json;

namespace Avalara.ApiClient
{
	/// <summary>
	/// Simple implementation of the ApiResponse
	/// </summary>
	public class ApiResponse : IApiResponse
	{
		public HttpStatusCode StatusCode => HttpResponseMessage.StatusCode;

	    public HttpResponseMessage HttpResponseMessage { get; set; }

		private string _responseContent = null;
		public string ResponseContent
		{
			get
			{
				if (_responseContent == null)
				{
					_responseContent = HttpResponseMessage.Content.ReadAsStringAsync().Result ?? "";
				}
				return _responseContent;
				
			}
		}

	    public IApiResponse EnsureSuccessStatusCode()
	    {
	        HttpResponseMessage.EnsureSuccessStatusCode();
	        return this;
	    }
	}

    /// <summary>
	/// Simple implementation of generic ApiResponse.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResponse<T> : ApiResponse, IApiResponse<T>
	{
		private T _data;
		private bool _deserialized = false;
	    private readonly ISerializer _serializer;

	    internal ApiResponse(ISerializer serializer)
	    {
	        _serializer = serializer;
	    }

		public T Data
		{
			get
			{
				if (_deserialized)
				{
					return _data;
				}

				try
				{
					_data = _serializer.DeserializeObject<T>(ResponseContent);
				    _deserialized = true;
				}
				catch (JsonReaderException jrEx)
				{
					if (HttpResponseMessage.Content.Headers.ContentType != null &&
						!string.Equals(HttpResponseMessage.Content.Headers.ContentType.MediaType, "application/json", StringComparison.InvariantCultureIgnoreCase))
					{
						throw new JsonReaderException(string.Format("Response is not JSON. Content Type is: {0},", HttpResponseMessage.Content.Headers.ContentType.MediaType), jrEx);
					}
					throw;
				}

			

				return _data;
			}
			set => _data = value;
		}

	    public new IApiResponse<T> EnsureSuccessStatusCode()
	    {
	        HttpResponseMessage.EnsureSuccessStatusCode();
	        return this;
	    }

        /// <summary>
        /// Convenience class for creating a deserialized ApiResponse.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="httpResponseMessage"></param>
        /// <returns></returns>
	    public static ApiResponse<T> Create(T data, HttpResponseMessage httpResponseMessage)
	    {
            return new ApiResponse<T>(null)
            {
                Data = data,
                HttpResponseMessage = httpResponseMessage,
                _deserialized = true
            };
	    }
	}  
}