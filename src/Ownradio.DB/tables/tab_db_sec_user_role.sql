CREATE TABLE db.sec_user_role (
 user_id VARCHAR(128) NOT NULL,
 role_id VARCHAR(128) NOT NULL,
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
PRIMARY KEY(user_id, role_id)) WITH (oids = true);

COMMENT ON TABLE db.sec_user_role IS 'Relation a user to a role';
COMMENT ON COLUMN db.sec_user_role.user_id IS 'Identifies the user';
COMMENT ON COLUMN db.sec_user_role.role_id IS 'Identifies the role';
COMMENT ON COLUMN db.sec_user_role.lastupdatedatetime IS 'Timestamp of the last update';