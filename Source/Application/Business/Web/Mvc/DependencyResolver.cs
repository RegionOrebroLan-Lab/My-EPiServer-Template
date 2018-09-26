using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Business.Web.Mvc
{
	public class DependencyResolver : IDependencyResolver
	{
		#region Constructors

		public DependencyResolver(IServiceLocator serviceLocator)
		{
			this.ServiceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
		}

		#endregion

		#region Properties

		protected internal virtual IServiceLocator ServiceLocator { get; }

		#endregion

		#region Methods

		protected internal virtual object GetConcreteService(Type serviceType)
		{
			try
			{
				// Can't use TryGetInstance here because it won’t create concrete types.
				return this.ServiceLocator.GetInstance(serviceType);
			}
			catch(ActivationException)
			{
				return null;
			}
		}

		protected internal virtual object GetInterfaceService(Type serviceType)
		{
			return this.ServiceLocator.TryGetExistingInstance(serviceType, out var instance) ? instance : null;
		}

		public virtual object GetService(Type serviceType)
		{
			if(serviceType == null)
				throw new ArgumentNullException(nameof(serviceType));

			return serviceType.IsInterface || serviceType.IsAbstract ? this.GetInterfaceService(serviceType) : this.GetConcreteService(serviceType);
		}

		public virtual IEnumerable<object> GetServices(Type serviceType)
		{
			return this.ServiceLocator.GetAllInstances(serviceType);
		}

		#endregion
	}
}