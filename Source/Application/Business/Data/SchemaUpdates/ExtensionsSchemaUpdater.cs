using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using EPiServer.Data;
using EPiServer.Data.SchemaUpdates;
using RegionOrebroLan;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.EPiServer.Data.SchemaUpdates;
using RegionOrebroLan.Extensions;

namespace MyCompany.MyWebApplication.Business.Data.SchemaUpdates
{
	public class ExtensionsSchemaUpdater : BasicSchemaUpdater, ISchemaUpdater
	{
		#region Fields

		private const string _name = "Extensions";
		private static readonly string _scriptFileName = typeof(ExtensionsSchemaUpdater).Assembly.GetName().Name + ".Database.sql";

		#endregion

		#region Constructors

		public ExtensionsSchemaUpdater(IApplicationDomain applicationDomain, IFileSystem fileSystem, IProviderFactories providerFactories) : base(providerFactories)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual Version EmptyVersion { get; } = new Version(0, 0);
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual string Name => _name;
		protected internal virtual Version RequiredDatabaseVersion { get; } = new Version(1, 0);
		protected internal virtual string ScriptFileName => _scriptFileName;

		#endregion

		#region Methods

		protected internal virtual Version GetDatabaseVersion(ConnectionStringOptions connectionStringOption)
		{
			if(connectionStringOption == null)
				throw new ArgumentNullException(nameof(connectionStringOption));

			var databaseProviderFactory = this.ProviderFactories.Get(connectionStringOption.ProviderName);

			using(var connection = databaseProviderFactory.CreateConnection())
			{
				// ReSharper disable PossibleNullReferenceException
				connection.ConnectionString = connectionStringOption.ConnectionString;
				// ReSharper restore PossibleNullReferenceException
				connection.Open();

				using(var command = connection.CreateCommand())
				{
					command.CommandText = "SELECT COUNT(*) FROM [Test];";
					command.CommandType = CommandType.Text;

					try
					{
						command.ExecuteNonQuery();
						return this.RequiredDatabaseVersion;
					}
					catch(DbException)
					{
						return this.EmptyVersion;
					}
				}
			}
		}

		public virtual SchemaStatus GetStatus(IEnumerable<ConnectionStringOptions> connectionStringOptions)
		{
			var connectionStringOption = connectionStringOptions.FirstOrDefault(item => string.Equals(item.Name, this.Name, StringComparison.OrdinalIgnoreCase));

			if(connectionStringOption == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There is no connection-string with name \"{0}\".", this.Name));

			var schemaStatus = new SchemaStatus
			{
				ApplicationRequiredVersion = this.RequiredDatabaseVersion,
				ConnectionStringOption = connectionStringOption,
				DatabaseVersion = this.GetDatabaseVersion(connectionStringOption),
				DisplayName = this.Name
			};

			return schemaStatus;
		}

		public virtual void Update(ConnectionStringOptions connectionStringOptions)
		{
			if(connectionStringOptions == null)
				throw new ArgumentNullException(nameof(connectionStringOptions));

			var dataDirectoryPath = this.ApplicationDomain.GetDataDirectoryPath();

			var scriptPath = this.FileSystem.Path.Combine(dataDirectoryPath, this.ScriptFileName);

			if(this.FileSystem.File.Exists(scriptPath))
			{
				// We can not run the script containing ":setvar", SQLCMD has to be enabled for that. Havent found a way to do it.
				// So we have to split the script on some value and then use the second part.
				const string scriptDelimiter = "USE [$(DatabaseName)];";
				var scriptContent = this.FileSystem.File.ReadAllText(scriptPath);

				var parts = scriptContent.Split(new[] {scriptDelimiter}, StringSplitOptions.None);

				scriptContent = parts[1].Trim().TrimStart("GO".ToCharArray()).Trim();

				this.ExecuteScript(connectionStringOptions, scriptContent);
			}
			else
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The file \"{0}\" does not exist.", scriptPath));
			}
		}

		#endregion
	}
}