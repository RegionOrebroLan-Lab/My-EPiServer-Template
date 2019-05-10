using System.Web.Profile;
using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Business.Web.Profile
{
	[ServiceConfiguration(typeof(IProfileManager), Lifecycle = ServiceInstanceScope.Singleton)]
	public class ProfileManagerWrapper : IProfileManager
	{
		#region Properties

		public virtual bool Enabled => ProfileManager.Enabled;

		#endregion
	}
}