-- FK
CREATE INDEX ind_fk_sec_role_description_role_id ON db.sec_role_description (role_id) tablespace musicdb_index;
CREATE INDEX ind_fk_sec_user_role_user_id ON db.sec_user_role (user_id) tablespace musicdb_index;
CREATE INDEX ind_fk_sec_user_role_role_id ON db.sec_user_role (role_id) tablespace musicdb_index;
CREATE INDEX ind_fk_sec_user_claims_user_id ON db.sec_user_claims (user_id) tablespace musicdb_index;
CREATE INDEX ind_fk_cnt_music_ratio_music_id ON db.cnt_music_ratio (music_id) tablespace musicdb_index;
CREATE INDEX ind_fk_cnt_music_ratio_user_id ON db.cnt_music_ratio (user_id) tablespace musicdb_index;
CREATE INDEX ind_fk_cnt_music_history_music_id ON db.cnt_music_history (music_id) tablespace musicdb_index;
CREATE INDEX ind_fk_cnt_music_history_user_id ON db.cnt_music_history (user_id) tablespace musicdb_index;

-- Pagination 
-- : order by 
CREATE INDEX ind_cnt_music_filename ON db.cnt_music (filename) tablespace musicdb_index;
CREATE INDEX ind_cnt_music_ratio_ratio ON db.cnt_music_ratio (ratio) tablespace musicdb_index;

-- : where

-- Application
