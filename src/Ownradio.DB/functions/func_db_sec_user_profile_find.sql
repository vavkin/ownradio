CREATE OR REPLACE FUNCTION db.sec_user_profile_find (
  IN aid VARCHAR(128)
, OUT email VARCHAR(256)
, OUT first_name VARCHAR(30)
, OUT last_name VARCHAR(30)
, OUT middle_name VARCHAR(30)
, OUT gender CHAR(1)
, OUT birthdate TIMESTAMP
, OUT phone VARCHAR(50)
, OUT user_name VARCHAR(256)
) RETURNS SETOF record AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : retrieve information about user by id
	-- Version : 1.0.0

	SELECT 
		CASE WHEN email_confirmed = true THEN email
		ELSE 'email not confirmed'
		END
		, first_name
		, last_name
		, middle_name
		, gender
		, birthdate
		, phone
		, user_name
	FROM db.sec_user
	WHERE id = aid;
$$ LANGUAGE sql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_profile_find (
IN aid VARCHAR(128)
, OUT email VARCHAR(256)
, OUT first_name VARCHAR(30)
, OUT last_name VARCHAR(30)
, OUT middle_name VARCHAR(30)
, OUT gender CHAR(1)
, OUT birthdate TIMESTAMP
, OUT phone VARCHAR(50)
, OUT user_name VARCHAR(256)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_profile_find (
IN aid VARCHAR(128)
, OUT email VARCHAR(256)
, OUT first_name VARCHAR(30)
, OUT last_name VARCHAR(30)
, OUT middle_name VARCHAR(30)
, OUT gender CHAR(1)
, OUT birthdate TIMESTAMP
, OUT phone VARCHAR(50)
, OUT user_name VARCHAR(256)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_user_profile_find (
IN aid VARCHAR(128)
, OUT email VARCHAR(256)
, OUT first_name VARCHAR(30)
, OUT last_name VARCHAR(30)
, OUT middle_name VARCHAR(30)
, OUT gender CHAR(1)
, OUT birthdate TIMESTAMP
, OUT phone VARCHAR(50)
, OUT user_name VARCHAR(256)
) TO apiuser;