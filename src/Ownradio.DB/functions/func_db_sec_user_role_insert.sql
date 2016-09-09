CREATE OR REPLACE FUNCTION db.sec_user_role_insert (
  auser_id VARCHAR(128)
, arole_name VARCHAR(256)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : assign user to role
	-- Version : 1.0.0

DECLARE
	frole_id VARCHAR(128);
BEGIN
	PERFORM db.set_app_user(auserapp);

	SELECT id
	INTO frole_id
	FROM db.sec_role
	WHERE name = arole_name;

	IF coalesce(frole_id, '') = '' THEN
		RETURN 0;
	ELSE
		INSERT INTO db.sec_user_role (
				  user_id
				, role_id )
		VALUES ( auser_id
				, frole_id);
		RETURN 1;
	END IF;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_role_insert (
  auser_id VARCHAR(128)
, arole_name VARCHAR(256)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_role_insert (
  auser_id VARCHAR(128)
, arole_name VARCHAR(256)
, auserapp VARCHAR(128)
) TO writer;