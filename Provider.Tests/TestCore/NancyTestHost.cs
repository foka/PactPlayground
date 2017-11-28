using System;
using Autofac;
using Nancy.Hosting.Self;

namespace Provider.Tests.TestCore
{
	public class NancyTestHost
	{
		public static NancyHost Start(string serviceUri, ILifetimeScope lifetimeScope)
		{
			var hostConfig = new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = true }};
			var nancyHost = new NancyHost(new Uri(serviceUri), new NonScanningAutofacNancyBootstrapper(lifetimeScope), hostConfig);
			nancyHost.Start();
			return nancyHost;
		} 
	}
}