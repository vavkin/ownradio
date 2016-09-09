CREATE OR REPLACE FUNCTION db.sec_user_claims_clean(
) RETURNS void AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : function that clears user claims less than current date
	-- Version : 1.0.0

	DELETE FROM  db.sec_user_claims WHERE expiry_date < now()::date;
$$ LANGUAGE sql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_claims_clean() FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_claims_clean() TO writer;