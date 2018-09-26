using System;
using System.Web.Optimization;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using MyCompany.MyWebApplication.Business.Configuration;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[InitializableModule]
	public class BundleInitialization : IInitializableModule
	{
		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			BundleTable.EnableOptimizations = bool.Parse(context.Locate.Advanced.GetInstance<IConfigurationManager>().ApplicationSettings["EnableBundleOptimizations"]);
			BundleTable.Bundles.FileExtensionReplacementList.Clear();

			var scriptBundle = new ScriptBundle("~/Site-scripts").Include(BundleTable.EnableOptimizations ? new[] {"~/Scripts/Site.min.js"} : new[] {"~/Scripts/Libraries/jquery.js", "~/Scripts/Libraries/popper.js", "~/Scripts/Libraries/bootstrap.js", "~/Scripts/Main.js"});
			scriptBundle.Transforms.Clear();
			BundleTable.Bundles.Add(scriptBundle);

			var styleBundle = new StyleBundle("~/Site-style").Include(BundleTable.EnableOptimizations ? new[] {"~/Style/Site.min.css"} : new[] {"~/Style/Libraries/bootstrap.css", "~/Style/Main.css"});
			styleBundle.Transforms.Clear();
			BundleTable.Bundles.Add(styleBundle);
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}