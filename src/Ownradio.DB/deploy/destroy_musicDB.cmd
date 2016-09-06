@echo off
set PGHOST="localhost"
set PGSUPERUSER="postgres"
set PGBIN="C:\PostgreSQL\9.4\bin\"
REM set PGBIN="C:\Program Files (x86)\PostgreSQL\9.4\bin\"

set PGDATA=%CD%
if not exist "%PGDATA%\log" mkdir %PGDATA%\log
echo Start removing musicdb DB
echo **************************************
%PGBIN%psql --host %PGHOST% --port 5432 --username %PGSUPERUSER% --dbname postgres -f %PGDATA%\drop_musicdb.sql >> %PGDATA%\log\log_drop_musicdb.txt
echo **************************************
echo End removing musicdb DB
pause