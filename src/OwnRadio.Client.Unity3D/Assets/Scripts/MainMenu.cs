using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class MainMenu : MonoBehaviour {
    const string API_WEBSITE_STRING = "http://ownradio.ru/api/track/";
    const string API_GET_NEXT_TRACK_METHOD = "GetNextTrackID/";
    const string API_GET_TRACK_BY_ID_METHOD = "GetTrackByID/";
    const string DEFAULT_DEVICE_ID = "12345678-1234-1234-1234-123456789012";
    public const string RESOURCES_FOLDER_PATH = "Assets/Resources/";

    AudioSource _audioSource;
    WWW _www;
    Track currentTrack;
    bool _isTrackDownloaded;
    bool _isNextTrackIdGot;
    string _nextTrackId;

    void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
        _isTrackDownloaded = false;

    }
	
	void Update () {
        if (_isNextTrackIdGot)
        {
            StartCoroutine(GetTrackByID(currentTrack.fileGuid.ToString()));
            _isNextTrackIdGot = false;
        }
	    if (_isTrackDownloaded)
        {
            _isTrackDownloaded = false;
            PlayFile("tmp");
        }
	}
    void PlayFile(string title)
    {
        _audioSource.clip = Resources.Load<AudioClip>(title);
        _audioSource.Play();
    }

    //Нажатие кнопки вызывает эту функцию, запускаем корутину получения GUID трека
    public void PlaySong()
    {
        _isTrackDownloaded = false;
        _isNextTrackIdGot = false;
        StartCoroutine(GetNextTrackID());
    }
    
    IEnumerator GetNextTrackID(string deviceId = DEFAULT_DEVICE_ID)
    {
        string url = API_WEBSITE_STRING + API_GET_NEXT_TRACK_METHOD + deviceId;
        _www = new WWW(url);
        yield return _www;

        //Удаляем кавычки в начале/конце
        string wwwGUID = _www.text.Substring(1, _www.text.Length - 2);
        
        currentTrack = new Track("tmp", new Guid(wwwGUID));
        _isNextTrackIdGot = true;
    }

    IEnumerator GetTrackByID(string trackId)
    {
        string url = API_WEBSITE_STRING + API_GET_TRACK_BY_ID_METHOD + currentTrack.fileGuid.ToString();
        _www = new WWW(url);
        yield return _www;
        File.WriteAllBytes(RESOURCES_FOLDER_PATH + "tmp.mp3", _www.bytes);
        _isTrackDownloaded = true;
    }
}
