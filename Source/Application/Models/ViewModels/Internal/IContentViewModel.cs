using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IContentViewModel<out T> : IViewModel where T : IContent
	{
		#region Properties

		T Content { get; }

		#endregion
	}
}