using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Owin;
using MyCompany.MyWebApplication.Business.Bootstrapping;

[assembly: AssemblyCompany("MyCompany")]
[assembly: AssemblyConfiguration(
#if DEBUG
	"Debug"
#else
	"Release"
#endif
)]
[assembly: AssemblyDescription("MyWebApplication - MyCompany")]
[assembly: AssemblyProduct("MyWebApplication - MyCompany")]
[assembly: AssemblyTitle("MyWebApplication - MyCompany")]
[assembly: AssemblyVersion("0.0.*")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: OwinStartup(typeof(Startup))]