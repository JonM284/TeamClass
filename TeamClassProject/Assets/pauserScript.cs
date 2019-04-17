using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;



public class pauserScript : MonoBehaviour
{

    public EventSystem es;
    public GameObject rsmeB;
    public GameObject sndB;
    public GameObject cntB;
    public GameObject qutB;

    public int position;

    public Canvas pPanel;
    public Canvas canv;

    public Slider sfxS, musS;

    public Image cntrl;
    private bool ctlB;

    private Vector2 originsfS, originmuS, cntrlOr;

    public AudioMixerGroup fx, mc;

    public int playerNum;
    public float timer;
    public float timerMax;

    private bool up, down;

    void Start()
     {

        es.SetSelectedGameObject(null);

        timer = 0f;
        timerMax = 15f;

        position = 1;

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

        if (timer >= timerMax)
        {
            print("gyes");
        }
        up = false;
        down = false;

        Player myPlayer = ReInput.players.Players[0];

        float need = myPlayer.GetAxis("Vertical");
        print(need + "float");

        
        if (myPlayer.GetButtonDown("Pause"))
        {
            Pauser();
        }

        //if (es.currentSelectedGameObject == null)
        {
        //    es.SetSelectedGameObject(es.firstSelectedGameObject);
        }

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


        //Iterating through Players (excluding the System Player)
        //for (int i = 0; i < ReInput.players.playerCount; i++)
        
            if (pPanel.enabled == true)
            {

                if (myPlayer.GetAxis("Vertical") <= -0.1f && timer >= timerMax)
                {
                    print("GoDown");
                    timer = 0;
                    position--;
                }

                if (myPlayer.GetAxis("Vertical") >= 0.1f && timer >= timerMax)
                {
                    timer = 0;
                    position++;
                }

                if (position < 1)
                {
                    position = 4;
                }

                if (position > 4)
                {
                    position = 1;
                }

                if (position == 1)
                {
                    es.SetSelectedGameObject(rsmeB);
                }

                if (position == 2)
                {
                    es.SetSelectedGameObject(sndB);
                }

                if (position == 3)
                {
                    es.SetSelectedGameObject(cntB);
                }

                if (position == 4)
                {
                    es.SetSelectedGameObject(qutB);
                }
            }

        
    }


   public void Pauser()
   {
        if (pPanel.enabled == true)
        {
            Continuer();
        }
        else
        {
            Time.timeScale = 0;
            pPanel.enabled = true;
            print("Pause!");
            canv.enabled = false;
        }
   }

   public void Continuer()
   {
        if (pPanel.enabled == true)
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

    }

    public void ScriptFSound()
    {
        if (pPanel.enabled == true)
        {
            cntrl.transform.position = new Vector2(9999, 9999);
            ctlB = false;

            if (sfxS.enabled == true)
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
    }

    public void AudioChanging()
    {
        print(sfxS.value);
        fx.audioMixer.SetFloat("VolumeSF", sfxS.value);
        mc.audioMixer.SetFloat("VolumeMu", musS.value);
    }

    public void ControlSF()
    {
        if (pPanel.enabled == true)
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
    }


    public void quitGame()
    {
        if (pPanel.enabled == true)
        {
            SceneManager.LoadScene("NolanScene");
        }
    }
    

}

