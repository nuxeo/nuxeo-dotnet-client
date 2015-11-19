### settings
$relativeNuxeoClientFolderPath = "../"
###

$homeDir = (Get-Item -Path "./" -Verbose).FullName
$nuget = Join-Path $homeDir nuget.exe
$nuspec = Join-Path $homeDir NuxeoClient.nuspec

if (-Not (Test-Path $nuget))
{
    Write-Host "Downloading nuget.exe" -ForegroundColor Yellow
    Invoke-WebRequest "http://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -UseBasicParsing -OutFile $nuget
}

Write-Host "Creating NuGet package" -ForegroundColor Yellow

Invoke-Expression $nuget pack