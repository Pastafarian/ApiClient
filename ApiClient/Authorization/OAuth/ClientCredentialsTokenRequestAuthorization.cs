using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Avalara.ApiClient.Authorization.OAuth
{
    /// <summary>
    /// IRequestAuthorization using OAuth client credentials.
    /// </summary>
    /// <remarks>
    /// Implemented using <see cref="IdentityModel"/>.
    /// </remarks>
    public class ClientCredentialsTokenRequestAuthorization : IRequestAuthorization
    {
        private readonly IdServerSettings _settings;
        private string _token;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        
        /// <summary>
        /// Timestamp of last successul authorization.
        /// </summary>
        private DateTime _lastAuth = DateTime.MinValue;
       
        /// <summary>
        /// Token expiry, default to 5 mins but should get overriden if Token.ExpiresIn is valid 
        /// after first successful authorization.
        /// </summary>
        private TimeSpan _tokenLifeTime = TimeSpan.FromMinutes(5);

        public ClientCredentialsTokenRequestAuthorization(IdServerSettings settings)
        {
            _settings = settings;
        }

        public async Task Authorize(HttpRequestMessage requestMessage)
        {
            var token = _token;
            if (string.IsNullOrEmpty(token) || TokenExpired())
            {
                //We're using a semaphore so this can be hooked up as a singleton and used under heavy load (eg a proxy).
                await _semaphore.WaitAsync();
                try
                {
                    if (string.IsNullOrEmpty(_token))
                    {
                        _token = await AuthenticateAsync();
                        _lastAuth = DateTime.UtcNow;
                    }
                    token = _token;
                }
                finally
                {
                    _semaphore.Release();
                }

            }
            requestMessage.SetBearerToken(token);
        }

        private async Task<string> AuthenticateAsync()
        {
            var discoveryClient  = new DiscoveryClient(_settings.IdServerUrl)
            {
                Policy = { RequireHttps = _settings.RequireHttps }
            };

            var disco = await discoveryClient.GetAsync();

            if (disco.IsError)
            {
                throw new AuthorizationException($"Error getting Discovery from url:{_settings.IdServerUrl}. Error Is:{disco.Error}");
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, _settings.IdServerClientId, _settings.IdServerClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(_settings.Scope);
          
            if (tokenResponse.IsError)
            {
                throw new AuthorizationException($"Error getting token. Error Is: {tokenResponse.Error}");
            }

            if (tokenResponse.ExpiresIn > 0)
            {
                //Subtract 10s so we're not waiting until the token has actually expired.
                _tokenLifeTime = TimeSpan.FromSeconds(tokenResponse.ExpiresIn > 10 ? tokenResponse.ExpiresIn - 10 : tokenResponse.ExpiresIn);
            }

            return tokenResponse.AccessToken;
        }

        private bool TokenExpired()
        {
            return (DateTime.UtcNow - _lastAuth) > _tokenLifeTime;
        }


        public async Task Expire()
        {
            await _semaphore.WaitAsync();
            try
            {
                _token = null;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}