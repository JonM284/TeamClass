using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;

public class optionScene : MonoBehaviour
{

    public Slider sfxS, musS;
    public AudioMixerGroup fx, mc;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void AudioChanging()
    {
        //print(sfxS.value);
        fx.audioMixer.SetFloat("VolumeSF", sfxS.value);
        mc.audioMixer.SetFloat("VolumeMu", musS.value);
    }

    public void quitGame()
    {
        SceneManager.LoadScene("NolanScene");
    }

}
