using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Moq;

namespace MyCompany.MyWebApplication.IntegrationTests.Helpers
{
	[InitializableModule]
	public class TestInitialization : IConfigurableModule
	{
		#region Fields

		private static Action<IServiceConfigurationProvider> _serviceConfiguration;

		#endregion

		#region Properties

		public static Action<IServiceConfigurationProvider> ServiceConfiguration
		{
			get => _serviceConfiguration ?? (_serviceConfiguration = serviceConfiguration => { });
			set => _serviceConfiguration = value;
		}

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton(serviceLocator =>
			{
				var webHostingEnvironmentMock = new Mock<IWebHostingEnvironment>();

				webHostingEnvironmentMock.Setup(webHostingEnvironment => webHostingEnvironment.WebRootPath).Returns(Global.ProjectDirectoryPath);
				webHostingEnvironmentMock.Setup(webHostingEnvironment => webHostingEnvironment.WebRootVirtualPath).Returns("/");

				return webHostingEnvironmentMock.Object;
			});

			context.ConfigurationComplete += this.OnConfigurationComplete;
		}

		public virtual void Initialize(InitializationEngine context) { }

		protected internal virtual void OnConfigurationComplete(object sender, ServiceConfigurationEventArgs e)
		{
			if(e == null)
				throw new ArgumentNullException(nameof(e));

			ServiceConfiguration(e.Services);
		}

		public static void Reset()
		{
			_serviceConfiguration = null;
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}