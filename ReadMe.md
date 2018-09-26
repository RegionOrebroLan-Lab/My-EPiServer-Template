# My EPiServer-template

This is an EPiServer Visual Studio solution to use as a template. The idea is to download it and then do search and replace on some values and use it as a template for new EPiServer Visual Studio solutions. The values to search and replace are **MyCompany-MyWebApplication** and **MyCompany.MyWebApplication**.

These values are in files in the solution so you can use **Replace in Files** (Ctrl+Shift+H) to easily replace them. The values are not in the *.csproj-files but in **Build.props** files imported in the *.csproj-files.

For example in the **Application**-project:
- [**&lt;AssemblyName&gt;MyCompany.MyWebApplication&lt;/AssemblyName&gt;**](/Source/Application/Build/Build.props#L3) is in the [**Build\Build.props**](/Source/Application/Build/Build.props)-file
- [**&lt;RootNamespace&gt;$(AssemblyName)&lt;/RootNamespace&gt;**](/Source/Application/Application.csproj#L18) is in the [**Application.csproj**](/Source/Application/Application.csproj)-file

You also should change the name of the solution file, [**MyCompany-MyWebApplication.sln**](/Source/MyCompany-MyWebApplication.sln).

1. Download the solution, https://github.com/RegionOrebroLan-Lab/My-EPiServer-Template/archive/master.zip.
2. Unzip and copy the content to a directory on your system where you want it.
3. Rename **MyCompany-MyWebApplication.sln** to **YourCompany-YourWebApplication.sln** or whatever you like.
4. Open the solution in Visual Studio.
5. Search for **MyCompany** and replace with **YourCompany** or whatever you like.
6. Search for **MyWebApplication** and replace with **YourWebApplication** or whatever you like, eg. **MyWebApplication** or **Intranet**.
7. Close and reopen the solution (eg. to avoid ReSharper warnings).

The focus for the EPiServer project is initialization and continous-release.
1. **Initialization** - you should be able to start debugging instantly (F5) after cloning from source-control, if there is no database a local one will be created.
2. **Continous-release** - transforms for License.config, Log.config and Web.config.

## IIS-express-url

The IIS-express-url is set to: https://localhost/44377/

To change it:
1. Unload the Application-project
2. Change Project/PropertyGroup/IISExpressSSLPort from **44377** to what you want
3. Change Project/ProjectExtensions/VisualStudio/FlavorProperties/WebProjectProperties/IISUrl from **https://localhost:44377/** to what you want
4. Reload the Application-project

You should also change this value: [baseUri="https://localhost:44377/Services/IndexingService.svc"](/Source/Application/Web.config#L107)