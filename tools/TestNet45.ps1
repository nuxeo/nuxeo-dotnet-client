### settings
$relativeNuxeoClientFolderPath = "../"
###

$homeDir = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$nuxeoClientFolder = [IO.Path]::GetFullPath((Join-Path $homeDir $relativeNuxeoClientFolderPath))
$packages = Join-Path $nuxeoClientFolder packages
$tckFolder = Join-Path $nuxeoClientFolder test/TCK_net45/
$xunitRunner = Join-Path $packages xunit.runner.console.2.1.0/tools/xunit.console.exe
$xunitTest = Join-Path $tckFolder bin/Release/TCK.dll
$xunitTestParams = "-parallel none"

Write-Host "Running Nuxeo Client TCK (net45)" -ForegroundColor Yellow

Invoke-Expression "$xunitRunner $xunitTest $xunitTestParams"

if ($LASTEXITCODE -ne 0)
{
    Write-Host "Nuxeo Client TCK (net45) failed" -ForegroundColor Red
    cd $homeDir
    Exit 1
}
else
{
    Write-Host "Nuxeo Client TCK (net45) passed" -ForegroundColor Green
    cd $homeDir
    Exit 0
}