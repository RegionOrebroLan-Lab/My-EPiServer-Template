using System.Collections.Specialized;
using System.Configuration;
using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Business.Configuration
{
	[ServiceConfiguration(typeof(IConfigurationManager), Lifecycle = ServiceInstanceScope.Singleton)]
	public class ConfigurationManagerWrapper : IConfigurationManager
	{
		#region Properties

		public virtual NameValueCollection ApplicationSettings => ConfigurationManager.AppSettings;

		#endregion

		#region Methods

		public virtual object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}

		public virtual void RefreshSection(string sectionName)
		{
			ConfigurationManager.RefreshSection(sectionName);
		}

		#endregion
	}
}