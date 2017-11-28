using NUnit.Framework;
using PactNet;

namespace Provider.Tests
{
	public class CustomerApiTests
	{
		[Test]
		public void CustomerApiShouldHonourPactWithConsumer()
		{
			// Arrange
			const string serviceUri = "http://localhost:9876";
			var config = new PactVerifierConfig();

			using (NancyTestHost.Start(serviceUri))
			{
				// Act / Assert
				new PactVerifier(config)
					.ProviderState(serviceUri + "/customer-provider-states")
					.ServiceProvider("Customer API", serviceUri)
					.HonoursPactWith("Consumer")
					.PactUri(@"..\..\..\pacts\consumer-customer_api.json")
					.Verify();
			}
		}
	}
}