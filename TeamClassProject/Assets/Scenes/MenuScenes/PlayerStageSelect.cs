using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.SceneManagement;

public class PlayerStageSelect : MonoBehaviour
{
    //the following is in order to use rewired
    //[Tooltip("Reference for using rewired")]
    //private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public static int position;
    public static bool hasSelected;

    public GameObject selected_airship;
    public GameObject selected_forest;

    public GameObject stage_airship;
    public GameObject stage_forest;

    public GameObject white_box;
    public GameObject white_box_2;

    public float timer;
    public float timerMax;

    void Awake()
    {
        //myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        position = 2;
        hasSelected = false;

        //GameManager.gameState = 2;

        white_box.SetActive(false);
        white_box_2.SetActive(false);

        timer = 0f;
        timerMax = 15f;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];

            timer++;

            if (timer >= timerMax)
            {
                timer = timerMax;
            }

            //move right if holding right
            if (myPlayer.GetAxis("Horizontal") >= 0.1f && hasSelected == false)
            {
                position--;
            }

            //move left if holding left
            if (myPlayer.GetAxis("Horizontal") <= -0.1f && hasSelected == false)
            {
                position++;
            }

            //if holding left on 1, don't do anything
            if (position <= 1)
            {
                position = 1; //THIS IS OFF TEMPORARILY BECAUSE WE ARENT SENDING NOAH STAGE 2
               // position = 2;
            }

            //if holding right on 2, don't do anything 
            if (position >= 2)
            {
                position = 2;
            }

            //if position is 1, turn on the grey object over the airship level
            if (position == 1)
            {
                selected_forest.SetActive(true);
            }
            else
            {
                selected_forest.SetActive(false);
            }

            //if position is 2, turn on the grey object over the forest level
            if (position == 2)
            {
                selected_airship.SetActive(true);
            }
            else
            {
                selected_airship.SetActive(false);
            }

            if (myPlayer.GetButtonDown("Jump") && hasSelected == false && timer >= timerMax)
            {
                Debug.Log("pos"+position);
                timer = 0;
                hasSelected = true;
                white_box.SetActive(true);
                white_box_2.SetActive(false);

                if (position == 1)
                {
                    stage_forest.SetActive(true);
                    selected_forest.SetActive(false);
                    selected_airship.SetActive(false);
                    
                }

                if (position == 2)
                {
                    stage_airship.SetActive(true);
                    selected_forest.SetActive(false);
                    selected_airship.SetActive(false);
                    
                }
            }

            if (myPlayer.GetButtonDown("HeavyAttack") && hasSelected == true && timer >= timerMax && Portrait.playersLockedIn == 0)
            {
                white_box.SetActive(false);
                white_box_2.SetActive(true);
                stage_airship.SetActive(false);
                stage_forest.SetActive(false);
                hasSelected = false;
                timer = 0;
            }

            if (myPlayer.GetButtonDown("HeavyAttack") && hasSelected == false && timer >= timerMax)
            {
                timer = 0;
                SceneManager.LoadScene("NolanScene");
            }

            if (Portrait.playersLockedIn >= 4 && position == 2)
            {
                PlayerPrefs.SetInt("Current_Stage", 0);
                SceneManager.LoadScene("JonScene");
            }

            if (Portrait.playersLockedIn >= 4 && position == 1)
            {
                PlayerPrefs.SetInt("Current_Stage", 1);
                SceneManager.LoadScene("JustinScene");
            }
        }
    }
}
