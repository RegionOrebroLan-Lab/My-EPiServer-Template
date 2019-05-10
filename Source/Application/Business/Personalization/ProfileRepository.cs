using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.Profile;
using EPiServer.Personalization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.Abstractions;

namespace MyCompany.MyWebApplication.Business.Personalization
{
	[ServiceConfiguration(typeof(IProfileRepository), Lifecycle = ServiceInstanceScope.Singleton)]
	public class ProfileRepository : IProfileRepository
	{
		#region Methods

		public virtual IProfile Create(string userName)
		{
			return (EPiServerProfileWrapper) EPiServerProfile.Wrap(ProfileBase.Create(userName));
		}

		[SuppressMessage("Microsoft.Naming", "CA1716:Identifiers should not match keywords")]
		public virtual IProfile Get(string userName)
		{
			return (EPiServerProfileWrapper) EPiServerProfile.Get(userName);
		}

		public virtual void Save(IProfile profile)
		{
			if(profile == null)
				throw new ArgumentNullException(nameof(profile));

			try
			{
				var epiServerProfile = (EPiServerProfile) ((IWrapper) profile).WrappedInstance;
				epiServerProfile.Save();
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "This implementation only supports profiles of type \"{0}\".", typeof(EPiServerProfileWrapper)), exception);
			}
		}

		#endregion
	}
}