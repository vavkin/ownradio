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

-- Table: public."User"

-- DROP TABLE public."User";

CREATE TABLE public."User"
(
  id uuid NOT NULL,
  name character varying(100) NOT NULL,
  CONSTRAINT pk_users PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."User"
  OWNER TO postgres;

  -- Table: public."Device"

-- DROP TABLE public."Device";

CREATE TABLE public."Device"
(
  id uuid NOT NULL, -- Device ID
  "userId" uuid,
  name character varying(100),
  CONSTRAINT pk_device PRIMARY KEY (id),
  CONSTRAINT fk_user FOREIGN KEY ("userId")
      REFERENCES public."User" (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Device"
  OWNER TO postgres;
COMMENT ON COLUMN public."Device".id IS 'Device ID';


-- Index: public.fki_user

-- DROP INDEX public.fki_user;

CREATE INDEX fki_user
  ON public."Device"
  USING btree
  ("userId");

-- Table: public."Track"

-- DROP TABLE public."Track";

CREATE TABLE public."Track"
(
  id uuid NOT NULL, -- идентификатор
  "userId" uuid NOT NULL, -- Загрузивший пользователь
  name character varying(2048), -- имя файла
  path character varying(2048), -- путь на ПК пользователя
  CONSTRAINT pk_track PRIMARY KEY (id),
  CONSTRAINT fk_track_user FOREIGN KEY ("userId")
      REFERENCES public."User" (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Track"
  OWNER TO postgres;
COMMENT ON TABLE public."Track"
  IS 'Музыкальный файл';
COMMENT ON COLUMN public."Track".id IS 'идентификатор';
COMMENT ON COLUMN public."Track"."userId" IS 'Загрузивший пользователь';
COMMENT ON COLUMN public."Track".name IS 'имя файла';
COMMENT ON COLUMN public."Track".path IS 'путь на ПК пользователя';


-- Index: public.fki_track_user

-- DROP INDEX public.fki_track_user;

CREATE INDEX fki_track_user
  ON public."Track"
  USING btree
  ("userId");

-- Function: public.registerfile(uuid, character varying, character varying, uuid)

-- DROP FUNCTION public.registerfile(uuid, character varying, character varying, uuid);

CREATE OR REPLACE FUNCTION public.registerfile(
    id uuid,
    "fileName" character varying,
    path character varying,
    "userId" uuid)
  RETURNS void AS
$BODY$INSERT INTO public."Track"("id", "userId", "name", "path") 
VALUES($1, $4, $2, $3);$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION public.registerfile(uuid, character varying, character varying, uuid)
  OWNER TO postgres;
  
INSERT INTO public."User" (id, name) VALUES ('ce0cc444-9bb6-4092-a6f0-a7838ad98000', 'panchenkodp');
INSERT INTO public."Device" (id, "userId", name) VALUES ('ce0cc444-9bb6-4092-a6f0-a7838ad98221', 'ce0cc444-9bb6-4092-a6f0-a7838ad98000', 'panchenkodp-pc');