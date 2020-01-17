using System.Net.Http;
using System.Threading.Tasks;

namespace Avalara.ApiClient.Authorization
{
	/// <summary>
	/// Interface to encapsulate applying authorization to a REST request.
	/// </summary>
	public interface IRequestAuthorization
	{
		/// <summary>
		/// Applies authorization to request.
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <exception cref="AuthorizationException"></exception>
		Task Authorize(HttpRequestMessage requestMessage);

	    Task Expire();
	}
}