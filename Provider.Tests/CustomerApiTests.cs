using Autofac;
using NUnit.Framework;
using PactNet;
using Provider.Tests.TestCore;

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

			using (NancyTestHost.Start(serviceUri, GetContainer()))
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

		private static IContainer GetContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<CustomerApi>();
			builder.RegisterType<CustomerApiStatesService>();

			return builder.Build();
		}
	}
}