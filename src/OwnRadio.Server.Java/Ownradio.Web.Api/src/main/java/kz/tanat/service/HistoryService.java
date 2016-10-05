package kz.tanat.service;

import kz.tanat.domain.History;
import kz.tanat.repository.HistoryRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class HistoryService {

    private final HistoryRepository historyRepository;

    @Autowired
    public HistoryService(HistoryRepository historyRepository) {
        this.historyRepository = historyRepository;
    }


    public void save(History history) {
        historyRepository.saveAndFlush(history);
    }
}
