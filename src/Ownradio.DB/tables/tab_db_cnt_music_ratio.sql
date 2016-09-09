CREATE TABLE db.cnt_music_ratio ( 
 user_id VARCHAR(128) NOT NULL,
 device_id VARCHAR(128) NOT NULL,
 music_id INTEGER NOT NULL,
 ratio int DEFAULT 0 NOT NULL,
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
PRIMARY KEY(user_id, music_id, device_id)) WITH (oids = true);

COMMENT ON TABLE db.cnt_music_ratio IS 'Music ratio';
COMMENT ON COLUMN db.cnt_music_ratio.user_id IS 'Identifies the user';
COMMENT ON COLUMN db.cnt_music_ratio.music_id IS 'Identifies the music';
COMMENT ON COLUMN db.cnt_music_ratio.ratio IS 'A track ratio';
COMMENT ON COLUMN db.cnt_music_ratio.lastupdatedatetime IS 'Last modifications date and time';
