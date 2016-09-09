CREATE OR REPLACE FUNCTION db.cnt_music_select_user_positive (
  auser_id VARCHAR(128)
) RETURNS SETOF refcursor AS
$$
	-- Authors : Alexey Serov
	-- Created : 08/09/2016
	-- Purpose : select user music whith positive ratio 
	-- Version : 1.0.0

DECLARE
	rc REFCURSOR;
BEGIN
	OPEN rc FOR
	SELECT	r.user_id,
			m.id,
			m.filename,
			m.title,
			m.artist,
			m.album,
			m.track_year,
			m.track_comment,
			m.track_number,
			m.lastupdatedatetime,
			sum(ratio) as ratio
	FROM db.cnt_music m
		 left join cntmusic_ratio r on r.music_id = m.id
	WHERE coalesce(r.user_id, '***') in (auser_id, '***')
	group by r.user_id,
			 m.id,
			 m.filename,
			 m.title,
			 m.artist,
			 m.album,
			 m.track_year,
			 m.track_comment,
			 m.track_number,
			 m.lastupdatedatetime
	having sum(coalesce(r.ratio, 0)) >= 0;
	RETURN NEXT rc;
END;
$$ LANGUAGE plpgsql
SECURITY DEFINER;

REVOKE ALL ON FUNCTION db.cnt_music_select_user_positive (
  auser_id VARCHAR(128)
) FROM PUBLIC;

GRANT EXECUTE ON FUNCTION db.cnt_music_select_user_positive (
  auser_id VARCHAR(128)
) TO writer;
