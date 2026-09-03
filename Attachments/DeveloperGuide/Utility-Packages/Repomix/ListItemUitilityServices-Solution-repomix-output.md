This file is a merged representation of the entire codebase, combined into a single document by Repomix.

# File Summary

## Purpose
This file contains a packed representation of the entire repository's contents.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Files are sorted by Git change count (files with more changes are at the bottom)

# Directory Structure
```
ListItemUitilityServices.slnx
ListItemUitilityServices/IListItemFactory.cs
ListItemUitilityServices/IListItemsFactoryBuilder.cs
ListItemUitilityServices/ListItemFactory.cs
ListItemUitilityServices/ListItemsFactoryBuilder.cs
ListItemUitilityServices/ListItemsType.cs
ListItemUitilityServices/ListItemUitilityServices.csproj
ListItemUitilityServices/ListItemUitilityServices.csproj.lscache
ListItemUitilityServices/Models/ListItemsFactoriesModel.cs
```

# Files

## File: ListItemUitilityServices.slnx
```
<Solution>
  <Project Path="ListItemUitilityServices.Tests/ListItemUitilityServices.Tests.csproj" />
  <Project Path="ListItemUitilityServices/ListItemUitilityServices.csproj" />
</Solution>
```

## File: ListItemUitilityServices/IListItemFactory.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public interface IListItemFactory
    {
        void Configure();
        string CreateListItems(string sep , IEnumerable<string> items , ListItemsType options = ListItemsType.NUMBER);
    }
}
```

## File: ListItemUitilityServices/IListItemsFactoryBuilder.cs
```csharp
using ListItemUitilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public interface IListItemsFactoryBuilder
    {
        ListItemsFactoriesModel ListItemsFactoriesModel { get; }
        void Build();
    }
}
```

## File: ListItemUitilityServices/ListItemFactory.cs
```csharp
using ListItemUitilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public class ListItemFactory(
        IListItemsFactoryBuilder listItemsFactoryBuilder = null
    ) : IListItemFactory
    {
        private readonly IListItemsFactoryBuilder _listItemsFactoryBuilder = listItemsFactoryBuilder ?? new ListItemsFactoryBuilder(null); // 內部實作中預設使用一個沒被設定過的Model

        private ListItemsFactoriesModel _listItemsFactoriesModel => _listItemsFactoryBuilder.ListItemsFactoriesModel;
        public void Configure()
        {
            _listItemsFactoryBuilder.Build();
        }

        public string CreateListItems(string sep , IEnumerable<string> items , ListItemsType options = ListItemsType.NUMBER)
        {
            string content = string.Empty;
            switch(options)
            {
                case ListItemsType.ALPHABET:
                    content = InternalCreateListItems_Alphabet(sep , items);
                    break;
                case ListItemsType.NUMBER:
                    content = InternalCreateListItems_Number(sep , items);
                    break;
                default:
                    throw new ArgumentException($"Invalid argument ListItemsType with value {options}");
            }

            return content;
        }
        private string InternalCreateListItems_Alphabet(string sep , IEnumerable<string> items)
        {
            var startPoint = 'A';
            var endPoint = (char)(startPoint + items.Count());
            var bulletinItems = _listItemsFactoriesModel.StringUtilityServices.RangeFrom(startPoint , endPoint).ToList();

            var menuItems = items.ToList();
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < bulletinItems.Count(); i++)
            {
                stringBuilder.Append(bulletinItems [ i ]).Append(sep).Append(menuItems [ i ]);
            }
            return stringBuilder.ToString();
        }
        private string InternalCreateListItems_Number(string sep , IEnumerable<string> items)
        {
            var startPoint = 1;
            var endPoint = startPoint + items.Count();
            var bulletinItems = _listItemsFactoriesModel.MathUtilityServices.RangeFrom(startPoint , endPoint).ToList();

            var menuItems = items.ToList();
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < bulletinItems.Count(); i++)
            {
                stringBuilder.Append(bulletinItems [ i ]).Append(sep).Append(menuItems [ i ]);
            }
            return stringBuilder.ToString();
        }
    }
}
```

## File: ListItemUitilityServices/ListItemsFactoryBuilder.cs
```csharp
using ListItemUitilityServices.Models;
using MathUtilityServices;
using SortingUtilityServices;
using StringUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public class ListItemsFactoryBuilder(
        ListItemsFactoriesModel listItemsFactoriesModel = null
    ) : IListItemsFactoryBuilder
    {
        private readonly ListItemsFactoriesModel _listItemsFactoriesModel = listItemsFactoriesModel ?? new ListItemsFactoriesModel(); // 存取預設實體化一個沒被設定過的Model (若為null)
        public ListItemsFactoriesModel ListItemsFactoriesModel => _listItemsFactoriesModel;
        public void Build()
        {
            _listItemsFactoriesModel.SortingUtilityService = _listItemsFactoriesModel.SortingUtilityService ?? new SortingUtilityService(); // 預設使用  SortingUtilityService 類別

            _listItemsFactoriesModel.StringUtilityServices = _listItemsFactoriesModel.StringUtilityServices ?? new StringUtilityService(_listItemsFactoriesModel.SortingUtilityService); // 預設使用  StringUtilityServices 類別

            _listItemsFactoriesModel.MathUtilityServices = _listItemsFactoriesModel.MathUtilityServices ??
                new MathUtilityService(_listItemsFactoriesModel.SortingUtilityService); // 預設使用 MathUtilityServices 類別
        }
    }
}
```

## File: ListItemUitilityServices/ListItemsType.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ListItemUitilityServices
{
    public enum ListItemsType
    {
        /// <summary>
        /// When create a menu, use alphabets as symbol
        /// </summary>
        [Description("alphabetic items")]
        ALPHABET = 0,

        /// <summary>
        /// When create a menu, use numbers as symbol
        /// </summary>
        [Description("numbered items")]
        NUMBER = 1,
    }
}
```

