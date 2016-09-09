CREATE OR REPLACE FUNCTION db.sec_user_claims_select_all (
auser_id VARCHAR(128)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : select, with parameters page_num, page_size - select the paged list of entities
	-- Version : 1.0.0

DECLARE
	rc REFCURSOR;
BEGIN
	OPEN rc FOR
	SELECT  claim_type
		, claim_value
		, expiry_date
	FROM db.sec_user_claims
	WHERE user_id = auser_id;
	RETURN NEXT rc;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_claims_select_all (
auser_id VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_claims_select_all (
auser_id VARCHAR(128)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_user_claims_select_all (
auser_id VARCHAR(128)
) TO apiuser;