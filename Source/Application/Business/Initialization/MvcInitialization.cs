using System;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[InitializableModule]
	public class MvcInitialization : IInitializableModule
	{
		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			DependencyResolver.SetResolver(new Web.Mvc.DependencyResolver(context.Locate.Advanced));
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}