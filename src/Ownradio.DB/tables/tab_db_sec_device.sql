CREATE TABLE db.sec_device (
  device_id VARCHAR(128) NOT NULL
, user_id VARCHAR(128) NOT NULL
, name varchar(256) NOT NULL
, description varchar(1024)
, is_active BOOLEAN DEFAULT true NOT NULL
, lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL
, PRIMARY KEY(device_id));

COMMENT ON TABLE db.sec_device IS 'The devices';
COMMENT ON COLUMN db.sec_device.device_id IS 'The unique identifier of device';
COMMENT ON COLUMN db.sec_device.user_id IS 'Identifies the user';
COMMENT ON COLUMN db.sec_device.name IS 'Device name';
COMMENT ON COLUMN db.sec_device.description IS 'Device additional description';
COMMENT ON COLUMN db.sec_device.is_active IS 'Is active';
COMMENT ON COLUMN db.sec_device.lastupdatedatetime IS 'Timestamp of the last update';