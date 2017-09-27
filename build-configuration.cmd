@echo off

%FAKE% %NYX% "target=clean" -st
%FAKE% %NYX% "target=RestoreNugetPackages" -st

IF NOT [%1]==[] (set RELEASE_NUGETKEY="%1")

SET SUMMARY="Elders.RedLock"
SET DESCRIPTION="Elders.Redlock"

%FAKE% %NYX% appName=RedLock appSummary=%SUMMARY% appDescription=%DESCRIPTION% nugetPackageName=RedLock nugetkey=%RELEASE_NUGETKEY%
