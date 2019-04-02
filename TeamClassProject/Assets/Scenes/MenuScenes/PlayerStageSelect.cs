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

    public int position;
    public bool canMove;

    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;
    public GameObject random;

    void Awake()
    {
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        position = 1;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //move right if holding right
        if(myPlayer.GetAxis("Horizontal") >= 0.1f && canMove == true)
        {
            position++;
        }

        //move left if holding left
        if (myPlayer.GetAxis("Horizontal") <= -0.1f && canMove == true)
        {
            position--;
        }

        //if holding left on 0, go back to 5
        if (position <= 0)
        {
            position = 5;
        }

        //if holding right on 5, go to 1
        if (position >= 6)
        {
            position = 1;
        }

        if(myPlayer.GetButtonDown("A") && canMove == true)
        {
            canMove = false;

            if(position == 1)
            {
                GameManager.Stage_1++;
            }

            if (position == 2)
            {
                GameManager.Stage_2++;
            }

            if (position == 3)
            {
                GameManager.Stage_3++;
            }

            if (position == 4)
            {
                GameManager.Stage_4++;
            }
        }

        if (myPlayer.GetButtonDown("B") && canMove == false)
        {
            canMove = true;

            if (position == 1)
            {
                GameManager.Stage_1--;
            }

            if (position == 2)
            {
                GameManager.Stage_2--;
            }

            if (position == 3)
            {
                GameManager.Stage_3--;
            }

            if (position == 4)
            {
                GameManager.Stage_4--;
            }

            if (position == 1)
            {
                stage1.SetActive(true);
            }
            else
            {
                stage1.SetActive(false);
            }

            if (position == 2)
            {
                stage2.SetActive(true);
            }
            else
            {
                stage2.SetActive(false);
            }

            if (position == 3)
            {
                stage3.SetActive(true);
            }
            else
            {
                stage3.SetActive(false);
            }

            if (position == 4)
            {
                stage4.SetActive(true);
            }
            else
            {
                stage4.SetActive(false);
            }

            if (position == 5)
            {
                random.SetActive(true);
            }
            else
            {
                random.SetActive(false);
            }
        }
    }
}
