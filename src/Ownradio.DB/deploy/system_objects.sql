SET client_encoding = 'win1251';

CREATE USER admin WITH ENCRYPTED PASSWORD 'admin';
ALTER USER admin NOSUPERUSER NOCREATEROLE;

CREATE TABLESPACE musicdb_data OWNER admin LOCATION 'C:/PostgreSQL/musicdb/data';
CREATE TABLESPACE musicdb_index OWNER admin LOCATION 'C:/PostgreSQL/musicdb/index';

CREATE DATABASE musicdb OWNER admin TABLESPACE musicdb_data;

CREATE USER apiuser WITH ENCRYPTED PASSWORD 'apiuser';
CREATE USER writer WITH ENCRYPTED PASSWORD 'writer';

GRANT CONNECT ON DATABASE musicdb TO apiuser;
GRANT CONNECT ON DATABASE musicdb TO writer;