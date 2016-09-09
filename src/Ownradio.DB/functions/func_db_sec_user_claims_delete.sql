CREATE OR REPLACE FUNCTION db.sec_user_claims_delete (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : delete record of the db.sec_user_claims
	-- Version : 1.0.0

DECLARE
	fid INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);

	DELETE FROM db.sec_user_claims 
	WHERE user_id = auser_id
		AND claim_type = aclaim_type
		AND claim_value = aclaim_value
	RETURNING id INTO fid;

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_claims_delete (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_claims_delete (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, auserapp VARCHAR(128)
) TO writer;