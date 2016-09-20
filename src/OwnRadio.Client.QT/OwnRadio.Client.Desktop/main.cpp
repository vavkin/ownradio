#include "ownradio.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    OwnRadio w;
    // Меняем стиль диалогового окна, чтобы появились кнопки "Свернуть/Развернуть"
    w.setWindowFlags(Qt::Window);
    w.show();
    return a.exec();
}
