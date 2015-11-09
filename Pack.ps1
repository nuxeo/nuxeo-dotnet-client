# let's build the nuget package
cd src/NuxeoClient
dnu pack --configuration Release
cd ../..

# let's move it to the nupkg directory
if (Test-Path nupkg)
{
  Remove-Item nupkg/*
}
else
{
  New-Item nupkg -type directory -force
}
Move-Item src/NuxeoClient/bin/Release/NuxeoClient.*.nupkg nupkg
