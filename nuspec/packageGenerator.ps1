
$location  = $env:APPVEYOR_BUILD_FOLDER

$locationNuspec = $location + "\nuspec"
$locationNuspec
	
Set-Location -Path $locationNuspec

"Packaging to nuget..."
"Build folder : " + $location

$strPath = $location + '\src\Auth0Client.iOS\bin\unified\Release\Auth0Client.iOS.dll'

$VersionInfos = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($strPath)
$ProductVersion = $VersionInfos.ProductVersion
"Product version : " + $ProductVersion

"Update nuspec versions ..."
	
$nuSpecFile =  $locationNuspec + '\Xamarin.Auth0Client.Unofficial.nuspec'
(Get-Content $nuSpecFile) | 
Foreach-Object {$_ -replace "{BuildNumberVersion}", "$ProductVersion" } |
Set-Content $nuSpecFile

"Generate nuget packages ..."
.\NuGet.exe pack Xamarin.Auth0Client.Unofficial.nuspec

$apiKey = $env:NuGetApiKey
	
"Publish packages ..."	
.\NuGet push Xamarin.Auth0Client.Unofficial.$ProductVersion.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey $apiKey