using System;
using Nancy;
using Nancy.Hosting.Self;
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
			const string serviceUri = "http://localhost:9222";
			var config = new PactVerifierConfig
			{
				// This allows the user to set a request header that will be sent with every request the verifier sends to the provider:
				// CustomHeader = new KeyValuePair<string, string>("Authorization", "Basic VGVzdA=="),
				// Output verbose verification logs to the test output:
				Verbose = true
			};

			using (StartNancyHost(serviceUri))
			{
				// Act / Assert
				new PactVerifier(config)
					.ServiceProvider("Customer API", serviceUri)
					.HonoursPactWith("Consumer")
					.PactUri(@"..\..\..\pacts\consumer-customer_api.json")
					.Verify();
			}
		}

		private static NancyHost StartNancyHost(string serviceUri)
		{
			var hostConfig = new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = true }};
			var nancyHost = new NancyHost(new Uri(serviceUri), new DefaultNancyBootstrapper(), hostConfig);
			nancyHost.Start();
			return nancyHost;
		}
	}
}