﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class Portrait : MonoBehaviour
{
    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public GameObject char_1, char_2, char_3, char_4;
    //public GameObject greyRect_1, greyRect_2, greyRect_3, greyRect_4; //this references teammates octagons, NOT YOUR OWN
    public int team_num;
    public int char_selected;
    public bool disabled_for_teammate;
    public float timer;
    public float timerMax;
    public int player_value; //1 - top left, 2 - bot left, 3 - top right, 4 - bot right
    public bool selected; //did this player select a character
    public static int playersLockedIn;
    public GameObject arrows;
    public GameObject dummyObject; //turns on a nonexistant object that allows players to lock in their characters

    public static bool canSelect;

    [Header("Audio")]
    public AudioClip[] stageSounds;
    public AudioSource stageSoundPlayer;

    void Awake()
    {
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        timerMax = 15f;
        selected = false;
        playersLockedIn = 0;
        canSelect = false;

        /*
        greyRect_1.SetActive(false);
        greyRect_2.SetActive(false);
        greyRect_3.SetActive(false);
        greyRect_4.SetActive(false);
        */      
              
    }

    // Update is called once per frame
    void Update()
    {

        timer++;

        if (timer >= timerMax)
        {
            timer = timerMax;
            //dummyObject.SetActive(true);
            //canSelect = true;
        }

        //move right if holding right
        if (myPlayer.GetAxis("Horizontal") >= 0.1f && timer >= timerMax && PlayerStageSelect.hasSelected == true && selected == false)
        {
            char_selected++;
            timer = 0f;
            dummyObject.SetActive(true);

            stageSoundPlayer.clip = stageSounds[1];
            stageSoundPlayer.Play();
        }

        //move left if holding left
        if (myPlayer.GetAxis("Horizontal") <= -0.1f && timer >= timerMax && PlayerStageSelect.hasSelected == true && selected == false)
        {
            char_selected--;
            timer = 0f;
            dummyObject.SetActive(true);

            stageSoundPlayer.clip = stageSounds[1];
            stageSoundPlayer.Play();
        }

        //if holding left on 1, go to 4 //THESE VALUES ARE BEING SET TO 2 BEFORE WE SEND TO NOAH BC WAWA AND GNO ARE NOT IN THE GAME
        if (char_selected < 1)
        {
            char_selected = 2;
        }

        //if holding right on 4, go to 1 //THESE VALUES ARE BEING SET TO 2 BEFORE WE SEND TO NOAH BC WAWA AND GNO ARE NOT IN THE GAME
        if (char_selected > 2)
        {
            char_selected = 1;
        }

        //if char_selected is 1, turn on the claire sprite
        if (char_selected == 1)
        {
            char_1.SetActive(true);
        }
        else
        {
            char_1.SetActive(false);
        }

        //if char_selected is 2, turn on the gil sprite
        if (char_selected == 2)
        {
            char_2.SetActive(true);
        }
        else
        {
            char_2.SetActive(false);
        }

        //if char_selected is 3, turn on the gno sprite
        if (char_selected == 3)
        {
            char_3.SetActive(true);
        }
        else
        {
            char_3.SetActive(false);
        }

        //if char_selected is 4, turn on the wawa sprite
        if (char_selected == 4)
        {
            char_4.SetActive(true);
        }
        else
        {
            char_4.SetActive(false);
        }

        if (myPlayer.GetButtonDown("Jump") && selected == false && PlayerStageSelect.hasSelected == true && timer >= timerMax && canSelect == true)
        {
            timer = 0;
            selected = true;
            arrows.SetActive(false);
            playersLockedIn += 1;

            stageSoundPlayer.clip = stageSounds[0];
            stageSoundPlayer.Play();

            if (char_selected == 1)
            {
                PlayerPrefs.SetInt("Player" + playerNum + "Character", 1);
                //greyRect_1.SetActive(true);
            }
            else
            {
                //greyRect_1.SetActive(false);
            }

            if (char_selected == 2)
            {
                PlayerPrefs.SetInt("Player" + playerNum + "Character", 2);
                //greyRect_2.SetActive(true);
            }
            else
            {
                //greyRect_2.SetActive(false);
            }

            if (char_selected == 3)
            {
                PlayerPrefs.SetInt("Player" + playerNum + "Character", 3);
                //greyRect_3.SetActive(true);
            }
            else
            {
                //greyRect_3.SetActive(false);
            }

            if (char_selected == 4)
            {
                PlayerPrefs.SetInt("Player" + playerNum + "Character", 4);
                //greyRect_4.SetActive(true);
            }
            else
            {
                //greyRect_4.SetActive(false);
            }
        }

        if (myPlayer.GetButtonDown("HeavyAttack") && selected == true && PlayerStageSelect.hasSelected == true && timer >= timerMax)
        {
            stageSoundPlayer.clip = stageSounds[2];
            stageSoundPlayer.Play();

            timer = 0;
            selected = false; 
            arrows.SetActive(true);
            playersLockedIn -= 1;

            /*
            greyRect_1.SetActive(false);
            greyRect_2.SetActive(false);
            greyRect_3.SetActive(false);
            greyRect_4.SetActive(false);
            */
        }
    }
}
