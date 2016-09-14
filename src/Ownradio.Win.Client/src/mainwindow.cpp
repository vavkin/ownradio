#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <VLCQtCore/Common.h>
#include <VLCQtCore/Instance.h>
#include <VLCQtCore/Media.h>
#include <VLCQtCore/MediaPlayer.h>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    media(0)
{
    ui->setupUi(this);

    manager = new QNetworkAccessManager(this);
    connect(manager,SIGNAL(finished(QNetworkReply*)),this,SLOT(reply(QNetworkReply*)));

    instance = new VlcInstance(VlcCommon::args(), 0);

    player = new VlcMediaPlayer(instance);
    connect(player,SIGNAL(end()),this,SLOT(nextFile()));
    connect(player,SIGNAL(error()),this,SLOT(errorPlayback()));
    
    trackid = "-1";
    method = "";

    done = false;
    play = false;
}

MainWindow::~MainWindow()
{
    stopPlayback();

    SAFE_RELEASE(player);
    SAFE_RELEASE(instance);
    SAFE_RELEASE(manager);
    
    SAFE_RELEASE(ui);
}

void MainWindow::errorNetwork(QNetworkReply::NetworkError)
{
    trackid = "-1";
    method = "";

    QMessageBox::information(this,"Error","Network error for some reason", QMessageBox::Ok);
}

void MainWindow::errorPlayback()
{
    if (player!=nullptr)
        stopPlayback();
    
    trackid = "-1";
    method = "";

    QMessageBox::information(this,"Error","Error playback for some reason", QMessageBox::Ok);
}

void MainWindow::reply(QNetworkReply* reply)
{
    trackid = "-1";
    method = "";

    if (reply->error() == QNetworkReply::NoError)
    {
        QString array = (QString)reply->readAll();

        QJsonDocument document = QJsonDocument::fromJson(array.toUtf8());
        if(!document.isNull())
        {
            QJsonObject root = document.object();
            if(!root.isEmpty())
            {
                QJsonValue _trackid = root["TrackId"];
                QJsonValue _method = root["Method"];

                trackid = _trackid.toString();
                method = _method.toString();
            }
        }
    }

    done = true;
}

void MainWindow::getNextTrack(const QString &_trackid, const QString &_method, bool tillend)
{
    QUrl url = "http://radio.redoc.ru/api/TrackSource/NextTrack?userId=297f55b4-d42c-4e30-b9d7-a802e7b7eed9&lastTrackId=" +_trackid + "&lastTrackMethod=" + _method + "&listedTillTheEnd=" + ((tillend==true) ? "true" : "false");

    done = false;

    QNetworkReply *reply = manager->get(QNetworkRequest(url));
    connect(reply, SIGNAL(error(QNetworkReply::NetworkError)),this,SLOT(errorNetwork(QNetworkReply::NetworkError)));

    while(done == false)
    {
        QTime dieTime= QTime::currentTime().addSecs(1);
        while (QTime::currentTime() < dieTime)
            QCoreApplication::processEvents(QEventLoop::AllEvents, 100);
    }
}

void MainWindow::startPlayback()
{
    QString url = "http://radio.redoc.ru/api/TrackSource/Play?trackId=" + trackid;

    media = new VlcMedia(url, instance);
    player->open(media);

    play = true;
}

void MainWindow::stopPlayback()
{
    if (play == true)
    {
        player->stop();
        play = false;
    }

    SAFE_RELEASE(media);
}

void MainWindow::on_pushButtonPlay_clicked()
{
    if (trackid == "-1")
    {
        if(manager != nullptr)
        {
            getNextTrack("-1", "", false);
            if (trackid == "-1") return;
        }
    }

    ui->pushButtonStop->setEnabled(true);
    ui->pushButtonNext->setEnabled(true);
    ui->pushButtonPlay->setEnabled(false);

    if(player != nullptr)
    {
        startPlayback();
    }
}

void MainWindow::on_pushButtonStop_clicked()
{
    ui->pushButtonPlay->setEnabled(true);
    ui->pushButtonNext->setEnabled(false);
    ui->pushButtonStop->setEnabled(false);

    if(player != nullptr)
    {
        stopPlayback();
    }
}

void MainWindow::on_pushButtonNext_clicked()
{
    if((player != nullptr) && (manager != nullptr))
    {
        stopPlayback();

        getNextTrack(trackid, method, false);
        if (trackid == "-1") return;

        startPlayback();
    }
}

void MainWindow::nextFile()
{
    if((player != nullptr) && (manager != nullptr))
    {
        stopPlayback();

        getNextTrack(trackid, method, true);
        if (trackid == "-1") return;

        startPlayback();
    }
}
