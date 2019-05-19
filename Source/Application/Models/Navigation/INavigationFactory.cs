using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public interface INavigationFactory
	{
		#region Methods

		INavigationNode Create(ContentReference root, INavigationSettings settings);

		#endregion
	}
}