CREATE OR REPLACE FUNCTION db.sec_role_list_for_userid (
auser_id VARCHAR(128)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : list of role for user
	-- Version : 1.0.0

DECLARE
	rc REFCURSOR;
BEGIN
	OPEN rc FOR
	SELECT r.id
		, r.name
		, rd.name AS description
		, true AS enabled
		, r.lastupdatedatetime
	FROM db.sec_role r 
	INNER JOIN db.sec_role_description rd ON r.id = rd.role_id
	WHERE rd.lang_id = 1;
	RETURN NEXT rc;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_list_for_userid (
auser_id VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_list_for_userid (
auser_id VARCHAR(128)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_role_list_for_userid (
auser_id VARCHAR(128)
) TO apiuser;