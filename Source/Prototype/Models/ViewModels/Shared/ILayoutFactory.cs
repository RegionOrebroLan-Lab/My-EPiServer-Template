using Prototype.Models.Content;

namespace Prototype.Models.ViewModels.Shared
{
	public interface ILayoutFactory
	{
		#region Methods

		ILayout Create();
		ILayout Create(IContentNode content);

		#endregion
	}
}