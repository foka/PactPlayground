using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace Consumer.Tests
{
	public class CustomerApiClientTests
	{
		[Test]
		public void GetCustomerById_ReturnsExpectedCustomer()
		{
			// Arrange
			mockProviderService
				.Given("There is a customer with id 69")
				.UponReceiving("A GET request to retrieve the customer")
				.With(new ProviderServiceRequest
				{
					Method = HttpVerb.Get,
					Path = "/customers/69",
					Headers = new Dictionary<string, object> { {"Accept", "application/json"} }
				})
				.WillRespondWith(new ProviderServiceResponse
				{
					Status = 200,
					Headers = new Dictionary<string, object> { {"Content-Type", "application/json; charset=utf-8"} },
					Body = new
					{
						Id = 69,
						FirstName = "Mateusz",
						LastName = "Trzaskawka"
					}
				});
			var client = new CustomerApiClient(mockProviderServiceBaseUri);

			// Act
			var customer = client.GetCustomerById(69);

			// Assert
			customer.ShouldBeEquivalentTo(new Customer()
			{
				Id = 69,
				FirstName = "Mateusz",
				LastName = "Trzaskawka"
			});
		}

		[SetUp]
		public void SetupPact()
		{
			pact = new CustomerApiPact();
			mockProviderService = pact.MockProviderService;
			// NOTE: Clears any previously registered interactions before the test is run
			mockProviderService.ClearInteractions();
			mockProviderServiceBaseUri = pact.MockProviderServiceBaseUri;
		}

		[TearDown]
		public void SavePactFile()
		{
			pact.SavePactFile();
		}

		private CustomerApiPact pact;
		private IMockProviderService mockProviderService;
		private string mockProviderServiceBaseUri;
	}
}