using System;
using Nancy;
using Nancy.Hosting.Self;

namespace Provider.Tests
{
	public class NancyTestHost
	{
		public static NancyHost Start(string serviceUri)
		{
			var hostConfig = new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = true }};
			var nancyHost = new NancyHost(new Uri(serviceUri), new DefaultNancyBootstrapper(), hostConfig);
			nancyHost.Start();
			return nancyHost;
		} 
	}
}