## File: ListItemUitilityServices/ListItemUitilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>17.0</MinMSBuildReq>
      <MinNetSdkReq>8.0</MinNetSdkReq>
      <TargetFrameworks>net10.0;net9.0;net8.0;</TargetFrameworks>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
      <Version>2.0.0</Version>
      <PackageTags>$(PackageTags);list items</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MathUtilityServices"/>
    <PackageReference Include="SortingUtilityServices"/>
    <PackageReference Include="StringUtilityServices"/>
  </ItemGroup>

</Project>
```

## File: ListItemUitilityServices/ListItemUitilityServices.csproj.lscache
```
version=1

# This file caches language service data to improve the performance of C# Dev Kit.
# It is not intended for manual editing. It can safely be deleted and will be
# regenerated automatically. For more information, see https://aka.ms/lscache
#
# To control where cache files are stored, use the following VS Code setting:
#   "dotnet.projectsystem.cacheInProjectFolder": true

[project]
language=C#
primary
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0

[properties]
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/ListItemUitilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net10.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0\ListItemUitilityServices.dll
/refout:obj\Debug\net10.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

[metadataReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
 Microsoft.CSharp.dll
 Microsoft.VisualBasic.Core.dll
 Microsoft.VisualBasic.dll
 Microsoft.Win32.Primitives.dll
 Microsoft.Win32.Registry.dll
 mscorlib.dll
 netstandard.dll
 System.AppContext.dll
 System.Buffers.dll
 System.Collections.Concurrent.dll
 System.Collections.dll
 System.Collections.Immutable.dll
 System.Collections.NonGeneric.dll
 System.Collections.Specialized.dll
 System.ComponentModel.Annotations.dll
 System.ComponentModel.DataAnnotations.dll
 System.ComponentModel.dll
 System.ComponentModel.EventBasedAsync.dll
 System.ComponentModel.Primitives.dll
 System.ComponentModel.TypeConverter.dll
 System.Configuration.dll
 System.Console.dll
 System.Core.dll
 System.Data.Common.dll
 System.Data.DataSetExtensions.dll
 System.Data.dll
 System.Diagnostics.Contracts.dll
 System.Diagnostics.Debug.dll
 System.Diagnostics.DiagnosticSource.dll
 System.Diagnostics.FileVersionInfo.dll
 System.Diagnostics.Process.dll
 System.Diagnostics.StackTrace.dll
 System.Diagnostics.TextWriterTraceListener.dll
 System.Diagnostics.Tools.dll
 System.Diagnostics.TraceSource.dll
 System.Diagnostics.Tracing.dll
 System.dll
 System.Drawing.dll
 System.Drawing.Primitives.dll
 System.Dynamic.Runtime.dll
 System.Formats.Asn1.dll
 System.Formats.Tar.dll
 System.Globalization.Calendars.dll
 System.Globalization.dll
 System.Globalization.Extensions.dll
 System.IO.Compression.Brotli.dll
 System.IO.Compression.dll
 System.IO.Compression.FileSystem.dll
 System.IO.Compression.ZipFile.dll
 System.IO.dll
 System.IO.FileSystem.AccessControl.dll
 System.IO.FileSystem.dll
 System.IO.FileSystem.DriveInfo.dll
 System.IO.FileSystem.Primitives.dll
 System.IO.FileSystem.Watcher.dll
 System.IO.IsolatedStorage.dll
 System.IO.MemoryMappedFiles.dll
 System.IO.Pipelines.dll
 System.IO.Pipes.AccessControl.dll
 System.IO.Pipes.dll
 System.IO.UnmanagedMemoryStream.dll
 System.Linq.AsyncEnumerable.dll
 System.Linq.dll
 System.Linq.Expressions.dll
 System.Linq.Parallel.dll
 System.Linq.Queryable.dll
 System.Memory.dll
 System.Net.dll
 System.Net.Http.dll
 System.Net.Http.Json.dll
 System.Net.HttpListener.dll
 System.Net.Mail.dll
 System.Net.NameResolution.dll
 System.Net.NetworkInformation.dll
 System.Net.Ping.dll
 System.Net.Primitives.dll
 System.Net.Quic.dll
 System.Net.Requests.dll
 System.Net.Security.dll
 System.Net.ServerSentEvents.dll
 System.Net.ServicePoint.dll
 System.Net.Sockets.dll
 System.Net.WebClient.dll
 System.Net.WebHeaderCollection.dll
 System.Net.WebProxy.dll
 System.Net.WebSockets.Client.dll
 System.Net.WebSockets.dll
 System.Numerics.dll
 System.Numerics.Vectors.dll
 System.ObjectModel.dll
 System.Reflection.DispatchProxy.dll
 System.Reflection.dll
 System.Reflection.Emit.dll
 System.Reflection.Emit.ILGeneration.dll
 System.Reflection.Emit.Lightweight.dll
 System.Reflection.Extensions.dll
 System.Reflection.Metadata.dll
 System.Reflection.Primitives.dll
 System.Reflection.TypeExtensions.dll
 System.Resources.Reader.dll
 System.Resources.ResourceManager.dll
 System.Resources.Writer.dll
 System.Runtime.CompilerServices.Unsafe.dll
 System.Runtime.CompilerServices.VisualC.dll
 System.Runtime.dll
 System.Runtime.Extensions.dll
 System.Runtime.Handles.dll
 System.Runtime.InteropServices.dll
 System.Runtime.InteropServices.JavaScript.dll
 System.Runtime.InteropServices.RuntimeInformation.dll
 System.Runtime.Intrinsics.dll
 System.Runtime.Loader.dll
 System.Runtime.Numerics.dll
 System.Runtime.Serialization.dll
 System.Runtime.Serialization.Formatters.dll
 System.Runtime.Serialization.Json.dll
 System.Runtime.Serialization.Primitives.dll
 System.Runtime.Serialization.Xml.dll
 System.Security.AccessControl.dll
 System.Security.Claims.dll
 System.Security.Cryptography.Algorithms.dll
 System.Security.Cryptography.Cng.dll
 System.Security.Cryptography.Csp.dll
 System.Security.Cryptography.dll
 System.Security.Cryptography.Encoding.dll
 System.Security.Cryptography.OpenSsl.dll
 System.Security.Cryptography.Primitives.dll
 System.Security.Cryptography.X509Certificates.dll
 System.Security.dll
 System.Security.Principal.dll
 System.Security.Principal.Windows.dll
 System.Security.SecureString.dll
 System.ServiceModel.Web.dll
 System.ServiceProcess.dll
 System.Text.Encoding.CodePages.dll
 System.Text.Encoding.dll
 System.Text.Encoding.Extensions.dll
 System.Text.Encodings.Web.dll
 System.Text.Json.dll
 System.Text.RegularExpressions.dll
 System.Threading.AccessControl.dll
 System.Threading.Channels.dll
 System.Threading.dll
 System.Threading.Overlapped.dll
 System.Threading.Tasks.Dataflow.dll
 System.Threading.Tasks.dll
 System.Threading.Tasks.Extensions.dll
 System.Threading.Tasks.Parallel.dll
 System.Threading.Thread.dll
 System.Threading.ThreadPool.dll
 System.Threading.Timer.dll
 System.Transactions.dll
 System.Transactions.Local.dll
 System.ValueTuple.dll
 System.Web.dll
 System.Web.HttpUtility.dll
 System.Windows.dll
 System.Xml.dll
 System.Xml.Linq.dll
 System.Xml.ReaderWriter.dll
 System.Xml.Serialization.dll
 System.Xml.XDocument.dll
 System.Xml.XmlDocument.dll
 System.Xml.XmlSerializer.dll
 System.Xml.XPath.dll
 System.Xml.XPath.XDocument.dll
 WindowsBase.dll

[analyzerReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/ListItemUitilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net8.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:8
/define:TRACE;DEBUG;NET;NET8_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net8.0\ListItemUitilityServices.dll
/refout:obj\Debug\net8.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/ListItemUitilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net9.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:9
/define:TRACE;DEBUG;NET;NET9_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net9.0\ListItemUitilityServices.dll
/refout:obj\Debug\net9.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: ListItemUitilityServices/Models/ListItemsFactoriesModel.cs
```csharp
using MathUtilityServices;
using SortingUtilityServices;
using StringUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices.Models
{
    public class ListItemsFactoriesModel
    {
        public ISortingUtilityService SortingUtilityService { get; set; }
        public IStringUtilityService StringUtilityServices { get; set; }
        public IMathUtilityService MathUtilityServices { get; set; }
    }
}
```
