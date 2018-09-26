using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EPiServer.Search.IndexingService;

namespace MyCompany.MyWebApplication.Services
{
	public class IndexingServiceHostFactory : WebServiceHostFactory
	{
		#region Methods

		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			var host = base.CreateServiceHost(serviceType, baseAddresses);

			var binding = new WebHttpBinding("IndexingServiceCustomBinding");
			host.AddServiceEndpoint(typeof(IIndexingService), binding, string.Empty);

			return host;
		}

		#endregion
	}
}