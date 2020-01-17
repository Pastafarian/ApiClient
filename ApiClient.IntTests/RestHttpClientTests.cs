using System;
using System.Net;
using Avalara.ApiClient;
using Avalara.ApiClient.Authorization.OAuth;
using Avalara.ApiClient.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiClient.IntTests
{
	public class RestHttpClientTests
	{
		private IRestHttpClient Build(IdServerSettings settings = null)
		{
			if (settings == null)
			{
				settings = new IdServerSettings
				{
					IdServerUrl = "http://localhost:5400/ids",
					IdServerClientId = "IntTest",
					IdServerClientSecret = "SecretSquirrel",
					Scope = "MrsWorkflow.Api"
				};
			}

			var serviceCollection = new ServiceCollection();
			serviceCollection.ConfigureForClientCredentials(settings);

			var serviceProvider = serviceCollection.BuildServiceProvider();

			return serviceProvider.GetService<IRestHttpClient>();
		}

		[Fact]
		public async void Get_from_workflow_api()
		{
			var restClient = Build();

			var response = await restClient.GetAsync("http://localhost:5000/api/v3/FilingFrequencies");

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
	}
}
