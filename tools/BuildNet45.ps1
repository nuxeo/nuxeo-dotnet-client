### settings
$relativeNuxeoClientFolderPath = "../"
###

(new-object Net.WebClient).DownloadString("https://raw.githubusercontent.com/ligershark/psbuild/master/src/GetPSBuild.ps1") | iex

$homeDir = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$nuxeoClientFolder = [IO.Path]::GetFullPath((Join-Path $homeDir $relativeNuxeoClientFolderPath))
$nuxeoClientSolution = Join-Path $nuxeoClientFolder "NuxeoClient_net45.sln"
$msbuildLogFile = Join-Path $nuxeoClientFolder tools/msbuild.log
$nuget = Join-Path $homeDir nuget.exe

if (-Not (Test-Path $nuget))
{
    Write-Host "Downloading nuget.exe" -ForegroundColor Yellow
    Invoke-WebRequest "http://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -UseBasicParsing -OutFile $nuget
}

cd $nuxeoClientFolder

Write-Host "Restoring packages for Nuxeo Client (net45)" -ForegroundColor Yellow
Invoke-Expression "$nuget restore $nuxeoClientSolution"

Write-Host "Building Nuxeo Client (net45)" -ForegroundColor Yellow

Try
{
    Invoke-MsBuild $nuxeoClientSolution -Configuration Release -targets Clean,Build -extraArgs "/fl /flp:logfile=""$msbuildLogFile"";verbosity=detailed"
}
Catch [Exception]
{
    Write-Host Exception
    Write-Host "Nuxeo Client build (net45) failed" -ForegroundColor Red
    Exit 1
}
Finally
{
  cd $homeDir
}

Write-Host "Nuxeo Client (net45) build succeeded" -ForegroundColor Green
Exit 0
