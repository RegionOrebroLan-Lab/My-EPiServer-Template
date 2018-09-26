using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Security;
using log4net;
using log4net.Config;
using MyCompany.MyWebApplication.Business.Personalization;
using MyCompany.MyWebApplication.Business.Web.Profile;
using RegionOrebroLan.EPiServer.Framework.Initialization;

namespace MyCompany.MyWebApplication
{
	[SuppressMessage("Microsoft.Naming", "CA1716:Identifiers should not match keywords")]
	public class Global : EPiServer.Global
	{
		#region Fields

		private static bool _firstRequest = true;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static Global()
		{
			// Check if log4net is configured, if not configure it with "Log.config".
			if(!LogManager.GetRepository().Configured)
				XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Log.config"));

			Initializer.Initialize(HostType.WebApplication);
		}

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1707:Identifiers should not contain underscores")]
		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
		}

		[SuppressMessage("Microsoft.Naming", "CA1707:Identifiers should not contain underscores")]
		protected void Application_AuthenticateRequest()
		{
			this.CreateAdministratorIfNecessary();
		}

		protected internal virtual void CreateAdministratorIfNecessary()
		{
			if(!_firstRequest)
				return;

			_firstRequest = false;
			// ReSharper disable All
			if(!this.Request.IsLocal)
				return;

			var userProvider = ServiceLocator.Current.GetInstance<UIUserProvider>();

			userProvider.GetAllUsers(0, 1, out var numberOfUsers);

			if(numberOfUsers > 0)
				return;

			var user = userProvider.CreateUser("Administrator", "P@ssword12", "administrator@company.com", null, null, true, out var status, out var errors);

			if(status == UIUserCreateStatus.Success)
			{
				const string administrationRoleName = "WebAdmins";
				var roleProvider = ServiceLocator.Current.GetInstance<UIRoleProvider>();
				roleProvider.CreateRole(administrationRoleName);
				roleProvider.AddUserToRoles(user.Username, new string[] {administrationRoleName});

				var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
				if(profileManager.Enabled)
				{
					var profileRepository = ServiceLocator.Current.GetInstance<IProfileRepository>();
					var profile = profileRepository.Get(user.Username) ?? profileRepository.Create(user.Username);
					profile.Email = user.Email;
					profileRepository.Save(profile);
				}

				var contentSecurityRepository = ServiceLocator.Current.GetInstance<IContentSecurityRepository>();
				var contentSecurityDescriptor = contentSecurityRepository.Get(ContentReference.RootPage).CreateWritableClone() as IContentSecurityDescriptor;

				contentSecurityDescriptor.AddEntry(new AccessControlEntry(administrationRoleName, AccessLevel.FullAccess));
				contentSecurityRepository.Save(ContentReference.RootPage, contentSecurityDescriptor, SecuritySaveType.Replace);
			}
			else
			{
				throw new InvalidOperationException("Could not create administrator.", errors.Any() ? new InvalidOperationException(errors.First()) : null);
			}
		}

		#endregion
	}
}