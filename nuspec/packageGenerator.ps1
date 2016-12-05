
$location  = $env:APPVEYOR_BUILD_FOLDER

$locationNuspec = $location + "\nuspec"
$locationNuspec
	
Set-Location -Path $locationNuspec

"Packaging to nuget..."
"Build folder : " + $location

$strPath = $location + '\IOAuthClient\bin\IOAuthClient.dll'

$VersionInfos = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($strPath)
$ProductVersion = $VersionInfos.ProductVersion
"Product version : " + $ProductVersion

"Update nuspec versions ..."
	
$nuSpecFile =  $locationNuspec + '\IOAuthClient.nuspec'
(Get-Content $nuSpecFile) | 
Foreach-Object {$_ -replace "{BuildNumberVersion}", "$ProductVersion" } |
Set-Content $nuSpecFile

"Generate nuget packages ..."
.\NuGet.exe pack IOAuthClient.nuspec

$apiKey = $env:NuGetApiKey
	
"Publish packages ..."	
.\NuGet push IOAuthClient.$ProductVersion.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey $apiKey