﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MachineBehaviour2 : MonoBehaviour
{
    [Tooltip("Is the current machine in use?")]
    public bool is_In_Use = false;
    [Header("All Hazards")]
    [Tooltip("Insert all GameObjects that will be controlled by THIS machine")]
    public GameObject[] Controlled_Hazard;
    [Tooltip("Vertical and Horizontal move speed of certain hazzards")]
    public float speed;
    [Header("Max Hazzards")]
    [Tooltip("How many hazzards will this machine have access to?")]
    public int max_Machines_Amnt;
    [Tooltip("Max distance for side hazards, max angle for side cannons")]
    public float Max_range;
    //These are the different MACHINE TYPES
    public enum MachineID { Two_Fairy, Two_BottomHazard };
    [Header("Machine Type")]
    [Tooltip("What type of machine will this be?")]
    public MachineID mach;


    //inputs are for movement
    private float horizontalInput, verticalInput;
    //this allows us to change between hazzards
    private int Current_Haz_Num = 0;
    //velocity
    private Vector2 vel;

    public bool can_Use = false;
    private bool other_can_Use = false;
    //rewired after this point
    //myPlayer will properly connect this players inputs to go to the correct location in rewired
    private Player myPlayer;
    //This is in order to "Spawn in" objects
    private ObjectSpawner objectPool;
    private Vector3 Move_Rotation, originalRotation;

    public FairyScript fairyScript;
    private float cooldownTimer_Fairy;
    private float cooldownLength_Fairy;
    private bool Fairy_Machine_Ready;

    public List<Vector3> Hazard_StartPos;
    public List<Vector3> Hazard_MaxPos;
    public List<Vector3> Hazard_MinPos;

    public GameObject my_Controller_Player;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.rotation.x,
            Controlled_Hazard[Current_Haz_Num].transform.rotation.y,
            Controlled_Hazard[Current_Haz_Num].transform.rotation.z);

        Move_Rotation = new Vector3(0, 0, 90);
        //get an instance of the object spawner so we can spawn objects
        objectPool = ObjectSpawner.Instance;
        my_Controller_Player = null;

        //only do this if this machine is of type "Background Cannon" 
        if (mach == MachineID.Two_Fairy)
        {
            Controlled_Hazard[0].SetActive(false);
        }


        //resetting cooldowns
        cooldownTimer_Fairy = 0f;
        cooldownLength_Fairy = 10f;
        Fairy_Machine_Ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        //fairy cooldown
        if(Fairy_Machine_Ready == false)
        {
            cooldownTimer_Fairy -= Time.deltaTime;
            if(cooldownTimer_Fairy <= 0)
            {
                Fairy_Machine_Ready = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }


        //if the machine is in use
        if (is_In_Use) {

            if (mach == MachineID.Two_Fairy)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                FairyControl();
            }
            else if (mach == MachineID.Two_BottomHazard)
            {
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                Two_BottomHazardControl();
            }/*
            else if (mach == MachineID.SideHazard)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                SideHazzardControl();
            }
            else if (mach == MachineID.SpecialPlatform)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                SpecialPlatformBehaviour();
            }
            else if (mach == MachineID.MiddlePlatform)
            {

                MiddlePlatformBehavior();
            }*/
        }

    }


    //--------------------------------------------------------------------------------------------------
    /// <summary>
    /// Function description: This contains all movement and interactions for the Side Cannons
    /// </summary>
    void FairyControl()
    {

        vel.x = horizontalInput * speed;
        vel.y = verticalInput * speed;
        //if fairy hits a player
        if (fairyScript.fairyHitPlayer == true)
        {
            StartCoroutine(FairyResetInitialDelay());
            End_Control();
            Fairy_Machine_Ready = false;
            cooldownTimer_Fairy = cooldownLength_Fairy;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        //this allows the player to move the crosshair
        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }

    IEnumerator FairyResetInitialDelay()
    {
        yield return new WaitForSeconds(1.0f);
        fairyScript.fairyHitPlayer = false;
    }

    void Two_BottomHazardControl()
    {
        vel.x = horizontalInput * speed;
        print(Current_Haz_Num);
        //this allows players to change which side hazzard is currently selected
        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;
            }

        }
        //reset current_haz_Num if it is greater than or equal to the max number of hazzards
        if (Current_Haz_Num >= max_Machines_Amnt)
        {
            Current_Haz_Num = 0;
        }


        if(myPlayer.GetButton("BasicAttack") && can_Use)
        {
            if (Current_Haz_Num == 0)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Spike_Movement>().Spike_Active = true;
            }
            else if(Current_Haz_Num == 1)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Tree_Movement>().Tree_Active = true;
            }

            End_Control();
        }
        /*
        if (myPlayer.GetButtonDown("BasicAttack") && can_Use)
        {

            Debug.Log("Should go off");
            Controlled_Hazard[Current_Haz_Num].GetComponent<Eel_Movement>().Eel_Active = true;
            End_Control();
        }
        /*
        if (myPlayer.GetButtonDown("Jump"))
        {
            End_Control();
        }
        */
        /*if (Controlled_Hazard[Current_Haz_Num].transform.position.x >= Hazard_MaxPos[Current_Haz_Num].x)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MaxPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }

        if (Controlled_Hazard[Current_Haz_Num].transform.position.x <= Hazard_MinPos[Current_Haz_Num].x)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MinPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        */
        //this allows the player to move the side cannon (will be changed to rotation)
        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);



    }

    //--------------------------------------------------------------------------------------------------
    /// <summary>
    /// Function Description: This contains all movement and interactions for the Background Cannon.
    /// </summary>
    void BackgroundCannonMovement()
    {
        vel.x = horizontalInput * speed;
        vel.y = verticalInput * speed;

        //this allows the player to spawn an object in the position where the crosshair is
        if (myPlayer.GetButtonDown("Jump") && can_Use)
        {
            objectPool.SpawnFromPool("Tester", Controlled_Hazard[Current_Haz_Num].transform.position, Quaternion.identity);
            End_Control();
            Debug.Log("Has Spawned object");
        }

        //this allows the player to move the crosshair
        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }



    

    //--------------------------------------------------------------------------------------------------
    /// <summary>
    ///
    //This Function requires access to the correct player number in order to have the correct player interact with the hazzards.
    /// </summary>
    /// <param name="playerNum">Player ID from the support character that is activating this machine.</param>
    /// <param name="teamID">Player's Team ID</param>
    public void Commence_Control(int playerNum, int teamID, GameObject player)
    {
        // Recieve the number of a player and use it as my inputs.
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        my_Controller_Player = player;

        is_In_Use = true;

        StartCoroutine(waitForUse());

        if (mach == MachineID.Two_Fairy)
        {
            Controlled_Hazard[0].SetActive(true);
        }

            Debug.Log("Player:"+playerNum+ " has activated hazzard: "+mach);
    }


    //--------------------------------------------------------------------------------------------------
    /// <summary>
    /// Turns off the inputs that were being recieved from the player.
    /// Player is no longer going to use this machine for now.
    /// </summary>
    public void End_Control()
    {
        is_In_Use = false;
        can_Use = false;
        other_can_Use = false;
        my_Controller_Player.GetComponent<AlternateSP2>().status = AlternateSP2.Status.Free;
        my_Controller_Player = null;
        // The playerID "-1" does not exist, therefore, the inputs will never be recieved.
        myPlayer = ReInput.players.GetPlayer(-1);

        if (mach == MachineID.Two_Fairy)
        {
            Controlled_Hazard[0].transform.position = fairyScript.startPos;
            Controlled_Hazard[0].SetActive(false);
        }

        Debug.Log("Player has deactivated machine: "+transform.name);
    }




    //--------------------------------------------------------------------------------------------------
    // This function just draws Gizmos in unity
    /*private void OnDrawGizmos()
    {
        if (mach == MachineID.SideHazard) {
            Gizmos.color = new Color32(255, 0, 0, 200);
            for (int i = 0; i < Controlled_Hazard.Length; i++) {
                Gizmos.DrawLine(new Vector3(Controlled_Hazard[i].transform.position.x,
                    Controlled_Hazard[i].transform.position.y + 0.6f, Controlled_Hazard[i].transform.position.x),
                    new Vector3(Controlled_Hazard[i].transform.position.x,
                    Controlled_Hazard[i].transform.position.y - 0.6f, Controlled_Hazard[i].transform.position.x));
            }
        }

        if (mach == MachineID.BackgroundCannon)
        {
            Gizmos.color = new Color32(0,255,0, 80);
        }
        if (mach == MachineID.SideCannon)
        {
            Gizmos.color = new Color32(0, 0, 255, 125);
        }
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
    */

    IEnumerator waitForUse()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Now can use");
        can_Use = true;
        other_can_Use = true;
    }

}
