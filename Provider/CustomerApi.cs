using Nancy;

namespace Provider
{
	public class CustomerApi : NancyModule
	{
		public CustomerApi() : base("customers")
		{
			Get["/{customerId}"] = o => GetCustomerById((int)o.customerId);
		}

		private object GetCustomerById(int customerId)
		{
			return new 
			{
				Id = 123,
				FirstName = "Jan",
				LastName = "Kowalski"
			};
		}
	}
}