@echo off
set DBNAME=SoodaUnitTests

echo Creating %DBNAME% database on %MSSQLSERVER_HOST%...
echo.

sqlcmd -S %MSSQLSERVER_HOST% -U %MSSQLSERVER_SA_USER% -P %MSSQLSERVER_SA_PASS% -i createdb.sql

echo Creating %DBNAME% database on %MSSQLSERVER_HOST%... done!
echo.