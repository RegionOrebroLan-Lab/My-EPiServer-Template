using System.Web.Profile;

namespace MyCompany.MyWebApplication.Business.Web.Profile
{
	public class ProfileManagerWrapper : IProfileManager
	{
		#region Properties

		public virtual bool Enabled => ProfileManager.Enabled;

		#endregion
	}
}