package kz.tanat.service;

import kz.tanat.domain.Track;
import kz.tanat.repository.TrackRepository;
import kz.tanat.util.ResourceFile;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.StringUtils;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

@Service
public class TrackService {

    private final TrackRepository trackRepository;

    @Autowired
    public TrackService(TrackRepository trackRepository) {
        this.trackRepository = trackRepository;
    }

    @Transactional(readOnly = true)
    public Track getById(String id) {
        return trackRepository.findOne(id);
    }

    @Transactional(readOnly = true)
    public String getRandomByUserId(String userId) {
        List<Track> tracks = trackRepository.getRandomTrackByUserId(userId, new PageRequest(0, 1));

        if (tracks.isEmpty()) {
            return null;
        }

        // HACK: Возвращаем одну запись
        return tracks.get(0).getId();
    }

    @Transactional
    public void save(Track track, MultipartFile file) {
        trackRepository.save(track);

        String dirName = track.getUploadUserId();
        String fileName = track.getId() + "." + StringUtils.getFilenameExtension(file.getOriginalFilename());
        String filePath = ResourceFile.save(dirName, fileName, file);

        track.setPath(filePath);
        trackRepository.flush();
    }
}
