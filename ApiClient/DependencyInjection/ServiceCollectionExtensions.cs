using Avalara.ApiClient.Authorization;
using Avalara.ApiClient.Authorization.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace Avalara.ApiClient.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the container to return <see cref="IRestHttpClient"/> using ClientCredentialsTokenRequestAuthorization for authorization.
        /// <para>Note: this configures ClientCredentialsTokenRequestAuthorization as a singleton so don't use this if using <see cref="IRestHttpClient"/>
        /// against multiple API's with different authorization.</para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="idServerSettings"></param>
        /// <param name="retryIfUnauthorized"></param>
        /// <returns></returns>
        /// <remarks>ClientCredentialsTokenRequestAuthorization is configured as a singleton so that tokens are shared between instances of IRestHttpClient.</remarks>
        public static IServiceCollection ConfigureForClientCredentials(this IServiceCollection services, IdServerSettings idServerSettings, bool retryIfUnauthorized = true)
        {
            services.AddSingleton<IRequestAuthorization>(new ClientCredentialsTokenRequestAuthorization(idServerSettings));
            services.AddTransient<IRestHttpClient, RestHttpClient>(
                serviceProvider => new RestHttpClient(serviceProvider.GetService<IRequestAuthorization>(), retryIfUnauthorized)
            );
            return services;
        }
    }
}
