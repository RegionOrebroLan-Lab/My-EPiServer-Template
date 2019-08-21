using Prototype.Models.Content;
using Prototype.Models.Navigation;

namespace Prototype.Models.ViewModels.Shared
{
	public interface ILayoutFactory
	{
		#region Properties

		INavigationSettings MainNavigationSettings { get; }
		INavigationSettings SubNavigationSettings { get; }

		#endregion

		#region Methods

		ILayout Create();
		ILayout Create(IContentNode content);

		#endregion
	}
}