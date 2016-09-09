CREATE OR REPLACE FUNCTION db.sec_user_update (
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
, alastupdatedatetime TIMESTAMP
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) RETURNS VARCHAR(128) AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : update record of the db.sec_user
	-- Version : 1.0.0
	
DECLARE
	fid VARCHAR(128);
	flastupdatedatetime TIMESTAMP;
BEGIN
	PERFORM db.set_app_user(auserapp); 
	flastupdatedatetime := now(); 
  
	UPDATE db.sec_user 
	SET email = aemail
		, first_name = afirst_name
		, last_name = alast_name
		, middle_name = amiddle_name
		, pwd = apwd
		, gender = agender
		, birthdate = abirthdate
		, phone = '+' || regexp_replace(aphone, '\D', '', 'g')
		, email_confirmed = aemail_confirmed
		, security_stamp = asecurity_stamp
		, lockout_end_date = alockout_end_date
		, lockout_enabled = alockout_enabled
		, access_failed_count = aaccess_failed_count
		, lastupdatedatetime = flastupdatedatetime
		, user_name = auser_name
		, api = aapi
		, backend = abackend
	WHERE id = aid AND lastupdatedatetime = alastupdatedatetime
	RETURNING id INTO fid;

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_update (
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
, alastupdatedatetime TIMESTAMP
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_update (
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
, alastupdatedatetime TIMESTAMP
, auser_name VARCHAR(256)
, aapi BOOLEAN
, abackend BOOLEAN
, auserapp VARCHAR(128)
) TO writer;