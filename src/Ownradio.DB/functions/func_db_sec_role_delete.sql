CREATE OR REPLACE FUNCTION db.sec_role_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) RETURNS VARCHAR(128) AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : delete record of the db.sec_role
	-- Version : 1.0.0

DECLARE
	fid VARCHAR(128);
	ftitle VARCHAR(256);
	fcount INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);

	DELETE FROM db.sec_role 
	WHERE id = aid AND lastupdatedatetime =  alastupdatedatetime
	RETURNING id INTO fid;

	RETURN fid;
EXCEPTION
	WHEN foreign_key_violation THEN
		IF strpos(SQLERRM, '"sec_role_description_rid_fk"') != 0  THEN
			SELECT name, count(*) over()
			INTO ftitle, fcount
			FROM db.sec_role_description
			WHERE role_id = aid LIMIT 1;
			RAISE 'Невозможно удалить роль. Для роли есть описание "{%}". (с ролью связано {%} описаний роли).', ftitle, fcount;
		ELSIF strpos(SQLERRM, '"sec_user_role_rid_fk"') != 0  THEN
			SELECT count(*)
			INTO fcount
			FROM db.sec_user_role
			WHERE role_id  = aid LIMIT 1;
			RAISE 'Невозможно удалить роль. (с ролью связано {%} отношений с пользователем.)', fcount;
		END IF;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_role_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_role_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) TO writer;