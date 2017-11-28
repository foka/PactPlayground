using Nancy;

namespace Provider
{
	public class CustomerApi : NancyModule
	{
		public CustomerApi(ICustomerDao customerDao)
			: base("customers")
		{
			Get["/{customerId}"] = o => GetCustomerById((int)o.customerId);

			this.customerDao = customerDao;
		}

		private object GetCustomerById(int customerId)
		{
			var dbCustomer = customerDao.GetCustomerById(customerId);
			if (dbCustomer == null)
				return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
			
			return new
			{
				Id = dbCustomer.Id,
				FirstName = dbCustomer.FirstName,
				LastName = dbCustomer.LastName
			};
		}

		private readonly ICustomerDao customerDao;
	}
}