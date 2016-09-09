CREATE OR REPLACE FUNCTION db.sec_user_find (
aid VARCHAR(128)
) RETURNS db.sec_user AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : find, with id parameter - returns the db.sec_user by it's id
	-- Version : 1.0.0
	
	SELECT id
		, email
		, first_name
		, last_name
		, middle_name
		, pwd
		, gender
		, birthdate
		, phone
		, email_confirmed
		, security_stamp
		, lockout_end_date
		, lockout_enabled
		, access_failed_count
		, lastupdatedatetime
		, user_name
		, api
		, backend
	FROM db.sec_user
	WHERE id = aid;
$$ LANGUAGE sql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_find (
aid VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_find (
aid VARCHAR(128)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_user_find (
aid VARCHAR(128)
) TO apiuser;