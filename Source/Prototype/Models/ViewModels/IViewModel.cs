using System;
using Prototype.Models.ViewModels.Shared;

namespace Prototype.Models.ViewModels
{
	public interface IViewModel
	{
		#region Properties

		string Heading { get; }
		string HtmlContent { get; }
		string Introduction { get; }
		ILayout Layout { get; }
		string Name { get; }
		Uri Url { get; }

		#endregion
	}
}