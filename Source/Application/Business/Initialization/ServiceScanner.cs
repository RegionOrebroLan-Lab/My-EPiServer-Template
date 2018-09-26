using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.ServiceLocation;
using RegionOrebroLan.ServiceLocation.Extensions;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[InitializableModule]
	public class ServiceScanner : IConfigurableModule
	{
		#region Constructors

		public ServiceScanner() : this(AppDomain.CurrentDomain.GetAssemblies(), new ServiceConfigurationScanner()) { }

		public ServiceScanner(IEnumerable<Assembly> assemblies, IServiceConfigurationScanner serviceConfigurationScanner)
		{
			this.Assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
			this.ServiceConfigurationScanner = serviceConfigurationScanner ?? throw new ArgumentNullException(nameof(serviceConfigurationScanner));
		}

		#endregion

		#region Properties

		protected internal virtual IEnumerable<Assembly> Assemblies { get; }
		protected internal virtual IServiceConfigurationScanner ServiceConfigurationScanner { get; }

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton(this.ServiceConfigurationScanner);

			foreach(var mapping in this.ServiceConfigurationScanner.Scan(this.Assemblies.Where(this.IncludeAssembly)))
			{
				context.Services.Add(new ServiceDescriptor(mapping.Configuration.ServiceType, mapping.Type, this.GetServiceInstanceScope(mapping)));
			}
		}

		[SuppressMessage("Microsoft.Style", "IDE0010:Add missing cases")]
		protected internal virtual ServiceInstanceScope GetServiceInstanceScope(IServiceConfigurationMapping serviceConfigurationMapping)
		{
			if(serviceConfigurationMapping == null)
				throw new ArgumentNullException(nameof(serviceConfigurationMapping));

			// ReSharper disable SwitchStatementMissingSomeCases
			switch(serviceConfigurationMapping.Configuration.InstanceMode)
			{
				case InstanceMode.Request:
				case InstanceMode.Thread:
					return ServiceInstanceScope.Hybrid;
				case InstanceMode.Singleton:
					return ServiceInstanceScope.Singleton;
				default:
					return ServiceInstanceScope.Transient;
			}
			// ReSharper restore SwitchStatementMissingSomeCases
		}

		protected internal virtual bool IncludeAssembly(Assembly assembly)
		{
			const string name = "MyCompany";
			var assemblyName = assembly?.GetName().Name;

			// ReSharper disable InvertIf
			if(assemblyName != null)
			{
				if(assemblyName.Equals(name, StringComparison.OrdinalIgnoreCase))
					return true;

				if(assemblyName.StartsWith(name + ".", StringComparison.OrdinalIgnoreCase))
					return true;
			}
			// ReSharper restore InvertIf

			return false;
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}