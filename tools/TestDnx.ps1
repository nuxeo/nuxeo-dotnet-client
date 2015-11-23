### settings
$relativeNuxeoClientFolderPath = "../"
###

$homeDir = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$nuxeoClientFolder = [IO.Path]::GetFullPath((Join-Path $homeDir $relativeNuxeoClientFolderPath))
$nuxeoClientTck = Join-Path $nuxeoClientFolder test/TCK

cd $nuxeoClientTck

Write-Host "Building Nuxeo Client TCK (dnxcore50)" -ForegroundColor Yellow
dnu restore  -s https://www.myget.org/F/aspnetvnext/ -f https://www.nuget.org/api/v2/
dnu build --configuration Release


if ($LASTEXITCODE -ne 0)
{
    Write-Host "Nuxeo Client TCK (dnxcore50) build failed" -ForegroundColor Red
    cd $homeDir
    Exit 1
}
else
{
    Write-Host "Nuxeo Client TCK (dnxcore50) build succeeded" -ForegroundColor Green
}

Write-Host "Running Nuxeo Client TCK (dnxcore50)" -ForegroundColor Yellow
dnx test

if ($LASTEXITCODE -ne 0)
{
    Write-Host "Nuxeo Client TCK (dnxcore50) failed" -ForegroundColor Red
    cd $homeDir
    Exit 1
}
else
{
    Write-Host "Nuxeo Client TCK (dnxcore50) passed" -ForegroundColor Green
    cd $homeDir
    Exit 0
}