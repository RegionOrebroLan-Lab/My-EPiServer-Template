using System;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface IRecaptcha
	{
		#region Properties

		/// <summary>
		/// Even if set to true it may return false because Recaptcha can be disabled in configuration.
		/// </summary>
		bool Enabled { get; set; }

		Uri ScriptUrl { get; }
		string SiteKey { get; }
		string TokenParameterName { get; }

		#endregion
	}
}