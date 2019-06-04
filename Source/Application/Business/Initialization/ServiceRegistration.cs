using System;
using System.IO.Abstractions;
using EPiServer.Configuration;
using EPiServer.Data.SchemaUpdates;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using MyCompany.MyWebApplication.Business.Configuration;
using MyCompany.MyWebApplication.Business.Data.SchemaUpdates;
using RegionOrebroLan.Web.Security.Captcha;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	/// <summary>
	/// The module-dependency to RegionOrebroLan.EPiServer.Initialization.DataInitialization is important.
	/// The first thing that is done in RegionOrebroLan.EPiServer.Initialization.DataInitialization is that all
	/// registrations for ISchemaUpdater are removed. See:
	/// https://github.com/RegionOrebroLan/EPiServer-Initialization/blob/master/Source/Project/DataInitialization.cs#L30
	/// So our registration context.Services.AddSingleton&lt;ISchemaUpdater, ExtensionsSchemaUpdater&gt;(); must come after.
	/// </summary>
	[InitializableModule]
	[ModuleDependency(typeof(RegionOrebroLan.EPiServer.Initialization.DataInitialization))]
	public class ServiceRegistration : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton(AppDomain.CurrentDomain);
			context.Services.AddSingleton<IFileSystem, FileSystem>();
			context.Services.AddSingleton<IRecaptchaSettings>(serviceLocator =>
			{
				var applicationSettings = serviceLocator.GetInstance<IConfigurationManager>().ApplicationSettings;

				return new RecaptchaSettings
				{
					SecretKey = applicationSettings["Recaptcha-SecretKey"],
					SiteKey = applicationSettings["Recaptcha-SiteKey"]
				};
			});
			context.Services.AddSingleton<ISchemaUpdater, ExtensionsSchemaUpdater>();
			context.Services.AddSingleton(LogManager.LoggerFactory());
			context.Services.AddSingleton(Settings.Instance);
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}