
write-host "**************************" -foreground "Cyan"
write-host "*   Packaging to nuget   *" -foreground "Cyan"
write-host "**************************" -foreground "Cyan"

#$location  = "C:\Sources\NoSqlRepositories";
$location  = $env:APPVEYOR_BUILD_FOLDER

$locationNuspec = $location + "\nuspec"
$locationNuspec
	
Set-Location -Path $locationNuspec

$strPath = $location + '\MvvX.Plugins.IOAuthClient\bin\Release\MvvX.Plugins.IOAuthClient.dll'

$VersionInfos = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($strPath)
$ProductVersion = $VersionInfos.ProductVersion
write-host "Product version : " $ProductVersion -foreground "Green"

write-host "Packaging to nuget..." -foreground "Magenta"

write-host "Update nuspec versions" -foreground "Green"

write-host "Update nuspec versions MvvX.Plugins.IOAuthClient.nuspec" -foreground "DarkGray"
$nuSpecFile =  $locationNuspec + '\MvvX.Plugins.IOAuthClient.nuspec'
(Get-Content $nuSpecFile) | 
Foreach-Object {$_ -replace "{BuildNumberVersion}", "$ProductVersion" } | 
Set-Content $nuSpecFile

write-host "Generate nuget packages" -foreground "Green"

write-host "Generate nuget package for MvvX.Plugins.IOAuthClient.nuspec" -foreground "DarkGray"
.\NuGet.exe pack MvvX.Plugins.IOAuthClient.nuspec

$apiKey = $env:NuGetApiKey
	
write-host "Publish nuget packages" -foreground "Green"

write-host MvvX.Plugins.IOAuthClient.$ProductVersion.nupkg -foreground "DarkGray"
.\NuGet push MvvX.Plugins.IOAuthClient.$ProductVersion.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey $apiKey
