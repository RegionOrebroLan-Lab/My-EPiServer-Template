using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(typeof(ISettings), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class Settings : ISettings { }
}