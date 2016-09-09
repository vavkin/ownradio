CREATE OR REPLACE FUNCTION db.sec_user_profile_update (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, auser_name VARCHAR(256)
, auserapp VARCHAR(128)
) RETURNS VARCHAR(128) AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : update the information about user by id
	-- Version : 1.0.0

DECLARE
	fid VARCHAR(128);
BEGIN
	PERFORM db.set_app_user(auserapp); 
  
	UPDATE db.sec_user 
	SET email = aemail
		, first_name = afirst_name
		, last_name = alast_name
		, middle_name = amiddle_name
		, gender = agender
		, birthdate = abirthdate
		, phone = aphone
		, user_name = auser_name
	WHERE id = aid
	RETURNING id INTO fid;

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_profile_update (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, auser_name VARCHAR(256)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_profile_update (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, auser_name VARCHAR(256)
, auserapp VARCHAR(128)
) TO writer;