using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;

public class ButtonManager : MonoBehaviour
{
    public EventSystem es;
    public GameObject playButton;
    public GameObject fightersButton;
    public GameObject optionsButton;
    public GameObject extrasButton;
    public GameObject quitButton;
    //private bool buttonSelected;
    public int position;
    public bool pressed;

    //the following is in order to use rewired
    //[Tooltip("Reference for using rewired")]
    //private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public float timer;
    public float timerMax;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        timer = 0f;
        timerMax = 15f;

        position = 1;

        es.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        timer++;

        if (timer >= timerMax)
        {
            timer = timerMax;
        }

        if (es.currentSelectedGameObject == null){
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }

        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];


            if (myPlayer.GetAxis("Vertical") >= 0.1f && timer >= timerMax)
            {
                timer = 0;
                position--;
            }

            if (myPlayer.GetAxis("Vertical") <= -0.1f && timer >= timerMax)
            {
                timer = 0;
                position++;
            }

            if (position <= 0)
            {
                position = 5;
            }

            if (position >= 6)
            {
                position = 1;
            }

            if (position == 1)
            {
                es.SetSelectedGameObject(playButton);
            }

            if (position == 2)
            {
                es.SetSelectedGameObject(fightersButton);
            }

            if (position == 3)
            {
                es.SetSelectedGameObject(optionsButton);
            }

            if (position == 4)
            {
                es.SetSelectedGameObject(extrasButton);
            }

            if (position == 5)
            {
                es.SetSelectedGameObject(quitButton);
            }
        }
    }

    public void PlayBtn()
    {
        if (MenuSliderScript.sliderInt == 1 && timer >= timerMax)
        {
            SceneManager.LoadScene("StageSelection");
            timer = 0;
        }
    }

    public void FighterBtn()
    {
        if (MenuSliderScript.sliderInt == 1 && timer >= timerMax)
        {
            SceneManager.LoadScene("FightersScene");
        }
    }

    public void OptionsBtn()
    {
        //SceneManager.LoadScene("");
    }

    public void ExtrasBtn()
    {
        if (MenuSliderScript.sliderInt == 1 && timer >= timerMax)
        {
            SceneManager.LoadScene("Credits");
        }
    }

    public void QuitBtn()
    {
        if (MenuSliderScript.sliderInt == 1 && timer >= timerMax)
        {
            timer = 0;
            Application.Quit();
        }
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
