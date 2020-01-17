using System;
using System.Net.Http;
using Avalara.ApiClient;
using Xunit;

namespace ApiClient.Tests
{
	public class ApiResponseTests
	{
		[Fact]
		public void Create_should_set_data()
		{
			const string stringData = "hello there";
			var response = ApiResponse<string>.Create(stringData, new HttpResponseMessage());
			Assert.Equal(stringData, response.Data);
		}
	}
}
