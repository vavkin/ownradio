CREATE OR REPLACE FUNCTION db.sec_device_delete (
  auserID VARCHAR(128) 
, adeviceID VARCHAR(128)
, auserapp VARCHAR(128)
) RETURNS INTEGER AS
$$
	-- Authors : Alexey Serov
	-- Created : 08/09/2016
	-- Purpose : Delete device from table
	-- Version : 1.0.0

DECLARE
	fdevice_id VARCHAR(128);
	fcount INTEGER;
BEGIN
	PERFORM db.set_app_user(auserapp);

	SELECT device_id
	INTO fdevice_id
	FROM db.sec_device
	WHERE device_id = adeviceID;

	IF coalesce(fdevice_id, '') = '' THEN
		RETURN 0;
	ELSE
		DELETE FROM db.sec_device d
		WHERE d.user_id = auserID
			AND d.device_id = fdevice_id;
		RETURN 1;
	END IF;
EXCEPTION
	WHEN foreign_key_violation THEN
		IF strpos(SQLERRM, '"cnt_music_ratio_did_fk"') != 0  THEN
			SELECT count(*)
			INTO fcount
			FROM db.cnt_music_ratio
			WHERE user_id = aid LIMIT 1;
			RAISE 'Невозможно удалить устройство. С устройством связано {%} песен.', fcount;
		ELSIF strpos(SQLERRM, '"cnt_music_history_did_fk"') != 0  THEN
			SELECT count(*)
			INTO fcount
			FROM db.cnt_music_history
			WHERE user_id = aid LIMIT 1;
			RAISE 'Невозможно удалить устройство. С устройством связано {%} оценок песен.', fcount;
		END IF;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.sec_device_delete (
  auserID VARCHAR(128) 
, adeviceID VARCHAR(128)
, auserapp VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.sec_device_delete (
  auserID VARCHAR(128) 
, adeviceID VARCHAR(128)
, auserapp VARCHAR(128)
) TO writer;