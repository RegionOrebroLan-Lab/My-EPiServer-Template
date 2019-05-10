using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface ILayoutFactory
	{
		#region Methods

		ILayout Create();
		ILayout Create(IContent content);

		#endregion
	}
}