using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models
{
	public interface IMainAreaContent
	{
		#region Properties

		ContentArea MainArea { get; set; }

		#endregion
	}
}