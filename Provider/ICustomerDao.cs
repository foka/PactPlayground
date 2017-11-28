namespace Provider
{
	public interface ICustomerDao
	{
		DbCustomer GetCustomerById(int customerId);
	}
}