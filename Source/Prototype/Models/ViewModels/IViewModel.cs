using System;
using Prototype.Models.ViewModels.Shared;

namespace Prototype.Models.ViewModels
{
	public interface IViewModel
	{
		#region Properties

		string Heading { get; }
		string HtmlContent { get; }
		ILayout Layout { get; }
		string Name { get; }
		Uri Url { get; }

		#endregion
	}
}