# FileStreamUtilityServices-repomix-output.md

```markdown
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
.repomixignore copy
FileStreamUtilityServices.slnx
FileStreamUtilityServices/FileStreamUtilityServices.csproj
FileStreamUtilityServices/FileStreamUtilityServices.csproj.lscache
FileStreamUtilityServices/FileUtility.cs
FileStreamUtilityServices/README.md
```

# Files

## File: .repomixignore copy
```
# Add patterns to ignore here, one per line

### ============================================ ###
### === THESE MATCHING FILES WILL BE IGNORED === ###
### ============================================ ###

### --------------------------------------------------------------------------------------------------------- ###
### --- THESE ENTRIES ARE SPECIFIC (AND IT PROVIDES FOR CONFIGURATION OF SUBCONATINER UNDER THIS CONTAINER) --- ###
### --- AND THUS HARDCODED --- ###
### --- DON'T DELETE IT --- ###
### --- OTHERWISE, YOU WILL PROBABLY FORGOT THE SOURCE TO COPY IT IN THE FUTURE --- ###
### --------------------------------------------------------------------------------------------------------- ###

# templates
[Tt]emplates/

### --------------------------------------------------------------------------------------------------------- ###
### --- END OF COMMENT --- ###
### --------------------------------------------------------------------------------------------------------- ###

### --------------------------------------------------------------------------------------------------------- ###
### --- THESE ENTRIES ARE SPECIFIC (AND IT IS USED FOR PASS THE BUILD (MSBUILD) THAT USES MY CUSTOM CONFIGURATIONS) --- ###
### --- AND THUS HARDCODED --- ###
### --- DON'T DELETE IT --- ###
### --- OTHERWISE, IT MIGHT NOT PASS THE BUILD --- ###
### --------------------------------------------------------------------------------------------------------- ###

# custom folder
## local packages
local-packages

### --------------------------------------------------------------------------------------------------------- ###
### --- END OF COMMENT --- ###
### --------------------------------------------------------------------------------------------------------- ###

# temparory files
[Tt]mp/
[Tt]emp/
*-[Tt]emp

### ... and others (swap files, PID files, session temp files)
*.swp
*.pid
*.lock-s

# lock file
*.lock
*-lock

### ... and others (package manager and dependency locks)
package-lock.json
yarn.lock
pnpm-lock.yaml
paket.lock

# cached
*.cache
*.cached
*-cache
*-cached

### ... and others (temporary workspace cache)
*.suo
*.user
*.userosscache

# cached file about .NET
bin/
obj/

### ... and others (.NET build artifacts and localized outputs)
[Aa]rtifacts/
.[Aa]rtifacts/
_ReSharper*/

# (On Windows) system configuration files
*.ini
*.sys

### ... and others (Windows specific configuration and registry templates)
*.reg
*.inf
*.cfg
*.manifest
Thumbs.db

# CLI script or module
*.bat
*.cmd
*.sh
*.ps1
*.psm1

### ... and others (PowerShell modules and shell environment scripts)
*.psd1
*.pssc
*.bashrc
*.zshrc

# backups
*.bak
*-bak

### ... and others (versioned or historical backups)
*.old
*-old
*.none
*-none
*._bak
~*

# VS plugins
## vsix
*.vsix

# VS plugins backup
.vshistory/

### ... and others (extension local states and component caches)
.vs/
.localhistory/

# log
*.log
*-log

### ... and others (diagnostic and tracing logs)
*.trc
*.etl
*.svclog

# introductions
*.md
*.txt

### ... and others (documentation markup variations)
*.rst
*.adoc

# Nuke
.nuke/

# Cake
### ... omit content
build.cake
tools/

### ... and other batch build tool
Makefile
Rakefile
ant.xml

# git
.git/

### ... and others (Git internal hooks and attributes)
.gitattributes
.gitmodules

## github
.github/
workflows/

### ... and others (GitHub issue templates and pull request templates)
ISSUE_TEMPLATE/
PULL_REQUEST_TEMPLATE/

## gitlab
.gitlab
workflows/

### ... and others (GitLab CI template configs)
.gitlab-ci.yml

# license
LICENSE

### ... and others (legal and copyright notices)
NOTICE
PATENTS

# configuration
*.config
*.json
*.json5

# configuration about Repomix
.repomixignore
*.repomixignore

# configuration about Docker or Docker container
**/.devcontainer/
DockerFile

### ... and others Docker container (Compose and overrides)
docker-compose.yml
docker-compose.override.yml
.dockerignore

# configuration about git
.gitignore

# configuration or rule used on AI
## Gemini Code Assisant VSC plugin
Gemini.md

## cline
.clinerules

## template of rule sets for ai agent
ai-rules.md

### ... and others AI Agent (Cursor rules, Copilot instructions)
.cursorrules
github-copilot-instructions.md

# configuration about VSC
.vscode/

# configuration about VS
.vssetting/
.vssetting[Ss]/
.suo/
.vs/

# configuration about .NET SDK 
nuget.config

# configuration about MonoRepo in .NET 
*.props
*.targets

# Test project
*.Test
*.Test[Ss]

# Demo project
*Demo
*Demo[Ss]

# special folder
## ideas
idea/
idea[Ss]/

## special configuration and my ideas
my-idea-global.json5
notification-manager-idea.md
NotificationTarget.cs
projects.ignore.json

# images
*.png
*.jpg
*.jpeg
*.gif
*.bmp
*.tif
*.webp
*.heic
*.dwg
*.psd
*.raw

### ... and others (vector graphics and raw photography formats)
*.svg
*.eps
*.ai
*.cr2
*.nef

# Office
## Office Word
*.docx
*.doc

### --- for older version --- ###
*.odt

## Office Excel
*.xlsx
xls

### --- for older version --- ###
*.ods

## Office OneNote
*.one
*.onetoc2

## Office PowerPoint
*.pptx
*.ppt

### --- for older version --- ###
*.odp

### ... and others Office products (Visio, Project, Publisher)
*.vsdx
*.mpp
*.pub

# Pdf
*.pdf

# Database
*.db

# query about database
## MySQL
*.mssql

## SQL
*.sql

## NoSQL
*.litedb

## sqlLite
*.sqlite

## MongoDB
*.ns
*.[0-9]

*.wt
WiredTiger
WiredTiger.wt
WiredTiger.lock

### ... and others file about MongoDB
*.bson
mongod.lock

### ... and other files about other differnt kinds of DB (please comment the DB kind such as #mysql (for MySQL) )
#mssql
*.mdf
*.ldf
#postgresql
*.dump
#oracle
*.dmp

### ================================================ ###
### === THESE MATCHING FILES WILL NOT BE IGNORED === ###
### ================================================ ###

# README
!**/README.md

# solution configuration files
## newer version (at 2026)
!**/*.slnx

# project configuration files
!**/*.csproj
!**/*.proj

# configuration used by MSBuild in .NET 
!Directory.Build.props
!Directory.Build.targets

!Directory.Packages.props
!Directory.Packages.targets

### ... and others (solution legacy file format support if needed)
!**/*.sln
```

