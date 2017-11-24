using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Tests
{
	public class CustomerApiPact
	{
		public CustomerApiPact()
		{
			PactBuilder = new PactBuilder(new PactConfig
			{
				SpecificationVersion = "2.0.0",
				PactDir = @"..\..\..\pacts",
				LogDir = @"..\..\..\pact-logs"
			});

			PactBuilder
				.ServiceConsumer("Consumer")
				.HasPactWith("Customer API");

			MockProviderService = PactBuilder.MockService(MockServerPort);
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