using EPiServer.Personalization;
using RegionOrebroLan.Abstractions;

namespace MyCompany.MyWebApplication.Business.Personalization
{
	public class EPiServerProfileWrapper : Wrapper<EPiServerProfile>, IProfile
	{
		#region Constructors

		public EPiServerProfileWrapper(EPiServerProfile epiServerProfile) : base(epiServerProfile, nameof(EPiServerProfile)) { }

		#endregion

		#region Properties

		public virtual string Email
		{
			get => this.WrappedInstance.Email;
			set => this.WrappedInstance.Email = value;
		}

		#endregion

		#region Methods

		#region Implicit operator

		public static implicit operator EPiServerProfileWrapper(EPiServerProfile epiServerProfile)
		{
			return epiServerProfile != null ? new EPiServerProfileWrapper(epiServerProfile) : null;
		}

		#endregion

		public static EPiServerProfileWrapper ToEPiServerProfileWrapper(EPiServerProfile epiServerProfile)
		{
			return epiServerProfile;
		}

		#endregion
	}
}