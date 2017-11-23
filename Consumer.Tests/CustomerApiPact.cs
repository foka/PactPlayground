using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Tests
{
	public class CustomerApiPact
	{
		public CustomerApiPact()
		{
			// Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
			PactBuilder = new PactBuilder(new PactConfig
			{
				SpecificationVersion = "2.0.0",
				PactDir = @"..\..\..\pacts",
				LogDir = @"..\..\..\pact-logs"
			});

			PactBuilder
				.ServiceConsumer("Consumer")
				.HasPactWith("Customer API");

			// Configure the http mock server
			MockProviderService = PactBuilder.MockService(MockServerPort);
			// or
			// By passing true as the second param, you can enabled SSL. A self signed SSL cert will be provisioned by default.
			// MockProviderService = PactBuilder.MockService(MockServerPort, true);
			// or
			// You can also change the default Json serialization settings using this overload    
			// MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings());
			// or
			// By passing host as IPAddress.Any, the mock provider service will bind and listen on all ip addresses
			// MockProviderService = PactBuilder.MockService(MockServerPort, host: IPAddress.Any);
		}

		public IPactBuilder PactBuilder { get; private set; }
		public IMockProviderService MockProviderService { get; private set; }
		public int MockServerPort { get { return 9222; } }
		public string MockProviderServiceBaseUri { get { return "http://localhost:" + MockServerPort; } }

		public void SavePactFile()
		{
			PactBuilder.Build();
		}
	}
}