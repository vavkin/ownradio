CREATE OR REPLACE FUNCTION db.sec_role_update (
  aid VARCHAR(128)
, aname VARCHAR(256)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) RETURNS VARCHAR(128) AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : update record of the db.sec_role
	-- Version : 1.0.0

DECLARE
	fid VARCHAR(128);
	flastupdatedatetime TIMESTAMP;
BEGIN
	PERFORM db.set_app_user(auserapp); 
	flastupdatedatetime := now();   

	UPDATE db.sec_role 
	SET  name = aname
		, lastupdatedatetime = flastupdatedatetime
	WHERE id = aid AND lastupdatedatetime = alastupdatedatetime
	RETURNING id INTO fid;

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_update (
  aid VARCHAR(128)
, aname VARCHAR(256)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_update (
  aid VARCHAR(128)
, aname VARCHAR(256)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) TO writer;