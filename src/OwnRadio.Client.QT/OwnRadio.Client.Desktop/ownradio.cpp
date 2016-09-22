#include "ownradio.h"
#include "ui_ownradio.h"

OwnRadio::OwnRadio(QWidget *parent) : QDialog(parent)
{
    ui.setupUi(this);
    // Убираем возможность изменять размер окна приложения
    setFixedSize(size());

    loadSettings();

    // Инициализируем объект проигрывателя
    m_player = new QMediaPlayer;
    m_player->setNotifyInterval(1000);
    //m_player->setVolume(50);
    QObject::connect(m_player, SIGNAL(positionChanged(qint64)), this, SLOT(positionUpdate(qint64)));
    QObject::connect(m_player, SIGNAL(mediaStatusChanged(QMediaPlayer::MediaStatus)), this, SLOT(mediaStatusChange(QMediaPlayer::MediaStatus)));
}

OwnRadio::~OwnRadio()
{
    delete m_player;

    saveSettings();
}

void OwnRadio::loadSettings()
{
    QSettings settings("MySoft", "OwnRadio");

    // Читаем Id устройства. При первом запуске генерируем новый Id
    m_deviceId = settings.value("DeviceId").toString();
    if (m_deviceId.isEmpty())
    {
        m_deviceId = QUuid::createUuid().toString().mid(1, 36);
        settings.setValue("DeviceId", m_deviceId);
    }

    // Читаем адрес сервиса. При первом запуске устанавливаем значение по-умолчанию
    m_serviceUrl = settings.value("ServiceUrl").toString();
    if (m_serviceUrl.isEmpty())
    {
        m_serviceUrl = "http://ownradio.ru/api";
        settings.setValue("ServiceUrl", m_serviceUrl);
    }

    // Читаем Id трека
    m_nextTrackId = settings.value("NextTrackId").toString();
}

void OwnRadio::saveSettings()
{
    QSettings settings("MySoft", "OwnRadio");

    // Сохраняем Id трека для чтения при перезапуске приложения
    settings.setValue("NextTrackId", m_nextTrackId);
}

// Обработка нажатия кнопки "Play/Pause"
void OwnRadio::on_playButton_clicked()
{
    // Если Id трека для проигрывания пустой, пытаемся получить его
    if (m_nextTrackId == "")
        getNextTrackId();

    // Кнопка "Play"
    if (m_player->state() == QMediaPlayer::StoppedState)
    {
        // Если Id трека пустой - пишем сообщение об ошибке и больше ничего не делаем
        if (m_nextTrackId == "")
        {
            ui.stateLabel->setText("stopped, error");
            return;
        }

        // Запускаем воспроизведение трека с заданным Id
        m_player->setMedia(QUrl(m_serviceUrl + "/track/GetTrackByID/" + m_nextTrackId));
        m_player->play();
        ui.playButton->setText("Pause");
        ui.stateLabel->setText("playing");
        return;
    }

    // Кнопка "Pause"
    if (m_player->state() == QMediaPlayer::PlayingState)
    {
        m_player->pause();
        ui.playButton->setText("Play");
        ui.stateLabel->setText("paused");
        return;
    }

    // Кнопка "Play", в случае когда стоим на паузе
    if (m_player->state() == QMediaPlayer::PausedState)
    {
        m_player->play();
        ui.playButton->setText("Pause");
        ui.stateLabel->setText("playing");
        return;
    }
}

// Обработка нажатия кнопки "Stop"
void OwnRadio::on_stopButton_clicked()
{
    m_player->stop();
    ui.playButton->setText("Play");
    ui.stateLabel->setText("stopped");
}

// Обработка нажатия кнопки "Next"
void OwnRadio::on_nextButton_clicked()
{
    if (m_player->state() == QMediaPlayer::PlayingState)
        playNextTrack();
}

// Обновление индикатора времени
void OwnRadio::positionUpdate(qint64 position)
{
    qint64 minutesPosition = position / 1000 / 60;
    qint64 secondsPosition = position / 1000 % 60;
    qint64 minutesDuration = m_player->duration() / 1000 / 60;
    qint64 secondsDuration = m_player->duration() / 1000 % 60;

    QString sTime = QString("%1:%2 / %3:%4")
        .arg(minutesPosition, 2, 10, QLatin1Char('0'))
        .arg(secondsPosition, 2, 10, QLatin1Char('0'))
        .arg(minutesDuration, 2, 10, QLatin1Char('0'))
        .arg(secondsDuration, 2, 10, QLatin1Char('0'));

    ui.timeLabel->setText(sTime);
}

// Обработка событий конца трека и ошибочных состояний
void OwnRadio::mediaStatusChange(QMediaPlayer::MediaStatus status)
{
    // Воспроизведение достигло конца трека, запускаем следующий трек
    if (status == QMediaPlayer::EndOfMedia)
    {
        playNextTrack();
        return;
    }

    // Ошибка
    if (status == QMediaPlayer::InvalidMedia)
    {
        ui.playButton->setText("Play");
        ui.stateLabel->setText("stopped, error");
        return;
    }
}

// Получение Id следующего трека
void OwnRadio::getNextTrackId()
{
    // Строка запроса
    QString sRequestString = m_serviceUrl + "/track/GetNextTrackID/" + m_deviceId;

    // Запрос
    QNetworkAccessManager *manager = new QNetworkAccessManager(this);
    QNetworkReply *reply = manager->get(QNetworkRequest(QUrl(sRequestString)));

    // Ожидаем завершения выполнения асинхронной операции
    QEventLoop wait;
    connect(reply, SIGNAL(finished()), &wait, SLOT(quit()));
    wait.exec();

    // Убираем кавычки в конце и в начале полученной строки
    m_nextTrackId = reply->readAll().mid(1, 36);

    // Проверяем, что получили корректный Guid. Если нет - очищаем строку
    QUuid trackId(m_nextTrackId);
    if (trackId.isNull())
        m_nextTrackId = "";
}

// Воспроизведение следующего трека
void OwnRadio::playNextTrack()
{
    getNextTrackId();
    m_player->setMedia(QUrl(m_serviceUrl + "/track/GetTrackByID/" + m_nextTrackId));
    m_player->play();
}