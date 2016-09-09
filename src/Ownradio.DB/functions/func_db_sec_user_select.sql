CREATE OR REPLACE FUNCTION db.sec_user_select (
  apage INTEGER
, amaximumrows INTEGER
, asearch_str VARCHAR(30)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : select, with parameters page_num, page_size - select the paged list of entities
	-- Version : 1.0.0
	
DECLARE
	fstartrowindex INTEGER;
	fsearch_str VARCHAR(30);
	rc REFCURSOR;
	counts REFCURSOR;
BEGIN
	fsearch_str := COALESCE(substring(asearch_str for 30),'');

	 IF apage = 0 THEN
		fstartrowindex := 0;
		amaximumrows := 0;
	ELSE 
		fstartrowindex := (apage * amaximumrows) - amaximumrows;
	END IF;

	OPEN rc FOR
	SELECT id
		, email
		, first_name
		, last_name
		, middle_name
		, pwd
		, gender
		, birthdate
		, db.utl_masked_phone(phone) as phone
		, email_confirmed
		, security_stamp
		, lockout_end_date
		, lockout_enabled
		, access_failed_count
		, lastupdatedatetime
		, user_name
		, api
		, backend
	FROM db.sec_user
	WHERE (last_name ILIKE '%'|| fsearch_str ||'%' 
		OR first_name ILIKE '%'|| fsearch_str ||'%' 
		OR email ILIKE '%'|| fsearch_str ||'%')
	ORDER BY last_name, first_name, email OFFSET fstartrowindex LIMIT amaximumrows;
	RETURN NEXT rc;

	OPEN counts FOR
	SELECT COUNT(id)
	FROM db.sec_user
	WHERE (last_name ILIKE '%'|| fsearch_str ||'%' 
		OR first_name ILIKE '%'|| fsearch_str ||'%' 
		OR email ILIKE '%'|| fsearch_str ||'%');
	RETURN NEXT counts;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;


REVOKE ALL ON FUNCTION db.sec_user_select (
  apage INTEGER
, amaximumrows INTEGER
, asearch_str VARCHAR(30)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_select (
  apage INTEGER
, amaximumrows INTEGER
, asearch_str VARCHAR(30)
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_user_select (
  apage INTEGER
, amaximumrows INTEGER
, asearch_str VARCHAR(30)
) TO apiuser;