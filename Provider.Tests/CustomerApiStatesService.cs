using FakeItEasy;
using Nancy;
using Nancy.ModelBinding;

namespace Provider.Tests
{
	public class CustomerApiStatesService : NancyModule
	{
		public CustomerApiStatesService(ICustomerDao fakeCustomerDao)
			: base("/customer-provider-states")
		{
			this.fakeCustomerDao = fakeCustomerDao;
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
					A.CallTo(() => fakeCustomerDao.GetCustomerById(123))
						.Returns(new DbCustomer
						{
							Id = 123,
							FirstName = "Jan",
							LastName = "Kowalski"
						});
					break;
				case "There is NO customer with id 124":
					A.CallTo(() => fakeCustomerDao.GetCustomerById(124)).Returns(null);
					break;
			}
		}

		private readonly ICustomerDao fakeCustomerDao;

		private class ProviderState
		{
			// {"consumer":"Consumer","state":"There is a customer with id 123","states":["There is a customer with id 123"]}

			public string Consumer { get; set; }
			public string State { get; set; }
		}
	}
}