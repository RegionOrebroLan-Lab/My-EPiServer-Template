using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions;
using RegionOrebroLan;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.Data.Extensions;

namespace MyCompany.MyWebApplication.IntegrationTests.Helpers
{
	public static class DatabaseHelper
	{
		#region Fields

		private static readonly IProviderFactories _providerFactories = new DbProviderFactoriesWrapper();
		private static readonly IDatabaseManagerFactory _databaseManagerFactory = new DatabaseManagerFactory(new AppDomainWrapper(AppDomain.CurrentDomain), new ConnectionStringBuilderFactory(_providerFactories), new FileSystem(), _providerFactories);

		#endregion

		#region Methods

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public static void DropDatabasesIfTheyExist()
		{
			foreach(ConnectionStringSettings connectionSetting in ConfigurationManager.ConnectionStrings)
			{
				IDatabaseManager databaseManager;

				try
				{
					databaseManager = _databaseManagerFactory.Create(connectionSetting.ProviderName);
				}
				catch(InvalidOperationException)
				{
					continue;
				}

				var connectionString = ResolveConnectionString(connectionSetting.ConnectionString);
				databaseManager.DropDatabaseIfItExists(connectionString);
			}
		}

		private static string ResolveConnectionString(string connectionString)
		{
			return connectionString?.Replace("|DataDirectory|", Path.Combine(Global.ProjectDirectoryPath, "App_Data\\"));
		}

		#endregion
	}
}