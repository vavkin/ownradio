CREATE TABLE db.sec_user_claims (
  id SERIAL  NOT NULL,
  user_id VARCHAR(128),
  claim_type VARCHAR(4000),
  claim_value VARCHAR(4000),
  expiry_date TIMESTAMP,
PRIMARY KEY(id));

COMMENT ON TABLE db.sec_user_claims IS 'User claims';
COMMENT ON COLUMN db.sec_user_claims.id IS 'The unique identifier';
COMMENT ON COLUMN db.sec_user_claims.user_id IS 'Identifies the user';
COMMENT ON COLUMN db.sec_user_claims.claim_type IS 'The type of the claim';
COMMENT ON COLUMN db.sec_user_claims.claim_value IS 'The value of the claim';
COMMENT ON COLUMN db.sec_user_claims.expiry_date IS 'The date and time, when the role expires';
