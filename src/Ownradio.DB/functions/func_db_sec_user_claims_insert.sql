CREATE OR REPLACE FUNCTION db.sec_user_claims_insert (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, aexpiry_date TIMESTAMP
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : insert new record to db.sec_user_claims
	-- Version : 1.0.0

DECLARE
	fid INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);

	INSERT INTO db.sec_user_claims (
				  user_id
				, claim_type
				, claim_value
				, expiry_date)
	VALUES ( auser_id
			, aclaim_type
			, aclaim_value
			, aexpiry_date)
	RETURNING id INTO fid;

	RETURN fid;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_claims_insert (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, aexpiry_date TIMESTAMP
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_claims_insert (
  auser_id VARCHAR(128)
, aclaim_type VARCHAR(4000)
, aclaim_value VARCHAR(4000)
, aexpiry_date TIMESTAMP
, auserapp VARCHAR(128)
) TO writer;