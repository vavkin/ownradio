package kz.tanat.repository;

import kz.tanat.domain.Track;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface TrackRepository extends JpaRepository<Track, String> {
    @Query("select t from Track t where t.uploadUserId = :userId order by random()")
    List<Track> getRandomTrackByUserId(@Param("userId") String userId, Pageable pageable);
}
