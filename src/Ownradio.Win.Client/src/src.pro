#-------------------------------------------------
#
# Project created by QtCreator 2016-09-13T09:42:37
#
#-------------------------------------------------

QT       += core gui network
CONFIG 	 += c++11

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = player
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp

HEADERS  += mainwindow.h

LIBS     += -lVLCQtCore

FORMS    += mainwindow.ui
