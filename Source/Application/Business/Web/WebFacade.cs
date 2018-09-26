using System.Web;

namespace MyCompany.MyWebApplication.Business.Web
{
	public class WebFacade : IWebFacade
	{
		#region Properties

		public virtual HttpContextBase HttpContext => new HttpContextWrapper(System.Web.HttpContext.Current);

		#endregion
	}
}