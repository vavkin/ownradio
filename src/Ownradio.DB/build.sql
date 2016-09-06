SET client_encoding = 'win1251';
SET client_min_messages TO WARNING;
DROP SCHEMA IF EXISTS db CASCADE;
CREATE SCHEMA db AUTHORIZATION admin;
GRANT USAGE ON SCHEMA db TO apiuser;
GRANT USAGE ON SCHEMA db TO writer;
\i '01-tables.sql'
\i '03-indexes.sql'
\i '02-constraints.sql'
\i '04-views.sql'
\i '07-types.sql'
\i '05-functions.sql'
\i '06-triggers.sql'