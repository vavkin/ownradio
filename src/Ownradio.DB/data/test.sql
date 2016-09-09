        SELECT db.sec_user_insert(aid := '9d359659-5807-4fd8-90ce-f6f3091ca56d',
                                  aemail := 'test@test.ru',
                                  afirst_name := 'test',
                                  alast_name := 'test',
                                  amiddle_name := 'test',
                                  apwd := 'test',
                                  agender := 'M',
                                  abirthdate := CURRENT_DATE,
                                  aphone := 'test',
                                  aemail_confirmed := false,
                                  asecurity_stamp := 'test',
                                  alockout_end_date := CURRENT_DATE,
                                  alockout_enabled := false,
                                  aaccess_failed_count := 0,
                                  auser_name := 'test',
                                  aapi := false,
                                  abackend := false,
                                  auserapp := 'ut');
								  
        SELECT db.sec_role_insert(aid := 'id test',
                                  aname := 'test',
                                  auserapp := 'ut');

        SELECT db.sec_user_claims_insert(auser_id := '9d359659-5807-4fd8-90ce-f6f3091ca56d',
                                         aclaim_type := 'test',
                                         aclaim_value := 'test',
                                         aexpiry_date := CURRENT_DATE,
                                         auserapp := 'ut');

        SELECT db.sec_user_role_insert(auser_id := '9d359659-5807-4fd8-90ce-f6f3091ca56d',
                                       arole_name := 'test',
                                       auserapp := 'ut');

        SELECT db.sec_device_insert(auser_id := '9d359659-5807-4fd8-90ce-f6f3091ca56d',
                                    adeviceID := '0d000000-0000-0fd0-00ce-f0f0000ca00d',
									adeviceName := 'test_laptop',
									adeviceDescription := 'dell lattitude',
                                    auserapp := 'ut');
