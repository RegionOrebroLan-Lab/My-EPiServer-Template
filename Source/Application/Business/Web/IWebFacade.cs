using System.Web;

namespace MyCompany.MyWebApplication.Business.Web
{
	public interface IWebFacade
	{
		#region Properties

		HttpContextBase HttpContext { get; }

		#endregion
	}
}