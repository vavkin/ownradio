package kz.tanat.domain;


import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.springframework.format.annotation.DateTimeFormat;

import javax.persistence.Column;
import javax.persistence.Entity;
import java.util.Date;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Entity
public class History extends AbstractEntity {

    @Column(nullable = false)
    private String userId;

    @Column(nullable = false)
    private String trackId;

    @DateTimeFormat(pattern = "dd/MM/yyyy")
    @Column(nullable = false)
    private Date lastListen;

    @Column(nullable = false)
    private boolean listen;

}
