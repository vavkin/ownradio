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
  "ID" uuid NOT NULL,
  "Name" character varying(100) NOT NULL,
  CONSTRAINT "PK_Users" PRIMARY KEY ("ID")
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
  "ID" uuid NOT NULL, -- Device ID
  "UserID" uuid,
  "Name" character varying(100),
  CONSTRAINT "PK_Device" PRIMARY KEY ("ID"),
  CONSTRAINT "FK_User" FOREIGN KEY ("UserID")
      REFERENCES public."User" ("ID") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Device"
  OWNER TO postgres;
COMMENT ON COLUMN public."Device"."ID" IS 'Device ID';


-- Index: public.fki_user

-- DROP INDEX public.fki_user;

CREATE INDEX "FKI_User"
  ON public."Device"
  USING btree
  ("UserID");

-- Table: public."Track"

-- DROP TABLE public."Track";

CREATE TABLE public."Track"
(
  "ID" uuid NOT NULL, -- идентификатор
  "UserID" uuid NOT NULL, -- Загрузивший пользователь
  "Name" character varying(2048), -- имя файла
  "Path" character varying(2048), -- путь на ПК пользователя
  CONSTRAINT "PK_Track" PRIMARY KEY ("ID"),
  CONSTRAINT "FK_Track_User" FOREIGN KEY ("UserID")
      REFERENCES public."User" ("ID") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Track"
  OWNER TO postgres;
COMMENT ON TABLE public."Track"
  IS 'Музыкальный файл';
COMMENT ON COLUMN public."Track"."ID" IS 'идентификатор';
COMMENT ON COLUMN public."Track"."UserID" IS 'Загрузивший пользователь';
COMMENT ON COLUMN public."Track"."Name" IS 'имя файла';
COMMENT ON COLUMN public."Track"."Path" IS 'путь на ПК пользователя';


-- Index: public.fki_track_user

-- DROP INDEX public.fki_track_user;

CREATE INDEX "FKI_Track_User"
  ON public."Track"
  USING btree
  ("UserID");

-- Function: public.registerfile(uuid, character varying, character varying, uuid)

-- DROP FUNCTION public.registerfile(uuid, character varying, character varying, uuid);

CREATE OR REPLACE FUNCTION public.registerfile(
    "ID" uuid,
    "FileName" character varying,
    "Path" character varying,
    "UserID" uuid)
  RETURNS void AS
$BODY$INSERT INTO public."Track"("ID", "UserID", "Name", "Path") 
VALUES($1, $4, $2, $3);$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION public.registerfile(uuid, character varying, character varying, uuid)
  OWNER TO postgres;
  
INSERT INTO public."User" ("ID", "Name") VALUES ('12345678-1234-1234-1234-123456789012', 'Test User');
INSERT INTO public."Device" ("ID", "UserID", "Name") VALUES ('00000000-0000-0000-0000-000000000000', '12345678-1234-1234-1234-123456789012', 'TEST-USER-PC');