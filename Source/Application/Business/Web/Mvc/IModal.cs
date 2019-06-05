using System;
using System.Collections.Generic;

namespace MyCompany.MyWebApplication.Business.Web.Mvc
{
	public interface IModal
	{
		#region Properties

		IDictionary<string, Uri> Actions { get; }
		string CssClass { get; set; }
		object Model { get; set; }

		#endregion
	}
}