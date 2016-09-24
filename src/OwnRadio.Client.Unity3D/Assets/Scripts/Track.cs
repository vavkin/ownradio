using UnityEngine;
using System.Collections;
using System;

public class Track {
    public Track(string title, Guid guid)
    {
        fileGuid = guid;
        this.title = title;
        fileName = title + "mp3";
        filePath = MainMenu.RESOURCES_FOLDER_PATH;
    }
    public string fileName;
    public string filePath;
    public string title;
    public Guid fileGuid;
}
