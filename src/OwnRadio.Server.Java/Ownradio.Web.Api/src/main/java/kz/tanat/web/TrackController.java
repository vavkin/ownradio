package kz.tanat.web;

import kz.tanat.domain.Track;
import kz.tanat.service.TrackService;
import kz.tanat.util.ResourceFile;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

@RestController
@RequestMapping(value = "/tracks")
public class TrackController {

    private final TrackService trackService;

    @Autowired
    public TrackController(TrackService trackService) {
        this.trackService = trackService;
    }

    @RequestMapping(method = RequestMethod.POST)
    public ResponseEntity save(@RequestParam MultipartFile file, Track track) {
        if (file.isEmpty()) {
            return new ResponseEntity(HttpStatus.BAD_REQUEST);
        }

        try {
            trackService.save(track, file);

            return new ResponseEntity(HttpStatus.CREATED);
        } catch (Exception e) {
            return new ResponseEntity(HttpStatus.INTERNAL_SERVER_ERROR);
        }

    }

    @RequestMapping(value = "/{id}", method = RequestMethod.GET)
    public ResponseEntity<?> getTrack(@PathVariable String id) {
        Track track = trackService.getById(id);

        if (track != null) {
            byte[] bytes = ResourceFile.read(track.getPath());
            return new ResponseEntity<>(bytes, HttpStatus.OK);
        } else {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @RequestMapping(value = "/{userId}/random", method = RequestMethod.GET)
    public ResponseEntity<?> getRandomTrack(@PathVariable String userId) {
        String trackId = trackService.getRandomByUserId(userId);

        if (trackId != null && !trackId.isEmpty()) {
            return new ResponseEntity<>(trackId, HttpStatus.OK);
        } else {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

}
