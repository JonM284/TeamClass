using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MenuSliderScript : MonoBehaviour
{
    //the following is in order to use rewired
    //[Tooltip("Reference for using rewired")]
    //private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    private Animator anim;
    public static int sliderInt;
    public AudioSource menuUp;
    public AudioSource menuDown;
    public static bool hasMenuBeenUp;

    public GameObject pressA;
    //public GameObject pressB;

    //public Animation pressA_Fade;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sliderInt = 0;
        hasMenuBeenUp = false;
        pressA.SetActive(true);
        //pressB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];

            if (myPlayer.GetButtonDown("Jump") && sliderInt != 1)
            {
                anim.SetInteger("SliderValue", 1);
                //menuUp.Play();
                sliderInt = 1;
                hasMenuBeenUp = true;
                pressA.SetActive(false);
                //pressB.SetActive(true);

            }

            if (myPlayer.GetButtonDown("HeavyAttack") && sliderInt != 2 && hasMenuBeenUp == true)
            {
                anim.SetInteger("SliderValue", 2);
                //menuDown.Play();
                sliderInt = 2;
                pressA.SetActive(true);
                //pressB.SetActive(false);
            }
        }
    }
}
