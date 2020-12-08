@echo off
set DBNAME=SoodaUnitTests

set SCRIPT_NAME=dropdb.sql
if not (%1)==() set SCRIPT_NAME=%1\dropdb.sql
echo %SCRIPT_NAME%

echo Dropping %DBNAME% database on %MSSQLSERVER_HOST%...
echo.

sqlcmd -S %MSSQLSERVER_HOST% -U %MSSQLSERVER_SA_USER% -P %MSSQLSERVER_SA_PASS% -i %SCRIPT_NAME%

echo Dropping %DBNAME% database... done!
echo.