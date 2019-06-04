using System;
using EPiServer.Forms.Core.Events;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[CLSCompliant(false)]
	[InitializableModule]
	public class FormsInitialization : IInitializableModule
	{
		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var formEvents = context.Locate.Advanced.GetInstance<FormsEvents>();

			formEvents.FormsStepSubmitted += this.OnFormsStepSubmitted;
			formEvents.FormsStructureChange += this.OnFormsStructureChange;
			formEvents.FormsSubmissionFinalized += this.OnFormsSubmissionFinalized;
			formEvents.FormsSubmitting += this.OnFormsSubmitting;
		}

		protected internal virtual void OnFormsStepSubmitted(object sender, FormsEventArgs e) { }
		protected internal virtual void OnFormsStructureChange(object sender, FormsEventArgs e) { }
		protected internal virtual void OnFormsSubmissionFinalized(object sender, FormsEventArgs e) { }
		protected internal virtual void OnFormsSubmitting(object sender, FormsEventArgs e) { }

		public virtual void Uninitialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var formEvents = context.Locate.Advanced.GetInstance<FormsEvents>();

			formEvents.FormsSubmitting -= this.OnFormsSubmitting;
			formEvents.FormsSubmissionFinalized -= this.OnFormsSubmissionFinalized;
			formEvents.FormsStructureChange -= this.OnFormsStructureChange;
			formEvents.FormsStepSubmitted -= this.OnFormsStepSubmitted;
		}

		#endregion
	}
}