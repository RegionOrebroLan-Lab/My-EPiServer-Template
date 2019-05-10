using System;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using StructureMap;
using StructureMap.Pipeline;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	[ServiceConfiguration(typeof(IViewModelFactory), Lifecycle = ServiceInstanceScope.Singleton)]
	public class ViewModelFactory : IViewModelFactory
	{
		#region Constructors

		public ViewModelFactory(IContainer structureMap)
		{
			this.StructureMap = structureMap ?? throw new ArgumentNullException(nameof(structureMap));
		}

		#endregion

		#region Properties

		protected internal virtual IContainer StructureMap { get; }

		#endregion

		#region Methods

		public virtual T Create<T>() where T : IViewModel
		{
			var viewModel = this.StructureMap.GetInstance<T>(this.CreateExplicitArguments());

			this.Initialize(viewModel);

			return viewModel;
		}

		public virtual T Create<T, TContent>(TContent content) where T : IContentViewModel<TContent> where TContent : IContent
		{
			var viewModel = this.StructureMap.GetInstance<T>(this.CreateExplicitArguments(content));

			this.Initialize(viewModel);

			return viewModel;
		}

		protected internal virtual ExplicitArguments CreateExplicitArguments()
		{
			var contentRouteHelper = this.StructureMap.GetInstance<IContentRouteHelper>();

			return this.CreateExplicitArguments(contentRouteHelper.Content);
		}

		protected internal virtual ExplicitArguments CreateExplicitArguments(IContent content)
		{
			var explicitArguments = new ExplicitArguments();

			explicitArguments.SetArg("content", content);

			return explicitArguments;
		}

		protected internal virtual void Initialize(IViewModel viewModel)
		{
			viewModel?.Initialize();
		}

		#endregion
	}
}