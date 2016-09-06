
SET client_min_messages TO WARNING;
SET client_encoding = 'win1251';

SELECT pg_terminate_backend(pid) 
FROM pg_stat_activity
WHERE 
-- don't kill my own connection!
pid <> pg_backend_pid()
-- don't kill the connections to other databases
AND datname = 'musicdb';

-- drop database: musicdb
DROP DATABASE IF EXISTS musicdb;
-- drop tablespacs: musicdb_data, musicdb_index
DROP TABLESPACE IF EXISTS musicdb_data;
DROP TABLESPACE IF EXISTS musicdb_index;
-- drop users: admin, apiuser, writer 
DROP USER IF EXISTS admin, apiuser, writer;