package kz.tanat.repository;

import kz.tanat.domain.History;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.autoconfigure.orm.jpa.TestEntityManager;
import org.springframework.test.context.junit4.SpringRunner;

import java.util.Date;

import static org.hamcrest.CoreMatchers.nullValue;
import static org.hamcrest.Matchers.is;
import static org.hamcrest.Matchers.not;
import static org.junit.Assert.assertThat;

@RunWith(SpringRunner.class)
@DataJpaTest
public class HistoryRepositoryTest {
    @Autowired
    private HistoryRepository historyRepository;

    @Autowired
    private TestEntityManager entityManager;

    private History history;

    @Before
    public void setUp() throws Exception {
        history = new History("1", "1", new Date(), true);
        historyRepository.saveAndFlush(history);
    }

    @Test
    public void createdAt() throws Exception {
        assertThat(history.getCreatedAt(), not(nullValue()));
        assertThat(history.getCreatedAt().toString(), is(new Date().toString()));
    }

    @Test
    public void updatedAt() throws Exception {
        assertThat(history.getCreatedAt(), not(nullValue()));
        assertThat(history.getCreatedAt().toString(), is(new Date().toString()));
        assertThat(history.getUpdatedAt(), is(nullValue()));

        History storeHistory = historyRepository.findOne(history.getId());
        storeHistory.setListen(false);
        historyRepository.saveAndFlush(storeHistory);

        assertThat(storeHistory.getCreatedAt(), not(nullValue()));
        assertThat(storeHistory.getCreatedAt().toString(), is(history.getCreatedAt().toString()));

        assertThat(storeHistory.getUpdatedAt(), not(nullValue()));
        assertThat(storeHistory.getUpdatedAt().toString(), is(new Date().toString()));
    }


}