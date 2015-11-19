### settings
$relativeNuxeoClientFolderPath = "../"
###

$homeDir = (Get-Item -Path "./" -Verbose).FullName
$nuxeoClientFolder = [IO.Path]::GetFullPath((Join-Path $homeDir $relativeNuxeoClientFolderPath))
$nuxeoClientSrc = Join-Path $nuxeoClientFolder src/NuxeoClient

cd $nuxeoClientSrc

Write-Host "Building Nuxeo Client (dnxcore50)" -ForegroundColor Yellow
dnu restore -s https://www.myget.org/F/aspnetvnext/ -f https://www.nuget.org/api/v2/
dnu build --configuration Release

if ($LASTEXITCODE -ne 0)
{
    Write-Host "Nuxeo Client (dnxcore50) build failed" -ForegroundColor Red
    cd $homeDir
    Exit 1
}
else
{
    Write-Host "Nuxeo Client (dnxcore50) build succeeded" -ForegroundColor Green
    cd $homeDir
    Exit 0
}