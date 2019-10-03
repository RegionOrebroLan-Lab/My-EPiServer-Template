using EPiServer.Core;
using RegionOrebroLan.EPiServer.Collections;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface ILayoutFactory
	{
		#region Properties

		ITreeSettings MainNavigationSettings { get; }
		ITreeSettings SubNavigationSettings { get; }

		#endregion

		#region Methods

		ILayout Create();
		ILayout Create(IContent content);

		#endregion
	}
}