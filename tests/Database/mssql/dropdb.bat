@echo off
set DBNAME=SoodaUnitTests

echo Dropping %DBNAME% database on %MSSQLSERVER_HOST%...
echo.

sqlcmd -S %MSSQLSERVER_HOST% -U %MSSQLSERVER_SA_USER% -P %MSSQLSERVER_SA_PASS% -i dropdb.sql

echo Dropping %DBNAME% database... done!
echo.