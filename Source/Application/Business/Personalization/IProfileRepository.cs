using System.Diagnostics.CodeAnalysis;

namespace MyCompany.MyWebApplication.Business.Personalization
{
	public interface IProfileRepository
	{
		#region Methods

		IProfile Create(string userName);

		[SuppressMessage("Microsoft.Naming", "CA1716:Identifiers should not match keywords")]
		IProfile Get(string userName);

		void Save(IProfile profile);

		#endregion
	}
}