using System;
using System.Collections.Generic;
using System.Globalization;
using MyCompany.MyWebApplication.Models.ViewModels.Blocks.Internal;

namespace MyCompany.MyWebApplication.Models.ViewModels.Blocks
{
	public interface IPreviewViewModel : IMainAreaContent, IRightAreaContent
	{
		#region Properties

		CultureInfo Culture { get; }
		bool IncludeRightArea { get; }
		bool IncludeSubNavigation { get; }
		PreviewMode Mode { get; }
		IDictionary<PreviewMode, Uri> Modes { get; }

		#endregion
	}
}