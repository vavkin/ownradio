CREATE OR REPLACE FUNCTION db.set_app_user (
ausername varchar
) RETURNS void AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : get application user
	-- Version : 1.0.0

BEGIN
	PERFORM set_config('musicdb.username', ausername, false);
END;
$$
LANGUAGE 'plpgsql'
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.set_app_user (
ausername varchar
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.set_app_user (
ausername varchar
) TO writer;