using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[InitializableModule]
	public class MvcInitialization : IInitializableModule
	{
		#region Fields

		//private static readonly IEnumerable<string> _partialViewLocationFormats = new[] {"~/Views/Shared/Blocks/{0}.cshtml", "~/Views/Shared/EditorTemplates/{0}.cshtml"};
		private static readonly IEnumerable<string> _partialViewLocationFormats = new[] {"~/Views/Shared/Blocks/{0}.cshtml"};

		#endregion

		#region Properties

		protected internal virtual IEnumerable<string> PartialViewLocationFormats => _partialViewLocationFormats;

		#endregion

		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			DependencyResolver.SetResolver(new Web.Mvc.DependencyResolver(context.Locate.Advanced));

			var razorViewEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();

			if(razorViewEngine != null)
				razorViewEngine.PartialViewLocationFormats = razorViewEngine.PartialViewLocationFormats.Union(this.PartialViewLocationFormats).ToArray();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}