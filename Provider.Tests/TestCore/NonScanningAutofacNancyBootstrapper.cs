using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Nancy;
using Nancy.Bootstrapper;

namespace Provider.Tests.TestCore
{
	public class NonScanningAutofacNancyBootstrapper : AutofacNancyBootstrapper
	{
		public NonScanningAutofacNancyBootstrapper(ILifetimeScope lifetimeScope) : base(lifetimeScope)
		{
			var nancyModuleTypes = GetRegisteredNancyModuleTypes(lifetimeScope.ComponentRegistry);

			modules = nancyModuleTypes.Select(t => new ModuleRegistration(t)).ToArray();
		}

		private static IEnumerable<Type> GetRegisteredNancyModuleTypes(IComponentRegistry componentRegistry)
		{
			return componentRegistry.Registrations.SelectMany(r => r.Services)
				.OfType<IServiceWithType>()
				.Select(swt => swt.ServiceType)
				.Where(t => t.IsAssignableTo<INancyModule>());
		}

		protected override IEnumerable<ModuleRegistration> Modules
		{
			get { return modules; }
		}

		private readonly ModuleRegistration[] modules;
	}
}