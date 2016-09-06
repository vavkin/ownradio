@echo off
set PGHOST="localhost"
set PGSUPERUSER="postgres"
set PGUSER="admin"
set PGBIN="C:\Program Files\PostgreSQL\9.4\bin\"
REM set PGBIN="C:\Program Files (x86)\PostgreSQL\9.4\bin\"

cd ..
set PGDATA=%CD%
if not exist "%PGDATA%\deploy\log" mkdir %PGDATA%\deploy\log
echo Start deploying musicdb DB
echo **************************************
echo Creting System Objects ...
%PGBIN%psql --host %PGHOST% --port 5432 --username %PGSUPERUSER% --dbname postgres -f %PGDATA%\deploy\system_objects.sql >> %PGDATA%\deploy\log\log_system_objects.txt
echo Creating musicdb schema ...
%PGBIN%psql --host %PGHOST% --port 5432 --username %PGUSER% --dbname musicdb -f %PGDATA%\deploy\musicdb_schema.sql >> %PGDATA%\deploy\log\log_musicdb_schema.txt
echo Deploy database ...
%PGBIN%psql --host %PGHOST% --port 5432 --username %PGUSER% --dbname musicdb -f %PGDATA%\build.sql >> %PGDATA%\deploy\log\log_build.txt
echo **************************************
echo End deploying musicdb DB
pause