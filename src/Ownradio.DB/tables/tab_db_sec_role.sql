CREATE TABLE db.sec_role (
 id VARCHAR(128) NOT NULL,
 name VARCHAR(256),
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
CONSTRAINT sec_role_name_key UNIQUE(name),
PRIMARY KEY(id));

COMMENT ON TABLE db.sec_role IS 'Roles';
COMMENT ON COLUMN db.sec_role.id IS 'The unique identifier';
COMMENT ON COLUMN db.sec_role.name IS 'The name of the role (admin, ...)';
COMMENT ON COLUMN db.sec_role.lastupdatedatetime IS 'Timestamp of the last update';