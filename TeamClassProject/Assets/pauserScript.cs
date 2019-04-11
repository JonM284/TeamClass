using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class pauserScript : MonoBehaviour
{

    public Canvas pPanel;
    public Canvas canv;

    public Slider sfxS, musS;

    private Vector2 originsfS, originmuS;

    public AudioMixerGroup fx, mc;


     void Start()
     {
        originmuS = musS.transform.position;
        originsfS = sfxS.transform.position;


        sfxS.transform.position = new Vector2(9999, 9999);
        musS.transform.position = new Vector2(9999, 9999);

        pPanel.enabled = false;
        sfxS.enabled = false;
        musS.enabled = false;

     }


     void Update()
     {

         print("1GoGo");
         if (Input.GetKeyDown(KeyCode.Escape))
         {
             if (pPanel.enabled)
             {
                 Continuer();
             }
             else
             {
                 Pauser();
             }
         }
     }


   public void Pauser()
   {
      Time.timeScale = 0;
      pPanel.enabled = true;
      print("Pause!");
      canv.enabled = false;
   }

   public void Continuer()
   {
      Time.timeScale = 1;
      pPanel.enabled = false;
      print("Continue!");
      canv.enabled = true;

        sfxS.enabled = false;
        musS.enabled = false;
        sfxS.transform.position = new Vector2(9999, 9999);
        musS.transform.position = new Vector2(9999, 9999);

    }

    public void ScriptFSound()
    {
        if (sfxS.enabled==true)
        {
            sfxS.enabled = false;
            musS.enabled = false;
            sfxS.transform.position = new Vector2(9999, 9999);
            musS.transform.position = new Vector2(9999, 9999);

        }
        else
        {
            sfxS.transform.position = originsfS;
            musS.transform.position = originmuS;
            sfxS.enabled = true;
            musS.enabled = true;
        }
    }

    public void AudioChanging()
    {
        print(sfxS.value);
        fx.audioMixer.SetFloat("VolumeSF", sfxS.value);
        mc.audioMixer.SetFloat("VolumeMu", musS.value);
    }

}

