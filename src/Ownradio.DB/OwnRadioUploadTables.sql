-- Database: "ownRadio"

-- DROP DATABASE "ownRadio";

CREATE DATABASE "ownRadio"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Регистрируем расширение uuid-ossp - генератор гуидов
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------

-- Table: ownuser

-- DROP TABLE ownuser;

CREATE TABLE ownuser
(
  id uuid NOT NULL,
  username character varying(100) NOT NULL,
  CONSTRAINT pk_users PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE ownuser
  OWNER TO postgres;
  
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Table: device

-- DROP TABLE device;

CREATE TABLE device
(
  id uuid NOT NULL,
  userid uuid,
  devicename character varying(100),
  CONSTRAINT pk_device PRIMARY KEY (id),
  CONSTRAINT fk_user FOREIGN KEY (userid)
      REFERENCES ownuser (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.device
  OWNER TO postgres;

-- Index: fki_user

-- DROP INDEX fki_user;

CREATE INDEX fki_user
  ON device
  USING btree
  (userid);

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Table: track

-- DROP TABLE track;

CREATE TABLE track
(
  id uuid NOT NULL,
  uploaduserid uuid NOT NULL,
  localdevicepathupload character varying(2048),
  path character varying(2048),
  uploaddatetime timestamp with time zone NOT NULL DEFAULT now(),
  CONSTRAINT pk_track PRIMARY KEY (id),
  CONSTRAINT fk_track_user FOREIGN KEY (uploaduserid)
      REFERENCES ownuser (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE track
  OWNER TO postgres;

-- Index: fki_track_user

-- DROP INDEX fki_track_user;

CREATE INDEX fki_track_user
  ON track
  USING btree
  (uploaduserid);
  
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Table: history

-- DROP TABLE history;

CREATE TABLE history
(
  id uuid NOT NULL DEFAULT uuid_generate_v4(),
  userid uuid NOT NULL,
  trackid uuid NOT NULL,
  listendatetime timestamp with time zone NOT NULL DEFAULT now(),
  islisten integer,
  method character varying(50),
  CONSTRAINT pk_history PRIMARY KEY (id),
  CONSTRAINT fk_history_track FOREIGN KEY (trackid)
      REFERENCES track (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_history_user FOREIGN KEY (userid)
      REFERENCES ownuser (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE history
  OWNER TO postgres;

-- Index: fki_history_track

-- DROP INDEX fki_history_track;

CREATE INDEX fki_history_track
  ON history
  USING btree
  (trackid);

-- Index: fki_history_user

-- DROP INDEX fki_history_user;

CREATE INDEX fki_history_user
  ON history
  USING btree
  (userid);
  
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Table: rating

-- DROP TABLE rating;

CREATE TABLE rating
(
  id uuid NOT NULL DEFAULT uuid_generate_v4(),
  userid uuid NOT NULL,
  trackid uuid NOT NULL,
  lastlistendatetime timestamp with time zone NOT NULL DEFAULT now(),
  rating integer NOT NULL DEFAULT 0,
  CONSTRAINT pk_rating PRIMARY KEY (id),
  CONSTRAINT fk_rating_track FOREIGN KEY (trackid)
      REFERENCES track (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_rating_user FOREIGN KEY (userid)
      REFERENCES ownuser (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE rating
  OWNER TO postgres;
COMMENT ON TABLE rating
  IS 'Рейтинг';

-- Index: fki_rating_track

-- DROP INDEX fki_rating_track;

CREATE INDEX fki_rating_track
  ON rating
  USING btree
  (trackid);

-- Index: fki_rating_user

-- DROP INDEX fki_rating_user;

CREATE INDEX fki_rating_user
  ON public.rating
  USING btree
  (userid);

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
-- Function: registertrack(uuid, character varying, character varying, uuid)

-- DROP FUNCTION registertrack(uuid, character varying, character varying, uuid);

CREATE OR REPLACE FUNCTION registertrack(
    trackid uuid,
    localdevicepathupload character varying,
    path character varying,
    userid uuid)
  RETURNS void AS
$BODY$
-- 
-- Функция добавляет запись о треке в таблицу треков и делает сопутствующие записи в
-- таблицу статистики прослушивания и рейтингов. Если пользователя, загружающего трек 
-- нет в базе, то он добавляется в таблицу пользователей.
--

-- Добавляем пользователя, если его еще не существует
INSERT INTO ownuser (id,username)
SELECT userid, 'Anonymous new user'
WHERE NOT EXISTS(SELECT * FROM ownuser WHERE id=userid);

-- Добавляем трек в базу данных
INSERT INTO track(id, localdevicepathupload, path, uploaduserid) 
VALUES(trackid, localdevicepathupload, path, userid);

-- Добавляем запись о прослушивании трека в таблицу истории прослушивания
INSERT INTO history(userid,trackid)
VALUES(userid,trackid);

-- Добавляем запись в таблицу рейтингов
INSERT INTO rating(userid, trackid, rating)
VALUES(userid, trackid, 1);

$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION registertrack(uuid, character varying, character varying, uuid)
  OWNER TO postgres;

----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
-- Function: getnexttrackid(uuid)

-- DROP FUNCTION getnexttrackid(uuid);

CREATE OR REPLACE FUNCTION getnexttrackid(deviceid uuid)
  RETURNS uuid AS
$BODY$

-- get next track id for user by device 

SELECT id 
	FROM track 
	ORDER BY RANDOM()
	LIMIT 1

$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION getnexttrackid(uuid)
  OWNER TO postgres;
----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
-- Function: setstatustrack(uuid, uuid, integer, timestamp with time zone)

-- DROP FUNCTION setstatustrack(uuid, uuid, integer, timestamp with time zone);

CREATE OR REPLACE FUNCTION setstatustrack(
    deviceid uuid,
    trackid uuid,
    islisten integer,
    datetimelisten timestamp with time zone)
  RETURNS void AS
$BODY$
-- Устанавливает статус прослушивания трека: добавляет запись в таблицу статистики и обновляет рейтинг

INSERT INTO history(userid, trackid, listendatetime, islisten)
VALUES((SELECT userid FROM device WHERE id=deviceid LIMIT 1), trackid, datetimelisten, islisten);

UPDATE public.rating SET rating=rating+islisten,lastlistendatetime=datetimelisten WHERE trackid=trackid;

$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION setstatustrack(uuid, uuid, integer, timestamp with time zone)
  OWNER TO postgres;

----------------------------------------------------------------------------------------	
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------	
 
INSERT INTO ownuser (id, username) VALUES ('12345678-1234-1234-1234-123456789012', 'Test User');
INSERT INTO device (id, userid, devicename) VALUES ('00000000-0000-0000-0000-000000000000', '12345678-1234-1234-1234-123456789012', 'TEST-USER-PC');
