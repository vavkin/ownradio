CREATE OR REPLACE FUNCTION db.sec_user_find_by_user_name (
  auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
) RETURNS db.sec_user AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : find, with the user_name parameter - returns the db.sec_user
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
	WHERE user_name = auser_name 
		AND CASE WHEN COALESCE(aapi, '0') = '0' THEN backend = abackend 
				ELSE api = aapi END;
$$ LANGUAGE sql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_find_by_user_name (
  auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_find_by_user_name (
  auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
) TO writer;
