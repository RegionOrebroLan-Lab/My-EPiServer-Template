using System.Web;
using EPiServer.ServiceLocation;

namespace MyCompany.MyWebApplication.Business.Web
{
	[ServiceConfiguration(typeof(IWebFacade), Lifecycle = ServiceInstanceScope.Singleton)]
	public class WebFacade : IWebFacade
	{
		#region Properties

		public virtual HttpContextBase HttpContext => new HttpContextWrapper(System.Web.HttpContext.Current);

		#endregion
	}
}