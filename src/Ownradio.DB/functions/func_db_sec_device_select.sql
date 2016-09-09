CREATE OR REPLACE FUNCTION db.sec_device_select (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 08/09/2016
	-- Purpose : find user device by device id
	-- Version : 1.0.0

DECLARE
	rc REFCURSOR;
BEGIN
	OPEN rc FOR
	SELECT   d.device_id
		   , d.user_id
		   , d.name
		   , d.description
		   , d.is_active
		   , d.lastupdatedatetime
	FROM db.sec_device d
	WHERE d.user_id = auser_id
		  and d.device_id = adeviceID;
	RETURN NEXT rc;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_device_select (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_device_select (
  auser_id VARCHAR(128)
, adeviceID VARCHAR(128)
) TO writer;
