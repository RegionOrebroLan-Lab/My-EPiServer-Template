using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCompany.MyWebApplication.IntegrationTests
{
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		// ReSharper disable PossibleNullReferenceException
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		// ReSharper restore PossibleNullReferenceException

		#endregion

		#region Methods

		[AssemblyInitialize]
		[SuppressMessage("Usage", "CA1801:Review unused parameters")]
		public static void AssemblyInitialize(TestContext testContext) { }

		public static string GetDirectoryPath(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if(type.Assembly != typeof(Global).Assembly)
				throw new InvalidOperationException("It is not possible to get the directory-path for a type outside this assembly.");

			var @namespace = type.Namespace ?? string.Empty;
			var assemblyName = type.Assembly.GetName().Name;

			if(!@namespace.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("The namespace must start with the assembly-name.");

			var relativePath = @namespace.Substring(assemblyName.Length).TrimStart('.').Replace(".", @"\");

			return Path.Combine(ProjectDirectoryPath, relativePath);
		}

		#endregion
	}
}