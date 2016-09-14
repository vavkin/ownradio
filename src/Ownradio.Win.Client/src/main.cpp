#include "mainwindow.h"

#include <QApplication>
#include <QDebug>
#include <VLCQtCore/Common.h>

int main(int argc, char *argv[])
{
    QCoreApplication::setApplicationName("OwnRadio Player");
    QCoreApplication::setAttribute(Qt::AA_X11InitThreads);

    QApplication app(argc, argv);
    VlcCommon::setPluginPath(app.applicationDirPath() + "/plugins");

    MainWindow w;
    w.show();

    return app.exec();
}
