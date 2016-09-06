CREATE TABLE db.sec_role_description (
 id SERIAL NOT NULL,
 role_id VARCHAR(128) NOT NULL,
 name VARCHAR(256) NOT NULL,
UNIQUE (role_id),
PRIMARY KEY(id));

COMMENT ON TABLE db.sec_role_description IS 'Role description';
COMMENT ON COLUMN db.sec_role_description.id IS 'The unique identifier';
COMMENT ON COLUMN db.sec_role_description.role_id IS 'Identifies the role';
COMMENT ON COLUMN db.sec_role_description.name IS 'Name of role';
