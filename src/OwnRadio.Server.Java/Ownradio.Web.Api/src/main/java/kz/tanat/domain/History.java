package kz.tanat.domain;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.format.annotation.DateTimeFormat;

import javax.persistence.*;
import java.util.Calendar;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Entity
public class History {
    @Id
    @GeneratedValue(generator = "uuid")
    @GenericGenerator(name = "uuid", strategy = "uuid2")
    @Column(name = "ID", unique = true)
    private String id;

    @Column(name = "UserID", nullable = false)
    private String userId;

    @Column(name = "TrackID", nullable = false)
    private String trackId;

    @DateTimeFormat(pattern = "MM/dd/yyyy")
    @Column(name = "LastListenDateTime", nullable = false)
    private Calendar lastListenDateTime;

    @Column(name = "ListenYesNo")
    private boolean listenYesNo;

}
