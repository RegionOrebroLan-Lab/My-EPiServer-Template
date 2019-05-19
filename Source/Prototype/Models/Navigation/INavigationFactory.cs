using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public interface INavigationFactory
	{
		#region Methods

		INavigationNode Create(IContentNode root, INavigationSettings settings);

		#endregion
	}
}