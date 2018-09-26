using System.Collections.Specialized;

namespace MyCompany.MyWebApplication.Business.Configuration
{
	public interface IConfigurationManager
	{
		#region Properties

		NameValueCollection ApplicationSettings { get; }

		#endregion

		#region Methods

		object GetSection(string sectionName);
		void RefreshSection(string sectionName);

		#endregion
	}
}