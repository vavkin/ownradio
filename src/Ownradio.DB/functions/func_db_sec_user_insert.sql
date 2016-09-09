CREATE OR REPLACE FUNCTION db.sec_user_insert (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, apwd TEXT
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, aemail_confirmed BOOLEAN
, asecurity_stamp TEXT
, alockout_end_date TIMESTAMP
, alockout_enabled BOOLEAN
, aaccess_failed_count INTEGER
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : insert new record to db.sec_user
	-- Version : 1.0.0

DECLARE
	fid VARCHAR(128);
BEGIN
	PERFORM db.set_app_user(auserapp);
	fid := 1;

	INSERT INTO db.sec_user (
				id
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
			  , user_name
			  , api
			  , backend)
	VALUES (
				  aid
				, aemail
				, afirst_name
				, alast_name
				, amiddle_name
				, apwd
				, agender
				, abirthdate
				, '+' || regexp_replace(aphone, '\D', '', 'g')
				, aemail_confirmed
				, asecurity_stamp
				, alockout_end_date
				, alockout_enabled
				, aaccess_failed_count
				, auser_name
				, aapi
				, abackend);

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_insert (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, apwd TEXT
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, aemail_confirmed BOOLEAN
, asecurity_stamp TEXT
, alockout_end_date TIMESTAMP
, alockout_enabled BOOLEAN
, aaccess_failed_count INTEGER
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_insert (
  aid VARCHAR(128)
, aemail VARCHAR(256)
, afirst_name VARCHAR(30)
, alast_name VARCHAR(30)
, amiddle_name VARCHAR(30)
, apwd TEXT
, agender CHAR(1)
, abirthdate TIMESTAMP
, aphone VARCHAR(50)
, aemail_confirmed BOOLEAN
, asecurity_stamp TEXT
, alockout_end_date TIMESTAMP
, alockout_enabled BOOLEAN
, aaccess_failed_count INTEGER
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) TO writer;