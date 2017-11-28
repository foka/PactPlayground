using System;
using Nancy;
using Nancy.ModelBinding;

namespace Provider.Tests
{
	public class CustomerApiStatesService : NancyModule
	{
		public CustomerApiStatesService() : base("/customer-provider-states")
		{
			Post[""] = o => PostState();
		}

		private dynamic PostState()
		{
			var providerState = this.Bind<ProviderState>();
			SetState(providerState.State);
			return null;
		}

		private void SetState(string state)
		{
			switch (state)
			{
				case "There is a customer with id 123":
					Console.Out.WriteLine("Would insert a customer with id 123...");
					break;
			}
		}

		private class ProviderState
		{
			// {"consumer":"Consumer","state":"There is a customer with id 123","states":["There is a customer with id 123"]}

			public string Consumer { get; set; }
			public string State { get; set; }
		}
	}
}