CREATE OR REPLACE FUNCTION db.sec_role_find_by_name (
aname VARCHAR(256)
) RETURNS db.sec_role AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : find, with id parameter - returns the sec_role by name
	-- Version : 1.0.0

	SELECT id
		, name
		,lastupdatedatetime
	FROM db.sec_role 
	WHERE name = aname;
$$ LANGUAGE sql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_find_by_name (
aname VARCHAR(256)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_find_by_name (
aname VARCHAR(256)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_role_find_by_name (
aname VARCHAR(256)
) TO apiuser;