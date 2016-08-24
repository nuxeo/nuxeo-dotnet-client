# About Nuxeo .NET Client #

[![Build Status](https://qa.nuxeo.org/jenkins/buildStatus/icon?job=master/nuxeo-dotnet-client-master-windows)](https://qa.nuxeo.org/jenkins/job/master/job/nuxeo-dotnet-client-master-windows/)

The Nuxeo .NET Client is a cross-platform client library developed in C# for the Nuxeo Automation and REST API.

Nuxeo .NET Client targets two platforms: `net45` and `dnxcore50`. This allows it to be used to develop apps for not only Windows but also Linux and Mac OS. Bellow follows an app/platform compatibility table for the Nuxeo .NET Client library:

```
+------------------------------------+------------+
|                                    | Compatible |
+------------------------------------+------------+
| Destkop .NET 4.5+                  | Yes        |
| ASP .NET 4.5+                      | Yes        |
| ASP .NET 5 (Native & CoreCLR)      | Yes        |
| DNX Console App (Native & CoreCLR) | Yes        |
| Windows 8/8.1 Store APP            | No         |
| Windows Phone 8/8.1                | No         |
| Universal Windows Platform (UWP)   | No         |
| Xamarin.Android                    | Yes?       |
| Xamarin.iOS                        | Yes?       |
+------------------------------------+------------+
```

# Building #

If you are developing for `net45`, build the NuxeoClient_net45 solution on Visual Studio 2013 (or above).

If you are developing for `dnxcore50`, run:

```
cd src/NuxeoClient
dnu build
```

## Requirements ##

In order to build Nuxeo .NET Client, you must have installed the .NET Core DNX SDK, version 1.0.0-rc1-update1, or .NET 4.5 on a Windows setup.

### Developing for net45 ###

* Download and install [.NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653)

* Visual Studio 2013 (or above) required.

### Developing for dnxcore50 ###

1. Install .NET Core:

    * [Linux instructions](http://dotnet.readthedocs.org/en/latest/getting-started/installing-core-linux.html)

    * [Mac OS instructions](https://docs.asp.net/en/latest/getting-started/installing-on-mac.html)

    * [Windows instructions](http://dotnet.readthedocs.org/en/latest/getting-started/installing-core-windows.html)

2. Add the following feeds to NuGet config file:

    * https://www.myget.org/F/aspnetvnext/

    * https://www.nuget.org/api/v2/

    If you do not yet have a config file, create it on:

    * For Mac OS & Linux: *~/.config/NuGet/NuGet.config*

    * For Windows: *%AppData%/NuGet/NuGet.Config*

    And paste the following contents:

```XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="AspNetVNext" value="https://www.myget.org/F/aspnetvnext/" />
    <add key="NuGet" value="https://www.nuget.org/api/v2/" />
    <add key="DotNet-Core" value="https://dotnet.myget.org/F/dotnet-core/api/v3/index.json" />
  </packageSources>
</configuration>
```

#### On Visual Studio 2015 ####

If you are using Visual Studio 2015 on Windows, you will need to download the [Microsoft ASP.NET and Web Tools 2015 (RC) – Visual Studio](https://www.microsoft.com/en-us/download/details.aspx?id=49959). Two setup files must be installed:

* *DotNetVersionManager-x64.msi*

* *WebToolsExtensionsVS14.msi* for Visual Studio 2015 or *WebToolsExtensionsVWD14.msi* for Visual Studio 2015 Express

## Running the TCK ##

This project includes a Test Compatibility Kit (TCK) covering several tests on the client. In order for the tests to run, there must be an instance of the Nuxeo Server running, loading *nuxeo-automation-test-7.10-SNAPSHOT.jar*.

Before building and running the tests, the server's IP address should be updated in the *test/TCK/Config.cs* file.

### On Windows ###

1. Build the version of the TCK you want to test, by either running `tools\BuildDnx.ps1` or  `tools\BuildNet45.ps1`.

2. Run the TCK with `tools\TestDnx.ps1` or `tools\TestNet45.ps1`

### On Mac OS or Linux ###

On the solution folder, run:

1. `dnu restore` to download all dependencies

2. `cd test/TCK` to move to the TCK folder

3. `dnx test` to run all tests

## Limitations ##

There is currently a bug where DELETE requests are sent as GET on Linux and Mac OS only, which prevents operations such as deleting documents and drop batches from working. This issue was reported [here](https://github.com/dotnet/corefx/issues/4134). **This bug causes 5 out of 9 tests of the TCK to fail**, on the last stage where all temporary files are deleted.
.

## QA

[![Build Status](https://qa.nuxeo.org/jenkins/buildStatus/icon?job=nuxeo-dotnet-client-master-windows)](https://qa.nuxeo.org/jenkins/job/nuxeo-dotnet-client-master-windows/)

# Usage #

## Instructions ##

### 1. Include NuxeoClient in your project ###

On Visual Studio, run on the Package Manager Console: `PM> Install-Package NuxeoClient -Pre`

If you are developing for dnxcore50, include NuxeoClient in your project.json file:

```json
{
  ...,
  "dependencies": {
      ...,
      "NuxeoClient": "1.0.0-*"
  },
  ...
}
```

... and run `dnu restore`.

### 2. Reference it in the code ###

Reference *NuxeoClient* and *NuxeoClient.Wrappers* namespaces:

```csharp
using NuxeoClient;
using NuxeoClient.Wrappers;
```

### 3. Now let's create an instance of NuxeoClient and use it! ###

Create a new instance of NuxeoClient:

```csharp
Client client = new Client(); // assuming server running on localhost:8080
```

Create a folder named "Test Folder 1" on the root:

```csharp
client.Operation("Document.Create")
      .SetInput("doc:/")
      .SetParameter("type", "Folder")
      .SetParameter("name", "TestFolder1")
      .SetParameter("properties", new ParamProperties { { "dc:title", "Test Folder 1" } })
      .Execute();
```

Please check the tests on *test/TCK* for more usage examples and the [tutorial](#tutorial) and the end of this document.

A demo app making use of this client is also available [here](https://github.com/nuxeo/nuxeo-dotnet-browser).

## Tutorial ##

Let's make a quick DNX Console App that creates a folder on the server's root and a child document. This quit tutorial assumes that you have an instance of Nuxeo Server running on your local machine.

* Create a new directory for your project named *TestNuxeoApp* and `cd` there

* Create a new *project.json* with dependencies to "System.Console" and "NuxeoClient". It should look something like this:

```json
{
  "version": "1.0.0-*",
  "description": "Nuxeo Test App",
  "authors": [
    "John Doe"
  ],
  "tags": [
    ""
  ],
  "projectUrl": "",
  "licenseUrl": "",
  "dependencies": {
    "System.Console": "4.0.0-beta-23409",
    "NuxeoClient": "1.0.0-*"
  },
  "frameworks": {
    "dnxcore50": {}
  }
}
```

* Inside the project directory, create a new file named *Program.cs*. Make it create a new Nuxeo Client, create a folder in the server's root directory named "My Folder", and then a file inside My Folder named "My File". It should look something like this:

```csharp
using System;
using System.Threading.Tasks;
using NuxeoClient;
using NuxeoClient.Wrappers;

namespace TestNuxeoApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// create a new Nuxeo Client, which is a disposable objects
			using(var client = new Client())
			{
				// let's create an async job that doesn't block
				Task.Run(async () => {
					// perform an async request to create a folder on the root with the title "My Folder"
					Document folder = (Document)await client.Operation("Document.Create")
															.SetInput("doc:/")
															.SetParameter("type", "Folder")
															.SetParameter("name", "MyFolder")
															.SetParameter("properties", new ParamProperties { { "dc:title", "My Folder" } })
															.Execute();

					// print the returned folder object if it is not null			
					Console.WriteLine(folder == null ? "Failed to create folder." : "Created " + folder.Path);

					// perform an async request to create a child file named "My File"						
					Document file = (Document)await folder.Post(new Document {
						Type = "File",
						Name = "MyFile",
						Properties = new Properties { { "dc:title", "My File" } }
					});

					// print the returned file object if it is not null					   
					Console.WriteLine(file == null ? "Failed to create file." : "Created " + file.Path);
				}).Wait(); // let's wait for the task to complete
			}
		}
	}
}
```

* Restore dependencies by running `dnu restore`

* Run the app by running `dnx run`

* Check the server and verify that the documents were created

# Resources

## Documentation

- http://nuxeo.github.io/nuxeo-dotnet-client/

## Reporting issues

- https://jira.nuxeo.com/browse/NXP/component/15301/
- https://jira.nuxeo.com/secure/CreateIssue!default.jspa?project=NXP

# Licensing

[GNU Lesser General Public License (LGPL) v2.1](http://www.gnu.org/licenses/lgpl-2.1.html)

# About Nuxeo

Nuxeo dramatically improves how content-based applications are built, managed and deployed, making customers more agile, innovative and successful. Nuxeo provides a next generation, enterprise ready platform for building traditional and cutting-edge content oriented applications. Combining a powerful application development environment with
SaaS-based tools and a modular architecture, the Nuxeo Platform and Products provide clear business value to some of the most recognizable brands including Verizon, Electronic Arts, Sharp, FICO, the U.S. Navy, and Boeing. Nuxeo is headquartered in New York and Paris.
More information is available at [www.nuxeo.com](http://www.nuxeo.com).
