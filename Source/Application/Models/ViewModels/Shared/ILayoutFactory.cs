using EPiServer.Core;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface ILayoutFactory
	{
		#region Properties

		INavigationSettings MainNavigationSettings { get; }
		INavigationSettings SubNavigationSettings { get; }

		#endregion

		#region Methods

		ILayout Create();
		ILayout Create(IContent content);

		#endregion
	}
}