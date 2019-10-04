using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation.AutoDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.MyWebApplication.IntegrationTests.Helpers;

namespace MyCompany.MyWebApplication.IntegrationTests
{
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		public static readonly InitializationEngine InitializationEngine = new InitializationEngine((IServiceLocatorFactory) null, HostType.WebApplication, InitializationModule.Assemblies.AllowedAssemblies);

		// ReSharper disable PossibleNullReferenceException
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		// ReSharper restore PossibleNullReferenceException

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static void Cleanup()
		{
			if(InitializationEngine.InitializationState == InitializationState.Initialized)
				InitializationEngine.Uninitialize();

			DatabaseHelper.DropDatabasesIfTheyExist();
		}

		[AssemblyInitialize]
		[SuppressMessage("Usage", "CA1801:Review unused parameters")]
		public static void Initialize(TestContext testContext)
		{
			if(testContext == null)
				throw new ArgumentNullException(nameof(testContext));

			DatabaseHelper.DropDatabasesIfTheyExist();

			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("sv");

			InitializationEngine.Initialize();
		}

		#endregion
	}
}