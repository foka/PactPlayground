using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Consumer
{
	public class CustomerApiClient
	{
		public CustomerApiClient(string baseUri)
		{
			this.baseUri = baseUri;
		}

		public Customer GetCustomerById(int id)
		{
			using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
			using (var request = new HttpRequestMessage(HttpMethod.Get, "/customers/" + id))
			{
				request.Headers.Add("Accept", "application/json");

				using (var response = client.SendAsync(request).Result)
				{
					if (response.StatusCode != HttpStatusCode.OK)
						throw new ApplicationException(response.ReasonPhrase);

					var content = response.Content.ReadAsStringAsync().Result;
					return JsonConvert.DeserializeObject<Customer>(content);
				}
			}
		}

		private readonly string baseUri;
	}
}