CREATE OR REPLACE FUNCTION db.sec_user_role_select (
auser_id VARCHAR(128)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : find all roles to which User belongs (by user id)
	-- Version : 1.0.0

DECLARE
	rc REFCURSOR;
BEGIN
	OPEN rc FOR
	SELECT r.id
		, r.name
		, r.lastupdatedatetime
	FROM db.sec_role r
		INNER JOIN db.sec_user_role ur ON r.id = ur.role_id
	WHERE ur.user_id = auser_id;
	RETURN NEXT rc;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_role_select (
auser_id VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_role_select (
auser_id VARCHAR(128)
) TO writer;
