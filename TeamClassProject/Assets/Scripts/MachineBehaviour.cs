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
    public GameObject[] Controlled_Hazard;
    [Tooltip("Vertical and Horizontal move speed of certain hazzards")]
    public float speed;
    [Header("Max Hazzards")]
    [Tooltip("How many hazzards will this machine have access to?")]
    public int max_Machines_Amnt;
    [Tooltip("Max distance for side hazards, max angle for side cannons")]
    public float Max_range;
    //These are the different MACHINE TYPES
    public enum MachineID { SideCannon, BackgroundCannon, SideHazard, SpecialPlatform, MiddlePlatform };
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

    //variables for side hazzards "Eels"
    float sideHazardMovement;
    float moveSpeed = 2f;

    

    public bool sideHazardMachineEnabled;
    
    public List<Vector3> Hazard_StartPos;
    public List<Vector3> Hazard_MaxPos;
    public List<Vector3> Hazard_MinPos;

    private bool sideHazardShouldLerp;
    private bool sideHazardMovingForward;
    float sideHazardLerpSpeed;
    float sideHazardLerpTimer;

    bool sideHazardReady;
    float sideHazardCooldownTimer;

    //variables for MiddlePlatform "Ship"
    private GameObject middlePlatform;
    private Rigidbody2D middlePlatform_rb;
    private BoxCollider2D middlePlatform_blowBoxCollider;
    private BoxCollider2D middlePlatform_vacuumBoxCollider;
    private float middlePlatform_moveSpeed;
    private bool middlePlatform_movingUp;
    private bool middlePlatform_cycleFinished;


    // Start is called before the first frame update
    void Start()
    {
        originalRotation = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.rotation.x, 
            Controlled_Hazard[Current_Haz_Num].transform.rotation.y,
            Controlled_Hazard[Current_Haz_Num].transform.rotation.z);

        Move_Rotation = new Vector3(0,0,90);
        //get an instance of the object spawner so we can spawn objects
        objectPool = ObjectSpawner.Instance;


        if (mach == MachineID.SideHazard || mach == MachineID.SpecialPlatform) {
            for (int i = 0; i < Controlled_Hazard.Length; i++) {
                Hazard_StartPos[i] = Controlled_Hazard[i].transform.position;
                sideHazardLerpSpeed = 30f;
                sideHazardReady = true;
                Hazard_StartPos.Add(Hazard_StartPos[i]);
                Hazard_MaxPos[i] = new Vector3(Controlled_Hazard[i].transform.position.x, 
                    Controlled_Hazard[i].transform.position.y + Max_range, Controlled_Hazard[i].transform.position.z);
                Hazard_MaxPos.Add(Hazard_MaxPos[i]);
                Hazard_MinPos[i] = new Vector3(Controlled_Hazard[i].transform.position.x,
                    Controlled_Hazard[i].transform.position.y - Max_range, Controlled_Hazard[i].transform.position.z);
                Hazard_MinPos.Add(Hazard_MinPos[i]);
            }
        }

        //only do this if this machine is of type "Background Cannon" 
        if (mach == MachineID.BackgroundCannon)
        {
            Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazard[0].SetActive(false);
        }

        //set middlePlatform variables
        middlePlatform = GameObject.Find("MiddlePlatform");
        middlePlatform_rb = middlePlatform.GetComponent<Rigidbody2D>();
        middlePlatform_vacuumBoxCollider = GameObject.Find("MiddlePlatform_Collider_Vacuum").GetComponent<BoxCollider2D>();
        middlePlatform_blowBoxCollider = GameObject.Find("MiddlePlatform_Collider_Blow").GetComponent<BoxCollider2D>();
        middlePlatform_moveSpeed = 0.125f;
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
            }
            else if (mach == MachineID.BackgroundCannon)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                BackgroundCannonMovement();
            }
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
            }
        }
      
    }


    //--------------------------------------------------------------------------------------------------
    /// <summary>
    /// Function description: This contains all movement and interactions for the Side Cannons
    /// </summary>
    void SideCannonMovement()
    {

        //this allows players to change which side hazzard is currently selected
        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;
               
            }

        }

        if (myPlayer.GetButtonDown("Jump") && can_Use)
        {
            //Debug.Log(Controlled_Hazard[Current_Haz_Num].transform.GetChild(0).transform.name);
            //Vector3 dir = Controlled_Hazard[Current_Haz_Num].transform.GetChild(0).transform.position - Controlled_Hazard[Current_Haz_Num].transform.position;
            objectPool.SpawnFromPool("CannonBall", Controlled_Hazard[Current_Haz_Num].transform.position,
                Quaternion.Euler(Move_Rotation));
            End_Control();
            Debug.Log("Has Spawned object");
        }

        if (Current_Haz_Num >= max_Machines_Amnt)
        {
            Current_Haz_Num = 0;
        }

        
        if (verticalInput > 0.1f)
        {
            Move_Rotation.z += Time.deltaTime * speed;
        }
        if (verticalInput < -0.1f)
        {
            Move_Rotation.z -= Time.deltaTime * speed;
        }

        Controlled_Hazard[Current_Haz_Num].transform.rotation = Quaternion.Euler(Move_Rotation);

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
    /// Function Description: This contains all movement and interactions for the Side Hazzards (currently Eels).
    /// </summary>
    void SideHazzardControl()
    {

        vel.y = verticalInput * speed;

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

        if (myPlayer.GetButtonDown("BasicAttack") && can_Use)
        {
            Debug.Log("Should go off");
            Controlled_Hazard[Current_Haz_Num].GetComponent<Eel_Movement>().Eel_Active = true;
            End_Control();
        }
        

       /* //cooldown timer
        sideHazardCooldownTimer -= Time.deltaTime;

        //cooldown finished
        if (sideHazardCooldownTimer < 0)
        {
            sideHazardReady = true;
        }*/

        if (Controlled_Hazard[Current_Haz_Num].transform.position.y >= Hazard_MaxPos[Current_Haz_Num].y)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MaxPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }

        if (Controlled_Hazard[Current_Haz_Num].transform.position.y <= Hazard_MinPos[Current_Haz_Num].y)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MinPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        /*
        //lerping timer
        sideHazardLerpTimer += Time.deltaTime;

        //if you're in the machine and you hit jump the eel goes/starts
        if (sideHazardMachineEnabled == true && myPlayer.GetButtonDown("BasicAttack") && sideHazardReady == true)
        {
            sideHazardReady = false;
            sideHazardShouldLerp = true;
            sideHazardMovingForward = true;
            sideHazardLerpTimer = 0;
            sideHazardCooldownTimer = 5.0f;
        }

        //eel moves right first
        if (sideHazardShouldLerp == true && sideHazardMovingForward == true)
        {
            Controlled_Hazard[Current_Haz_Num].transform.Translate(Vector3.right * sideHazardLerpSpeed * Time.deltaTime);
        }
        //eel moves back left after
        else if (sideHazardShouldLerp == true && sideHazardMovingForward == false)
        {
            Controlled_Hazard[Current_Haz_Num].transform.Translate(Vector3.left * sideHazardLerpSpeed * Time.deltaTime);
        }

        //if the timer exceeds half a second but not a second, and its moving right/forward, then stop it 
        if (sideHazardLerpTimer > .5f && sideHazardLerpTimer <= 1.0f && sideHazardShouldLerp == true && sideHazardMovingForward == true)
        {
            sideHazardShouldLerp = false;
            //sideHazardMovingForward = false;
        }
        //it stops for half a second then moves back to left
        if (sideHazardLerpTimer > 1.0f && sideHazardMovingForward == true)
        {
            sideHazardMovingForward = false;
            sideHazardShouldLerp = true;
        }
        //after cycle completes it moves back to starting position and gets ready for next time
        if (sideHazardLerpTimer >= 1.5f && sideHazardShouldLerp == true)
        {
            sideHazardShouldLerp = false;
            sideHazardMachineEnabled = false;
            Controlled_Hazard[Current_Haz_Num].transform.position = sideHazardStartPos[Current_Haz_Num];
            SupportPlayer.movementEnabled = true;
        }
        */
        //this allows the player to move the side cannon (will be changed to rotation)
        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }


    void SpecialPlatformBehaviour()
    {
        vel.y = verticalInput * speed;

        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;

            }
            //reset current_haz_Num if it is greater than or equal to the max number of hazzards
            if (Current_Haz_Num >= max_Machines_Amnt)
            {
                Current_Haz_Num = 0;
            }

        }

        if (Controlled_Hazard[Current_Haz_Num].transform.position.y >= Hazard_MaxPos[Current_Haz_Num].y)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MaxPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }

        if (Controlled_Hazard[Current_Haz_Num].transform.position.y <= Hazard_MinPos[Current_Haz_Num].y)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                Hazard_MinPos[Current_Haz_Num].y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }


        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }

    void MiddlePlatformBehavior()
    {

        if(middlePlatform_movingUp == true)
        {
            Current_Haz_Num = 0;
        }
        else if(middlePlatform_movingUp == false)
        {
            Current_Haz_Num = 1;
        }

        //switches the trigger that is active (Vacuum or Blow)
        if (myPlayer.GetButtonDown("Special"))
        {
            if (Current_Haz_Num < max_Machines_Amnt)
            {
                Current_Haz_Num++;

            }
            //reset current_haz_Num if it is greater than or equal to the max number of hazzards
            if (Current_Haz_Num >= max_Machines_Amnt)
            {
                Current_Haz_Num = 0;
            }
        }

        //Controlling the MiddlePlatform

        if (Current_Haz_Num == 0 && middlePlatform_blowBoxCollider.enabled == false)    //Blow Active
        {
            middlePlatform_vacuumBoxCollider.enabled = false;
            middlePlatform_blowBoxCollider.enabled = true;
            middlePlatform_movingUp = true;
            
        }
        else if (Current_Haz_Num == 1 && middlePlatform_vacuumBoxCollider.enabled == false)    //Vacuum Active
        {
            middlePlatform_blowBoxCollider.enabled = false;
            middlePlatform_vacuumBoxCollider.enabled = true;
            middlePlatform_movingUp = false;
        }


        //move ship upward
        if (middlePlatform_movingUp == true)
        {
            middlePlatform_rb.MovePosition(middlePlatform.transform.position + middlePlatform.transform.up * middlePlatform_moveSpeed * Time.deltaTime);
        }
        //move ship downward
        else if(middlePlatform_movingUp == false)
        {
            middlePlatform_rb.MovePosition(middlePlatform.transform.position - middlePlatform.transform.up * middlePlatform_moveSpeed * Time.deltaTime);
        }
        
        //if ship reaches its max/min, kick out of machine and set cooldown
        if(middlePlatform.transform.position.y >= 3.0f && middlePlatform_movingUp == true)
        {
            End_Control();
            //The player still has to hit X or O to get off machine.  We need to make a way for the machine to force the player off
                //make Status enum in AlternateSP public?
            middlePlatform_movingUp = false;
            middlePlatform_blowBoxCollider.enabled = false;
        }
        if (middlePlatform.transform.position.y <= 1.0f && middlePlatform_movingUp == false)
        {
            End_Control();
            middlePlatform_movingUp = true;
            middlePlatform_vacuumBoxCollider.enabled = false;
        }

        

    }

    //--------------------------------------------------------------------------------------------------
    /// <summary>
    ///
    //This Function requires access to the correct player number in order to have the correct player interact with the hazzards.
    /// </summary>
    /// <param name="playerNum">Player ID from the support character that is activating this machine.</param>
    /// <param name="teamID">Player's Team ID</param>
    public void Commence_Control(int playerNum, int teamID)
    {
        // Recieve the number of a player and use it as my inputs.
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);

        is_In_Use = true;

        StartCoroutine(waitForUse());

        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazard[0].SetActive(true);
            switch (teamID)
            {
                case 2:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }
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
        // The playerID "-1" does not exist, therefore, the inputs will never be recieved.
        myPlayer = ReInput.players.GetPlayer(-1);
        
        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazard[0].SetActive(false);
        }
        if(mach == MachineID.MiddlePlatform)
        {
            middlePlatform_blowBoxCollider.enabled = false;
            middlePlatform_vacuumBoxCollider.enabled = false;
        }
        
        Debug.Log("Player has deactivated machine: "+transform.name);
    }

    //--------------------------------------------------------------------------------------------------
    //functions for side hazards created by Justin
    IEnumerator SetInMachineTrueDelay()
    {
        yield return new WaitForSeconds(.1f);
        sideHazardMachineEnabled = true;
        SupportPlayer.movementEnabled = false;
        Controlled_Hazard[Current_Haz_Num].transform.position = new Vector2(Controlled_Hazard[Current_Haz_Num].transform.position.x + 0.25f,
            Controlled_Hazard[Current_Haz_Num].transform.position.y);
    }



    //--------------------------------------------------------------------------------------------------
    // This function just draws Gizmos in unity
    private void OnDrawGizmos()
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


    IEnumerator waitForUse()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Now can use");
        can_Use = true;
        other_can_Use = true;
    }

}
