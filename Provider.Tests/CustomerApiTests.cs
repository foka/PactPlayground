using Autofac;
using FakeItEasy;
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
			var config = new PactVerifierConfig
			{
				ProviderVersion = "1.69",
//				var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
				PublishVerificationResults = true
			};

			using (NancyTestHost.Start(serviceUri, GetLifetimeScope()))
			{
				// Act / Assert
				new PactVerifier(config)
					.ProviderState(serviceUri + "/customer-provider-states")
					.ServiceProvider("Customer API", serviceUri)
					.HonoursPactWith("Consumer")
					.PactUri("http://localhost/pacts/provider/Customer%20API/consumer/Consumer/latest")
					.Verify();
			}
		}

		private static ILifetimeScope GetLifetimeScope()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<CustomerApi>();
			builder.RegisterType<CustomerApiStatesService>();
			builder.RegisterInstance(A.Fake<ICustomerDao>()).SingleInstance();

			return builder.Build();
		}
	}
}