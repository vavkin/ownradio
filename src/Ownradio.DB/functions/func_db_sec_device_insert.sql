CREATE OR REPLACE FUNCTION db.sec_device_insert (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
, adeviceName varchar(256)
, adeviceDescription varchar(1024)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 08/09/2016
	-- Purpose : add user device
	-- Version : 1.0.0

DECLARE
	fdevice_id VARCHAR(128);
BEGIN
	PERFORM db.set_app_user(auserapp);

	INSERT INTO db.sec_device (
			  device_id
			, user_id
			, name 
			, description 
			, is_active)
	VALUES (  adeviceID
			, auser_id
			, adeviceName
			, adeviceDescription 
			, true );
	RETURN 1;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_device_insert (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
, adeviceName varchar(256)
, adeviceDescription varchar(1024)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_device_insert (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
, adeviceName varchar(256)
, adeviceDescription varchar(1024)
, auserapp VARCHAR(128)
) TO writer;