﻿using System;
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

			BundleTable.EnableOptimizations = false;

			if(bool.TryParse(context.Locate.Advanced.GetInstance<IConfigurationManager>().ApplicationSettings["EnableBundleOptimizations"], out var enableBundleOptimizations))
				BundleTable.EnableOptimizations = enableBundleOptimizations;

			BundleTable.Bundles.FileExtensionReplacementList.Clear();

			var scriptBundle = new ScriptBundle("~/Site-scripts").Include(BundleTable.EnableOptimizations ? new[] {"~/Scripts/Site.min.js"} : new[] {"~/Scripts/Site.js"});
			scriptBundle.Transforms.Clear();
			BundleTable.Bundles.Add(scriptBundle);

			var styleBundle = new StyleBundle("~/Site-style").Include(BundleTable.EnableOptimizations ? new[] {"~/Style/Site.min.css"} : new[] {"~/Style/Site.css"});
			styleBundle.Transforms.Clear();
			BundleTable.Bundles.Add(styleBundle);
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}