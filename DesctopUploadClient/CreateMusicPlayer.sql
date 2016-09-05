-- Database: musicplayer

-- DROP DATABASE musicplayer;

CREATE DATABASE musicplayer
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Table: public."user"

-- DROP TABLE public."user";

CREATE TABLE public."user"
(
    id uuid NOT NULL,
    name character varying(100) COLLATE "default".pg_catalog NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."user"
    OWNER to postgres;
    
-- Table: public.device

-- DROP TABLE public.device;

CREATE TABLE public.device
(
    id uuid NOT NULL,
    userid uuid,
    name character varying(100) COLLATE "default".pg_catalog,
    CONSTRAINT pk_device PRIMARY KEY (id),
    CONSTRAINT fk_user FOREIGN KEY (userid)
        REFERENCES public."user" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.device
    OWNER to postgres;

COMMENT ON COLUMN public.device.id
    IS 'Device ID';

-- Index: fki_user

-- DROP INDEX public.fki_user;

CREATE INDEX fki_user
    ON public.device USING btree
    (userid)
    TABLESPACE pg_default;

-- Table: public.track

-- DROP TABLE public.track;

CREATE TABLE public.track
(
    id uuid NOT NULL,
    userid uuid NOT NULL,
    name character varying(2048) COLLATE "default".pg_catalog,
    path character varying(2048) COLLATE "default".pg_catalog,
    CONSTRAINT pk_track PRIMARY KEY (id),
    CONSTRAINT fk_track_user FOREIGN KEY (userid)
        REFERENCES public."user" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.track
    OWNER to postgres;
COMMENT ON TABLE public.track
    IS 'Музыкальный файл';

COMMENT ON COLUMN public.track.id
    IS 'идентификатор';

COMMENT ON COLUMN public.track.userid
    IS 'Загрузивший пользователь';

COMMENT ON COLUMN public.track.name
    IS 'имя файла';

COMMENT ON COLUMN public.track.path
    IS 'путь на ПК пользователя';

-- Index: fki_track_user

-- DROP INDEX public.fki_track_user;

CREATE INDEX fki_track_user
    ON public.track USING btree
    (userid)
    TABLESPACE pg_default;


INSERT INTO public.user (id, name) VALUES ('ce0cc444-9bb6-4092-a6f0-a7838ad98000', 'panchenkodp');
INSERT INTO public.device (id, userid, name) VALUES ('ce0cc444-9bb6-4092-a6f0-a7838ad98221', 'ce0cc444-9bb6-4092-a6f0-a7838ad98000', 'panchenkodp-pc');