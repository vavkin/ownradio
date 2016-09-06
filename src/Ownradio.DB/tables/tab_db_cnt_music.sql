CREATE TABLE db.cnt_music ( 
 id SERIAL,
 filename varchar(2048),
 title varchar(30),
 artist varchar(30),
 album varchar(30),
 track_year int,
 track_comment varchar(30),
 track_number int,
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
 CONSTRAINT music_filename_unique UNIQUE (filename),
PRIMARY KEY(id));

COMMENT ON TABLE db.cnt_music IS 'Music tracks';
COMMENT ON COLUMN db.cnt_music.id IS 'The unique identifier';
COMMENT ON COLUMN db.cnt_music.filename IS 'File fullpath on server for download';
COMMENT ON COLUMN db.cnt_music.title IS 'Track title';
COMMENT ON COLUMN db.cnt_music.artist IS 'Artist name';
COMMENT ON COLUMN db.cnt_music.album IS 'Album name';
COMMENT ON COLUMN db.cnt_music.track_year IS 'Track year';
COMMENT ON COLUMN db.cnt_music.track_comment IS 'Comment';
COMMENT ON COLUMN db.cnt_music.track_number IS 'Number of a track in an album';
COMMENT ON COLUMN db.cnt_music.lastupdatedatetime IS 'Last modifications date and time';
