CREATE TABLE db.ref_parameters (
 id SERIAL NOT NULL,
 application_id INTEGER,
 section VARCHAR(50),
 category VARCHAR(50),
 subcategory VARCHAR(50),
 item VARCHAR(50),
 is_system_defined BOOLEAN,
 is_value_localizable BOOLEAN,
 value VARCHAR(2000),
 type VARCHAR(30),
 description VARCHAR(255),
 lastupdatedatetime TIMESTAMP WITHOUT TIME ZONE DEFAULT now() NOT NULL,
 CONSTRAINT valid_type_values CHECK (type IN ('int', 'string', 'double')),
PRIMARY KEY(id));

COMMENT ON TABLE db.ref_parameters IS 'The parameters';
COMMENT ON COLUMN db.ref_parameters.id IS 'The unique identifier';
COMMENT ON COLUMN db.ref_parameters.application_id IS 'Identifies the application for parameter';
COMMENT ON COLUMN db.ref_parameters.section IS 'Section';
COMMENT ON COLUMN db.ref_parameters.category IS 'Category';
COMMENT ON COLUMN db.ref_parameters.subcategory IS 'SubCategory';
COMMENT ON COLUMN db.ref_parameters.item IS 'Item';
COMMENT ON COLUMN db.ref_parameters.is_system_defined IS 'Is system defined';
COMMENT ON COLUMN db.ref_parameters.is_value_localizable IS 'Is value localizable';
COMMENT ON COLUMN db.ref_parameters.value IS 'The value for the parameter';
COMMENT ON COLUMN db.ref_parameters.type IS 'The type of the parameter value';
COMMENT ON COLUMN db.ref_parameters.description IS 'Description';
COMMENT ON COLUMN db.ref_parameters.lastupdatedatetime IS 'Timestamp of the last update';