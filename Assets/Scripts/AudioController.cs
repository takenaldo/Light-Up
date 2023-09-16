using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isMusicONFromSettings())
            gameObject.GetComponent<AudioSource>(). Play();
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
            
    }

    bool isMusicONFromSettings() {
        return PlayerPrefs.GetString(Helper.SETTING_MUSIC, Helper.STATUS_ON) == Helper.STATUS_ON;
    }

    void setMusicONSetting(string status)
    {
        PlayerPrefs.SetString(Helper.SETTING_MUSIC, status);
    }
}
