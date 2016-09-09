CREATE OR REPLACE FUNCTION db.sec_user_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) RETURNS VARCHAR(128) AS
$$
	-- Authors : Alexey Serov
	-- Created : 07/09/2016
	-- Purpose : delete record of the db.sec_user
	-- Version : 1.0.0

DECLARE
	fid VARCHAR(128);
	fcount INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);

	DELETE FROM db.sec_user 
	WHERE id = aid AND lastupdatedatetime = alastupdatedatetime
	RETURNING id INTO fid;

	RETURN fid;
EXCEPTION
	WHEN foreign_key_violation THEN
		IF strpos(SQLERRM, '"cnt_music_ratio_uid_fk"') != 0  THEN
			SELECT count(*)
			INTO fcount
			FROM db.cnt_music_ratio
			WHERE user_id = aid LIMIT 1;
			RAISE 'Невозможно удалить пользователя. С пользователем связано {%} песен.', fcount;
		ELSIF strpos(SQLERRM, '"cnt_music_history_uid_fk"') != 0  THEN
			SELECT count(*)
			INTO fcount
			FROM db.cnt_music_history
			WHERE user_id = aid LIMIT 1;
			RAISE 'Невозможно удалить пользователя. С пользователем связано {%} оценок песен.', fcount;
		END IF;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_user_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_user_delete (
  aid VARCHAR(128)
, alastupdatedatetime TIMESTAMP
, auserapp VARCHAR(128)
) TO writer;