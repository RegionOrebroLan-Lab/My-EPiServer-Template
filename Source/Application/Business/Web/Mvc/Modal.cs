using System;
using System.Collections.Generic;

namespace MyCompany.MyWebApplication.Business.Web.Mvc
{
	public class Modal : IModal
	{
		#region Properties

		public virtual IDictionary<string, Uri> Actions { get; } = new Dictionary<string, Uri>();
		public virtual string CssClass { get; set; }
		public virtual object Model { get; set; }

		#endregion
	}
}