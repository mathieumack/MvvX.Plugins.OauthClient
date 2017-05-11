write-host "**************************" -foreground "Cyan"
write-host "*   Packaging to nuget   *" -foreground "Cyan"
write-host "**************************" -foreground "Cyan"

$location  = $env:APPVEYOR_BUILD_FOLDER

$locationNuspec = $location + "\nuspec"
$locationNuspec
	
Set-Location -Path $locationNuspec

write-host "Update the nuget.exe file" -foreground "DarkGray"
.\NuGet update -self

$strPath = $location + '\MvvX.Plugins.IOAuthClient\bin\Release\MvvX.Plugins.IOAuthClient.dll'

$VersionInfos = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($strPath)
$ProductVersion = $VersionInfos.ProductVersion
write-host "Product version : " $ProductVersion -foreground "Green"

write-host "Packaging to nuget..." -foreground "Magenta"
write-host "Generate nuget packages" -foreground "Green"

write-host "Generate nuget package for MvvX.Plugins.OAuthClient.nuspec" -foreground "DarkGray"
.\NuGet.exe pack MvvX.Plugins.OAuthClient.nuspec -Version $ProductVersion

$apiKey = $env:NuGetApiKey
	
write-host "Publish nuget packages" -foreground "Green"

write-host MvvX.Plugins.OAuthClient.$ProductVersion.nupkg -foreground "DarkGray"
.\NuGet push MvvX.Plugins.OAuthClient.$ProductVersion.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey $apiKey