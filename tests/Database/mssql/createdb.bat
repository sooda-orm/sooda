@echo off
set DBNAME=SoodaUnitTests

set SCRIPT_NAME=createdb.sql
if not (%1)==() set SCRIPT_NAME=%1\createdb.sql

echo Creating %DBNAME% database on %MSSQLSERVER_HOST%...
echo.

sqlcmd -S %MSSQLSERVER_HOST% -U %MSSQLSERVER_SA_USER% -P %MSSQLSERVER_SA_PASS% -i %SCRIPT_NAME%

echo Creating %DBNAME% database on %MSSQLSERVER_HOST%... done!
echo.