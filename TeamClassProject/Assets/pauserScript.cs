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

    public Image cntrl;
    private bool ctlB;

    private Vector2 originsfS, originmuS, cntrlOr;

    public AudioMixerGroup fx, mc;


     void Start()
     {
        originmuS = musS.transform.position;
        originsfS = sfxS.transform.position;
        cntrlOr = cntrl.transform.position;
        cntrl.transform.position = new Vector2(9999, 9999);
        sfxS.transform.position = new Vector2(9999, 9999);
        musS.transform.position = new Vector2(9999, 9999);

        pPanel.enabled = false;
        sfxS.enabled = false;
        musS.enabled = false;
        ctlB = false;
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
        cntrl.transform.position = new Vector2(9999, 9999);
        ctlB = false;

    }

    public void ScriptFSound()
    {
        cntrl.transform.position = new Vector2(9999, 9999);
        ctlB = false;

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

    public void ControlSF()
    {

        sfxS.enabled = false;
        musS.enabled = false;
        sfxS.transform.position = new Vector2(9999, 9999);
        musS.transform.position = new Vector2(9999, 9999);


        if (ctlB == false)
        {
            cntrl.transform.position = cntrlOr;
            ctlB = true;
        }
        else
        {
            cntrl.transform.position = new Vector2(9999, 9999);
            ctlB = false;
        }
    }


    public void quitGame()
    {
        //this does nothing. add quit when Nolan adds menu
    }
    

}

