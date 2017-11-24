using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Consumer
{
	public class CustomerApiClient
	{
		public CustomerApiClient(string baseUri)
		{
			this.baseUri = baseUri;
		}

		public async Task<Customer> GetCustomerByIdAsync(int id)
		{
			using (var request = new HttpRequestMessage(HttpMethod.Get, "/customers/" + id))
			{
				request.Headers.Add("Accept", "application/json");

				using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
				using (var response = await client.SendAsync(request))
				{
					if (response.StatusCode != HttpStatusCode.OK)
						throw new ApplicationException(response.ReasonPhrase);

					var content = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<Customer>(content);
				}
			}
		}

		private readonly string baseUri;
	}
}