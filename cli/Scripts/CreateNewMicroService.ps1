function AddFoldersToProject ($projectPath, $folders) {
    $csprojPath = "$srcDirectory\$projectPath\$projectPath.csproj"

    [xml]$csprojContent = Get-Content -Path $csprojPath

    $newItemGroup = $csprojContent.CreateElement("ItemGroup")

    foreach ($folder in $folders) {
        $newFolder = $csprojContent.CreateElement("Folder")
        $newFolder.SetAttribute("Include", "$folder\")
        $newItemGroup.AppendChild($newFolder)
    }

    $csprojContent.Project.AppendChild($newItemGroup)
    $csprojContent.Save($csprojPath)
}

function AddAndCreateFolders ($projectPath, $folders) {
    AddFoldersToProject $projectPath $folders

    foreach ($folder in $folders) {
        New-Item -Name $folder -ItemType "directory" -Force
    }
}

function AddPackageToProject ($packageNames) {
	pwd
	foreach ($package in $packageNames) {
		dotnet add package $package
	}
}

$name = $args[0]
$firstLetterUpper = $name.Substring(0, 1).ToUpper()
$restOfName = $name.Substring(1)

if ([string]::IsNullOrWhiteSpace($name)) {
    Write-Host "The project name must be specified."
    exit 1
}

$currentDirectory = Get-Location
$srcDirectory = "$currentDirectory\$name\src"
$testDirectory = "$currentDirectory\$name\test"

$prefix = "Projectiv."
$postfix = "Service"
$textInfo = [System.Globalization.CultureInfo]::CurrentCulture.TextInfo
$projectName = "${prefix}${firstLetterUpper}${restOfName}${postfix}"

$projectHost = "${projectName}.Host"
$projectApplication = "${projectName}.Application"
$projectApplicationShared = "${projectName}.ApplicationShared"
$projectDomain = "${projectName}.Domain"
$projectDomainShared = "${projectName}.DomainShared"
$projectEntityFrameworkCore = "${projectName}.EntityFrameworkCore"
$projectApi = "${projectName}.Api"

$projectApplicationTest = "${projectName}.Application.Test"
$projectBaseTest = "${projectName}.BasrTest"

New-Item -Path $currentDirectory -Name $name -ItemType "directory"
Set-Location -Path "$currentDirectory\$name"
New-Item -Path . -Name "src" -ItemType "directory"
New-Item -Path . -Name "test" -ItemType "directory"
Set-Location -Path ".\src"

dotnet new webapi -n $projectHost
dotnet new classlib -n $projectApplication
dotnet new classlib -n $projectApplicationShared
dotnet new classlib -n $projectDomain
dotnet new classlib -n $projectDomainShared
dotnet new classlib -n $projectEntityFrameworkCore
dotnet new classlib -n $projectApi

Set-Location -Path ".."
Set-Location -Path ".\test"

dotnet new classlib -n $projectApplicationTest
dotnet new classlib -n $projectBaseTest

Set-Location -Path ".."

dotnet new sln -n $projectName

foreach ($proj in @($projectHost, $projectApplication, $projectApplicationShared, $projectDomain, $projectDomainShared, $projectEntityFrameworkCore, $projectApi)) {
    dotnet sln $projectName.sln add "src\$proj\$proj.csproj"
}

foreach ($proj in @($projectApplicationTest, $projectBaseTest)) {
    dotnet sln $projectName.sln add "test\$proj\$proj.csproj"
}

foreach ($proj in @($projectHost, $projectApi, $projectApplication, $projectApplicationShared, $projectDomain, $projectDomainShared, $projectEntityFrameworkCore)) {
    Set-Location -Path "$srcDirectory\$proj"
    Remove-Item -Path .\Class1.cs -ErrorAction SilentlyContinue
    Remove-Item -Path .\WeatherForecast.cs -ErrorAction SilentlyContinue
    Remove-Item -Path .\Controllers -Recurse -ErrorAction SilentlyContinue
	Set-Location -Path "..\.."
}

foreach ($proj in @($projectApplicationTest, $projectBaseTest)) {
    Set-Location -Path "$testDirectory\$proj"
    Remove-Item -Path .\Class1.cs -ErrorAction SilentlyContinue
	Set-Location -Path "..\.."
}

Set-Location -Path "$srcDirectory\$projectApi"
dotnet add reference "..\$projectApplication\$projectApplication.csproj"
dotnet add reference "..\$projectDomain\$projectDomain.csproj"
dotnet add reference "..\$projectApplicationShared\$projectApplicationShared.csproj"
AddAndCreateFolders $projectApi @("Controllers")
AddPackageToProject @("Swashbuckle.AspNetCore", "Newtonsoft.Json", "Grpc.AspNetCore", "Google.Protobuf", "Grpc.Net.Client", "Grpc.Tools")

Set-Location -Path "$srcDirectory\$projectApplication"
dotnet add reference "..\$projectApplicationShared\$projectApplicationShared.csproj"
dotnet add reference "..\$projectDomain\$projectDomain.csproj"
dotnet add reference "..\$projectEntityFrameworkCore\$projectEntityFrameworkCore.csproj"
AddAndCreateFolders $projectApplication @("Helpers", "Services", "Mappings", "Protos")
AddPackageToProject @("Microsoft.EntityFrameworkCore", "Google.Protobuf", "Grpc.Net.Client", "Grpc.Tools", "AutoMapper", "Grpc.AspNetCore")

Set-Location -Path "$srcDirectory\$projectApplicationShared"
dotnet add reference "..\$projectDomainShared\$projectDomainShared.csproj"
AddAndCreateFolders $projectApplicationShared @("Dtos", "Inputs")
AddPackageToProject @("Grpc.AspNetCore", "Google.Protobuf", "Google.Protobuf", "Grpc.Net.Client", "Grpc.Tools")

Set-Location -Path "$srcDirectory\$projectDomain"
dotnet add reference "..\$projectApplicationShared\$projectApplicationShared.csproj"
dotnet add reference "..\$projectDomainShared\$projectDomainShared.csproj"
AddAndCreateFolders $projectDomain @("Models", "Managers", "Interfaces", "Handlers")

Set-Location -Path "$srcDirectory\$projectDomainShared"
dotnet add reference "..\..\..\..\shared\Projectvil.Shared.EntityFramework\Projectvil.Shared.EntityFramework.csproj"
AddAndCreateFolders $projectDomain @("Configuration")

Set-Location -Path "$srcDirectory\$projectEntityFrameworkCore"
dotnet add reference "..\$projectDomain\$projectDomain.csproj"
dotnet add reference "..\..\..\..\shared\Projectvil.Shared.EntityFramework\Projectvil.Shared.EntityFramework.csproj"
AddAndCreateFolders $projectEntityFrameworkCore @("Repositories", "Scripts", "Context")
AddPackageToProject @("Microsoft.EntityFrameworkCore.Design", "Microsoft.EntityFrameworkCore.Tools", "Npgsql.EntityFrameworkCore.PostgreSQL")

Set-Location -Path "$srcDirectory\$projectHost"
dotnet add reference "..\..\..\..\shared\Projectvil.Shared.Infrastructures\Projectvil.Shared.Infrastructures.csproj"
dotnet add reference "..\$projectApi\$projectApi.csproj"
dotnet add reference "..\$projectApplication\$projectApplication.csproj"
dotnet add reference "..\$projectEntityFrameworkCore\$projectEntityFrameworkCore.csproj"
AddPackageToProject @("AutoMapper.Extensions.Microsoft.DependencyInjection", "Microsoft.AspNetCore.OpenApi", "Swashbuckle.AspNetCore")

cd ../..