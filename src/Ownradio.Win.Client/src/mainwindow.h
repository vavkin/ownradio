#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QNetworkAccessManager>
#include <QJsonDocument>
#include <QJsonArray>
#include <QJsonValue>
#include <QJsonObject>
#include <QNetworkRequest>
#include <QNetworkReply>
#include <QBuffer>
#include <QMessageBox>
#include <QDebug>

class VlcInstance;
class VlcMedia;
class VlcMediaPlayer;

namespace Ui {
class MainWindow;
}

#define SAFE_RELEASE(x) if(x){delete x;x=nullptr;}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

    void getNextTrack(const QString &_trackid, const QString &_method, bool tillend);

    void startPlayback();
    void stopPlayback();

private slots:
    void on_pushButtonPlay_clicked();
    void on_pushButtonStop_clicked();
    void on_pushButtonNext_clicked();

    void reply(QNetworkReply *reply);
    void nextFile();
    void errorPlayback();
    void errorNetwork(QNetworkReply::NetworkError);

private:
    Ui::MainWindow *ui;

    VlcInstance *instance;
    VlcMedia *media;
    VlcMediaPlayer *player;

    QNetworkAccessManager *manager;

    QString trackid;
    QString method;

    bool done;
    bool play;
};

#endif // MAINWINDOW_H
