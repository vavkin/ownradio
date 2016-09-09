CREATE OR REPLACE FUNCTION db.sec_role_insert (
  aid VARCHAR(128)
, aname VARCHAR(256)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : insert new record to db.sec_role
	-- Version : 1.0.0

DECLARE
	fid INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);
	fid := 1;

	INSERT INTO db.sec_role ( id, name )
	VALUES ( aid, aname );

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_insert (
  aid VARCHAR(128)
, aname VARCHAR(256)
, auserapp VARCHAR(128)
)  FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_insert (
  aid VARCHAR(128)
, aname VARCHAR(256)
, auserapp VARCHAR(128)
) TO writer;