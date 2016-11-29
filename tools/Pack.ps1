### settings
$relativeNuxeoClientFolderPath = "../"
###

$homeDir = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$nuget = Join-Path $homeDir nuget.exe
$nuspec = Join-Path $homeDir NuxeoClient.nuspec

if (-Not (Test-Path $nuget))
{
    Write-Host "Downloading nuget.exe" -ForegroundColor Yellow
    Invoke-WebRequest "http://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -UseBasicParsing -OutFile $nuget
}

Write-Host "Creating NuGet package" -ForegroundColor Yellow

Invoke-Expression "$nuget pack"

if ($LASTEXITCODE -ne 0)
{
    Write-Host "Packing up failed" -ForegroundColor Red
    Exit 1
}
else
{
    Write-Host "Packing up succeeded" -ForegroundColor Green
    Exit 0
}
