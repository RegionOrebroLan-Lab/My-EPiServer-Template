using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models
{
	public interface IRightAreaContent
	{
		#region Properties

		ContentArea RightArea { get; set; }

		#endregion
	}
}