ALTER TABLE db.sec_role_description ADD CONSTRAINT sec_role_description_rid_FK FOREIGN KEY (role_id) REFERENCES db.sec_role(id);
ALTER TABLE db.sec_user_claims ADD CONSTRAINT sec_user_claims_uid_FK FOREIGN KEY (user_id) REFERENCES db.sec_user(id) ON DELETE CASCADE;
ALTER TABLE db.sec_user_role ADD CONSTRAINT sec_user_role_uid_FK FOREIGN KEY (user_id) REFERENCES db.sec_user(id) ON DELETE CASCADE;
ALTER TABLE db.sec_user_role ADD CONSTRAINT sec_user_role_rid_FK FOREIGN KEY (role_id) REFERENCES db.sec_role(id);
ALTER TABLE db.cnt_music_ratio ADD CONSTRAINT cnt_music_ratio_mid_FK FOREIGN KEY (music_id) REFERENCES db.cnt_music(id);
ALTER TABLE db.cnt_music_ratio ADD CONSTRAINT cnt_music_ratio_uid_FK FOREIGN KEY (user_id) REFERENCES db.sec_user(id) ON DELETE CASCADE;
ALTER TABLE db.cnt_music_history ADD CONSTRAINT cnt_music_history_mid_FK FOREIGN KEY (music_id) REFERENCES db.cnt_music(id);
ALTER TABLE db.cnt_music_history ADD CONSTRAINT cnt_music_history_uid_FK FOREIGN KEY (user_id) REFERENCES db.sec_user(id) ON DELETE CASCADE;