## File: FileStreamUtilityServices.slnx
```
<Solution>
    <Project Path="FileStreamUtilityServices.Tests/FileStreamUtilityServices.Tests.csproj" Id="597b4083-7671-4e72-afb8-158d08e858c9" />
    <Project Path="FileStreamUtilityServices/FileStreamUtilityServices.csproj" />
</Solution>
```

## File: FileStreamUtilityServices/FileStreamUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.1.0</Version>
    <PackageTags>$(PackageTags);file;stream;utility;lock</PackageTags>
  </PropertyGroup>
</Project>
```

## File: FileStreamUtilityServices/FileStreamUtilityServices.csproj.lscache
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
AssemblyName=FileStreamUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=FileStreamUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/FileStreamUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/FileStreamUtilityServices.dll
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
/out:obj\Debug\net10.0\FileStreamUtilityServices.dll
/refout:obj\Debug\net10.0\refint\FileStreamUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
FileUtility.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 FileStreamUtilityServices.AssemblyInfo.cs
 FileStreamUtilityServices.GlobalUsings.g.cs

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
obj/Debug/net10.0/FileStreamUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=FileStreamUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=FileStreamUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/FileStreamUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/FileStreamUtilityServices.dll
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
/out:obj\Debug\net8.0\FileStreamUtilityServices.dll
/refout:obj\Debug\net8.0\refint\FileStreamUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
FileUtility.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 FileStreamUtilityServices.AssemblyInfo.cs
 FileStreamUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/FileStreamUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=FileStreamUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=FileStreamUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/FileStreamUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/FileStreamUtilityServices.dll
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
/out:obj\Debug\net9.0\FileStreamUtilityServices.dll
/refout:obj\Debug\net9.0\refint\FileStreamUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
FileUtility.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 FileStreamUtilityServices.AssemblyInfo.cs
 FileStreamUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/FileStreamUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: FileStreamUtilityServices/FileUtility.cs
```csharp
using System.IO;
using System.Text;

namespace FileStreamUtilityServices
{
    public static class FileUtility
    {
        private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;
        /// <summary>
        /// 以獨佔鎖定方式讀取檔案 (同步版本)，防止讀取期間內容被修改
        /// </summary>
        public static string ReadWithLock(
            string filePath,
            Encoding encoding = null
        )
        {
            if(string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path can niether be null nor empty." , nameof(filePath));
            }

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File does NOT exists: {filePath}");
            }

            encoding ??= DEFAULT_ENCODING;

            // FileMode.Open: 打開現有檔案
            // FileAccess.Read: 我只讀取
            // FileShare.None: 重要！這會鎖定檔案，讀取期間不允許其他程序(如 VSC 存檔)讀取或寫入
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            using (var reader = new StreamReader(fs,encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 以獨佔鎖定方式讀取檔案 (非同步版本)
        /// </summary>
        public static async Task<string> ReadWithLockAsync(
            string filePath ,
            Encoding encoding = null
        )
        {
            if(string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path can niether be null nor empty." , nameof(filePath));
            }

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File does NOT exists: {filePath}");
            }
            // 預設使用 UTF8
            encoding ??= DEFAULT_ENCODING;

            // FileShare.None 確保讀取期間其他程序無法存取
            using var fs = new FileStream(
                filePath ,
                FileMode.Open ,
                FileAccess.Read ,
                FileShare.None ,
                bufferSize: 4096 ,
                useAsync: true
            );

            using var reader = new StreamReader(fs , encoding);
            return await reader.ReadToEndAsync();
        }
    }
}
```

## File: FileStreamUtilityServices/README.md
```markdown
# Description
An utility package that provides various file stream operations, such as reading files with lock.

# Features
## 1.0.0-preview-1.0.0
### Added
+ reading files with lock, avoid race condition when multiple processes read the same file.

## 2.0.0-preview-1.0.0
### Changed
+ Rename the project

+ Rename the namespace

+ Make a documentation.

## 2.1.0
### Added
+ supports for different encoding.

+ reading files with lock (`async` version)

### Added test
+ Added unit tests to mock.
```
