using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace Consumer.Tests
{
	public class CustomerApiClientTests
	{
		[Test]
		public async Task GetCustomerByIdAsync_ReturnsExpectedCustomer()
		{
			// Arrange
			mockProviderService
				.Given("There is a customer with id 123")
				.UponReceiving("A GET request to retrieve the customer")
				.With(new ProviderServiceRequest
				{
					Method = HttpVerb.Get,
					Path = "/customers/123",
					Headers = new Dictionary<string, object> { {"Accept", "application/json"} }
				})
				.WillRespondWith(new ProviderServiceResponse
				{
					Status = 200,
					Headers = new Dictionary<string, object> { {"Content-Type", "application/json; charset=utf-8"} },
					Body = new
					{
						Id = 123,
						FirstName = "Jan",
						LastName = "Kowalski"
					}
				});

			// Act
			var customer = await client.GetCustomerByIdAsync(123);

			// Assert
			customer.ShouldBeEquivalentTo(new Customer
			{
				Id = 123,
				FirstName = "Jan",
				LastName = "Kowalski"
			});
		}

		[Test]
		public async Task GetCustomerByIdAsync_ReturnsNull_ForNotExistingCustomer()
		{
			// Arrange
			mockProviderService
				.Given("There is NO customer with id 124")
				.UponReceiving("A GET request to retrieve the customer")
				.With(new ProviderServiceRequest
				{
					Method = HttpVerb.Get,
					Path = "/customers/124",
					Headers = new Dictionary<string, object> { {"Accept", "application/json"} }
				})
				.WillRespondWith(new ProviderServiceResponse
				{
					Status = 404,
					Headers = new Dictionary<string, object> { {"Content-Type", "application/json; charset=utf-8"} },
					Body = null
				});

			// Act
			var customer = await client.GetCustomerByIdAsync(124);

			// Assert
			customer.Should().BeNull();
		}


		[TestFixtureSetUp]
		public void SetupPact()
		{
			pact = new CustomerApiPact();
			mockProviderService = pact.MockProviderService;
			// NOTE: Clears any previously registered interactions before the test is run
			mockProviderService.ClearInteractions();
			mockProviderServiceBaseUri = pact.MockProviderServiceBaseUri;

			client = new CustomerApiClient(mockProviderServiceBaseUri);
		}

		[TestFixtureTearDown]
		public void SavePactFile()
		{
			pact.SavePactFile();

			// TODO: To ma być krok w CI!
			var pactPublisher = new PactPublisher("http://localhost");
			pactPublisher.PublishToBroker(@"..\..\..\pacts\consumer-customer_api.json", "1.2.125", new [] {"dev"});

		}

		private CustomerApiPact pact;
		private IMockProviderService mockProviderService;
		private string mockProviderServiceBaseUri;
		private CustomerApiClient client;
	}
}