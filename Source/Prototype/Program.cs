﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Prototype
{
	public class Program
	{
		#region Methods

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		#endregion
	}
}