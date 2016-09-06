@echo off
set PGHOST="172.31.0.200"
set PGUSER="admin"
set PGPASSWORD=admin
set PGBIN="C:\PostgreSQL\9.4\bin\"
set DBN="blockchain"

set PGDATA=%CD%
if not exist "%PGDATA%\patch\log" mkdir %PGDATA%\patch\log

echo Start Patch DB
cd %PGDATA%\patch\
%PGBIN%psql --host %PGHOST% --port 5432 --username %PGUSER% --dbname %DBN% -f %PGDATA%\patch\patch.sql --set ON_ERROR_STOP=on >> %PGDATA%\patch\log\log_patch.txt
if %ERRORLEVEL%==0 (
  RMDIR /S /Q "%PGDATA%\patch\log"
  Echo End Patch DB
) else (
  echo Patch DB FAILED: Please check: "%PGDATA%\patch\log\log_patch.txt" file
  exit /b %ERRORLEVEL%
)
