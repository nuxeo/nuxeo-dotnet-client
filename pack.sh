#!/usr/bin/env bash

# let's build the nuget package
cd src/NuxeoClient
dnu pack --configuration Release
cd ../..

# let's move it to the nupkg directory
if [ -d "nupkg" ]
then
  rm nupkg/*
else
  mkdir "nupkg"
fi
mv src/NuxeoClient/bin/Release/NuxeoClient.*.nupkg nupkg
