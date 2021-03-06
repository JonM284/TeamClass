﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    [Header("Audio")]
    public AudioClip[] mainMenuSounds;
    public AudioSource mainMenuSoundPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName == "JonScene")
        {
            Destroy(this.gameObject);
        }

        if (Application.loadedLevelName == "JustinScene")
        {
            Destroy(this.gameObject);
        }

        if (Application.loadedLevelName == "WinBlue")
        {
            Destroy(this.gameObject);
        }

        if (Application.loadedLevelName == "WinRed")
        {
            Destroy(this.gameObject);
        }

    }

    public void menuSelect()
    {
        mainMenuSoundPlayer.clip = mainMenuSounds[0];
        mainMenuSoundPlayer.Play();

    }

    public void menuBrowse()
    {
        mainMenuSoundPlayer.clip = mainMenuSounds[1];
        mainMenuSoundPlayer.Play();

    }

    public void menuBack()
    {
        mainMenuSoundPlayer.clip = mainMenuSounds[2];
        mainMenuSoundPlayer.Play();

     }

}
