using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class PlayerStageSelect : MonoBehaviour
{
    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public static int position;
    public static bool hasSelected;

    public GameObject selected_airship;
    public GameObject selected_forest;

    void Awake()
    {
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        position = 2;
        hasSelected = false;

        GameManager.gameState = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //move right if holding right
        if(myPlayer.GetAxis("Horizontal") >= 0.1f && hasSelected == false)
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
            position = 1;
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

        if (myPlayer.GetButtonDown("Jump") && hasSelected == false)
        {
            hasSelected = true;
        }

        if (myPlayer.GetButtonDown("HeavyAttack") && hasSelected == true)
        {
            hasSelected = false;
        }
    }
}
