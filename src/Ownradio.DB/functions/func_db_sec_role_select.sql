CREATE OR REPLACE FUNCTION db.sec_role_select (
  apage INTEGER
, amaximumrows INTEGER
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : select, with parameters page_num, page_size - select the paged list of entities
	-- Version : 1.0.0

DECLARE
	fstartrowindex INTEGER;
	rc REFCURSOR;
	counts REFCURSOR;
BEGIN
	IF apage = 0 THEN
		fstartrowindex := 0;
		amaximumrows := 0;
	ELSE 
		fstartrowindex := (apage * amaximumrows) - amaximumrows;
	END IF;

	OPEN rc FOR
	SELECT id
		, name
		,lastupdatedatetime
	FROM db.sec_role
	ORDER BY name OFFSET fstartrowindex LIMIT amaximumrows;
	RETURN NEXT rc;

	OPEN counts FOR
	SELECT COUNT(id)
	FROM db.sec_role;
	RETURN NEXT counts;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_select (
  apage INTEGER
, amaximumrows INTEGER
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_select (
  apage INTEGER
, amaximumrows INTEGER
) TO writer;

GRANT EXECUTE ON FUNCTION db.sec_role_select (
  apage INTEGER
, amaximumrows INTEGER
) TO apiuser;