using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MachineBehaviour : MonoBehaviour
{
    [Tooltip("Is the current machine in use?")]
    public bool is_In_Use = false;
    [Header("All Hazards")]
    [Tooltip("Insert all GameObjects that will be controlled by THIS machine")]
    public GameObject[] Controlled_Hazzard;
    [Tooltip("Vertical and Horizontal move speed of certain hazzards")]
    public float speed;
    [Header("Max Hazzards")]
    [Tooltip("How many hazzards will this machine have access to?")]
    public int max_Machines_Amnt;

    //These are the different MACHINE TYPES
    public enum MachineID { SideCannon, BackgroundCannon, SideHazard, MovingPlatform, SpecialPlatform };
    [Header("Machine Type")]
    [Tooltip("What type of machine will this be?")]
    public MachineID mach;


    //inputs are for movement
    private float horizontalInput, verticalInput;
    //this allows us to change between hazzards
    private int Current_Haz_Num = 0;
    //velocity
    private Vector2 vel;

    //rewired after this point
    //myPlayer will properly connect this players inputs to go to the correct location in rewired
    private Player myPlayer;
    //This is in order to "Spawn in" objects
    private ObjectSpawner objectPool;

    

    // Start is called before the first frame update
    void Start()
    {
        //get an instance of the object spawner so we can spawn objects
        objectPool = ObjectSpawner.Instance;

        //only do this if this machine is of type "Background Cannon" 
        if (mach == MachineID.BackgroundCannon)
        {
            Controlled_Hazzard[0].GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazzard[0].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the machine is in use
        if (is_In_Use) {
            
            if (mach == MachineID.SideCannon)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                SideCannonMovement();
            } else if (mach == MachineID.BackgroundCannon)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                BackgroundCannonMovement();
            }
            /*else if (mach == MachineID.SideHazard)
            {

            }else if (mach == MachineID.MovingPlatform)
            {

            }else if (mach == MachineID.SpecialPlatform)
            {

            }*/
        }
    }


    //--------------------------------------------------------------------------------------------------
    // Function description: This contains all movement and interactions for the Side Cannons
    void SideCannonMovement()
    {
        vel.y = verticalInput * speed;
        //this allows players to change which side hazzard is currently selected
        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;
            }else if (Current_Haz_Num >= max_Machines_Amnt)
            {
                Current_Haz_Num = 0;
            }
        }
        //this allows the player to move the side cannon (will be changed to rotation)
        Controlled_Hazzard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazzard[Current_Haz_Num].GetComponent<Rigidbody2D>().position 
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }



    //--------------------------------------------------------------------------------------------------
    //Function Description: This contains all movement and interactions for the Background Cannon
    void BackgroundCannonMovement()
    {
        vel.x = horizontalInput * speed;
        vel.y = verticalInput * speed;
        //this allows the player to spawn an object in the position where the crosshair is
        if (myPlayer.GetButtonDown("Jump"))
        {
            objectPool.SpawnFromPool("Tester", Controlled_Hazzard[Current_Haz_Num].transform.position, Quaternion.identity);
            End_Control();
        }
        //this allows the player to move the crosshair
        Controlled_Hazzard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazzard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }



    //--------------------------------------------------------------------------------------------------
    //Function Description: This contains all movement and interactions for the Side Hazzards (currently Eels)
    void SideHazzardControl()
    {
        //this allows players to change which side hazzard is currently selected
        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;
            }
            else if (Current_Haz_Num >= max_Machines_Amnt)
            {
                Current_Haz_Num = 0;
            }
        }


    }

    //--------------------------------------------------------------------------------------------------
    //Function Description: Will be called from the support script.
    //This script requires access to the correct player number in order to have the correct player interact with the hazzards.
    //Immediately takes player number from player, uses this ID number as its own inputs.
    //In the case of a team ID being necissary~ it also takes a team ID which players have.
    public void Commence_Control(int playerNum, int teamID)
    {
        is_In_Use = true;
        // Recieve the number of a player and use it as my inputs.
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        
        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazzard[0].SetActive(true);
            switch (teamID)
            {
                case 2:
                    Controlled_Hazzard[0].GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazzard[0].GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazzard[0].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }
        }
        Debug.Log("Player:"+playerNum+ " has activated cannon:"+mach);
    }


    //--------------------------------------------------------------------------------------------------
    //Turns off the inputs that were being recieved from the player.
    //Player is no longer going to use this machine for now.
    public void End_Control()
    {
        is_In_Use = false;

        // The playerID "-1" does not exist, therefore, the inputs will never be recieved.
        myPlayer = ReInput.players.GetPlayer(-1);
        
        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazzard[0].GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazzard[0].SetActive(false);
        }
        
        Debug.Log("Player has deactivated machine: "+transform.name);
    }
    
}
