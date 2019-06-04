using System;
using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(typeof(ISettings), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class Settings : ISettings
	{
		#region Constructors

		public Settings(IRecaptcha recaptcha)
		{
			this.Recaptcha = recaptcha ?? throw new ArgumentNullException(nameof(recaptcha));
		}

		#endregion

		#region Properties

		public virtual IRecaptcha Recaptcha { get; }

		#endregion
	}
}