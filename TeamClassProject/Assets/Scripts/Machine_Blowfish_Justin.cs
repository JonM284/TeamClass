using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine_Blowfish_Justin : MonoBehaviour
{
    private GameObject platformBlowfishLeft;
    private GameObject platformBlowfishRight;
    private Rigidbody2D rb_Blowfish_Left;
    private Rigidbody2D rb_Blowfish_Right;
    private float platformBlowfishMoveSpeed = 5.0f;
    public int blowfishPlatformControlled;  //0 = none, 1 = left, 2 = right
    public bool blowfishMachineControlled;

    // Start is called before the first frame update
    void Start()
    {
        //connect rigidbodies of blowfish platforms when first start game
        platformBlowfishLeft = GameObject.Find("PlatformBlowFishLeft");
        platformBlowfishRight = GameObject.Find("PlatformBlowFishRight");
        rb_Blowfish_Left = platformBlowfishLeft.GetComponent<Rigidbody2D>();
        rb_Blowfish_Right = platformBlowfishRight.GetComponent<Rigidbody2D>();
        blowfishPlatformControlled = 0;
        blowfishMachineControlled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ControlBlowfish();
    }


    public void ControlBlowfish()
    {
        //if you are accessing the machine
        if(blowfishMachineControlled == true)
        {
            //To switch which platform you are controlling
            
            if (/*WHAT BUTTON DO WE WANT TO SWITCH PLATFORMS?*/ Input.GetButtonDown("Jump") && blowfishPlatformControlled == 1)     //one button used for changing which platform you are controlling, left(1) or right(2)
            {
                blowfishPlatformControlled = 2;
            }
            else if (/*WHAT BUTTON DO WE WANT TO SWITCH PLATFORMS?*/ Input.GetButtonDown("Jump") && blowfishPlatformControlled == 2)
            {
                blowfishPlatformControlled = 1;
            }

            //move left platform up/down
            if (/*NEED THIS CHANGED TO ANALOG STICK*/Input.GetKey(KeyCode.W) && blowfishPlatformControlled == 1)         //change Key to whatever Input the left analog stick is
            {
                rb_Blowfish_Left.MovePosition(platformBlowfishLeft.transform.position + platformBlowfishLeft.transform.up * platformBlowfishMoveSpeed * Time.deltaTime);
            }
            else if (/*NEED THIS CHANGED TO ANALOG STICK*/Input.GetKey(KeyCode.S) && blowfishPlatformControlled == 1)         //change Key to whatever Input the left analog stick is and delete this if statement
            {
                rb_Blowfish_Left.MovePosition(platformBlowfishLeft.transform.position + -platformBlowfishLeft.transform.up * platformBlowfishMoveSpeed * Time.deltaTime);
            }
            //move right platform up/down
            if /*NEED THIS CHANGED TO ANALOG STICK*/(Input.GetKey(KeyCode.W) && blowfishPlatformControlled == 2)    //change Key to whatever Input the left analog stick is
            {
                rb_Blowfish_Right.MovePosition(platformBlowfishRight.transform.position + platformBlowfishRight.transform.up * platformBlowfishMoveSpeed * Time.deltaTime);
            }
            else if (/*NEED THIS CHANGED TO ANALOG STICK*/Input.GetKey(KeyCode.S) && blowfishPlatformControlled == 2)         //change Key to whatever Input the left analog stick is and delete this if statement
            {
                rb_Blowfish_Right.MovePosition(platformBlowfishRight.transform.position + -platformBlowfishRight.transform.up * platformBlowfishMoveSpeed * Time.deltaTime);
            }
            
        }
        
    }


    void OnTriggerStay2D(Collider2D other)
    {
        //enter machine Blowfish
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Jump") && blowfishMachineControlled == false && blowfishPlatformControlled == 0)
        {
            blowfishMachineControlled = true;
            blowfishPlatformControlled = 1;

            SupportPlayer.movementEnabled = false;
            //other.gameObject.GetComponent<SupportPlayer>().movementEnabled = false;   //use this instead, and change SupportPlayer to whatever the name of the script is
        }

        //Leave machine Blowfish
        //if (other.gameObject.tag == "Player" && /*WHAT BUTTON DO WE WANT TO LEAVE MACHINE?*/    Input.GetButtonDown("") && blowfishMachineControlled == true && blowfishPlatformControlled != 0)
        //{
            //blowfishMachineControlled = false;
            //SupportPlayer.movementEnabled = true;
            //other.gameObject.GetComponent<SupportPlayer>().movementEnabled = true;   //use this instead, and change SupportPlayer to whatever the name of the script is
                //cooldown for machine?
        //}
    }


}
