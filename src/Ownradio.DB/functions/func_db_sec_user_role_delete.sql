CREATE OR REPLACE FUNCTION db.sec_user_role_delete (
  auserID VARCHAR(128) 
, aroleName VARCHAR(256)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : db.sec_user_remove_role with the following arguments - userI_id, role_name (removes role for user)
	-- Version : 1.0.0

DECLARE
	frole_id VARCHAR(128);
BEGIN
	PERFORM db.set_app_user(auserapp);

	SELECT id
	INTO frole_id
	FROM db.sec_role
	WHERE name = aroleName;

	IF coalesce(frole_id, '') = '' THEN
		RETURN 0;
	ELSE
		DELETE FROM db.sec_user_role ur
		WHERE ur.user_id = auserID
			AND ur.role_id = frole_id;
		RETURN 1;
	END IF;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_role_delete (
  auserID VARCHAR(128) 
, aroleName VARCHAR(256)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_role_delete (
  auserID VARCHAR(128) 
, aroleName VARCHAR(256)
, auserapp VARCHAR(128)
) TO writer;