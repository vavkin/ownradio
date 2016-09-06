CREATE TABLE db.sec_user (
 id VARCHAR(128) NOT NULL,
 email VARCHAR(256) NULL,
 first_name VARCHAR(30),
 last_name VARCHAR(30),
 middle_name VARCHAR(30),
 pwd TEXT,
 gender CHAR(1),
 birthdate TIMESTAMP,
 phone VARCHAR(50),
 email_confirmed BOOLEAN NOT NULL,
 security_stamp TEXT NOT NULL,
 lockout_end_date TIMESTAMP,
 lockout_enabled BOOLEAN NOT NULL,
 access_failed_count INTEGER NOT NULL,
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
 user_name VARCHAR(256) NOT NULL,
 api BOOLEAN DEFAULT false,
 backend BOOLEAN DEFAULT false,
 CONSTRAINT sec_user_user_name_api_backend_key UNIQUE(user_name, api, backend),
PRIMARY KEY(id));

COMMENT ON TABLE db.sec_user IS 'Users';
COMMENT ON COLUMN db.sec_user.id IS 'The unique identifier';
COMMENT ON COLUMN db.sec_user.email IS 'The email of the user';
COMMENT ON COLUMN db.sec_user.first_name IS 'First name of teh user';
COMMENT ON COLUMN db.sec_user.last_name IS 'Last name of teh user';
COMMENT ON COLUMN db.sec_user.middle_name IS 'Middle name if the user';
COMMENT ON COLUMN db.sec_user.pwd IS 'Hashed password';
COMMENT ON COLUMN db.sec_user.gender IS 'The gender of the user';
COMMENT ON COLUMN db.sec_user.birthdate IS 'The date of the user"s birth';
COMMENT ON COLUMN db.sec_user.phone IS 'PhoneNumber for the user';
COMMENT ON COLUMN db.sec_user.email_confirmed IS 'The gender of the user';
COMMENT ON COLUMN db.sec_user.security_stamp IS 'A random value that should change whenever a users credentials have changed (password changed, login removed)';
COMMENT ON COLUMN db.sec_user.lockout_end_date IS 'When the user lockout ends, any time in the past is considered not locked out';
COMMENT ON COLUMN db.sec_user.lockout_enabled IS 'Is lockout enabled for this user';
COMMENT ON COLUMN db.sec_user.access_failed_count IS 'Used to record failures for the purposes of lockout';
COMMENT ON COLUMN db.sec_user.lastupdatedatetime IS 'Timestamp of the last update';
COMMENT ON COLUMN db.sec_user.user_name IS 'The user name of the user';
COMMENT ON COLUMN db.sec_user.api IS 'Indicates mobile registration (registered or not)';
COMMENT ON COLUMN db.sec_user.backend IS 'Indicates Back-office registration (registered or not)';
