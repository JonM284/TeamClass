using System.Collections;
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
    public enum MachineID { Two_Fairy, Two_BottomHazard, Two_TopHazard, Two_Mushrooms, Two_Squirrel };
    [Header("Machine Type")]
    [Tooltip("What type of machine will this be?")]
    public MachineID mach;
    public Vector2 baseScale, flippedScale;


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

    //cooldowns
    private float cooldownTimer_Fairy;
    private float cooldownLength_Fairy;
    private bool Fairy_Machine_Ready;

    private float cooldownTimer_TwoBottomHazard;
    private float cooldownLength_TwoBottomHazard;
    private bool Two_BottomHazard_Machine_Ready;

    private float cooldownTimer_TwoTopHazard;
    private float cooldownLength_TwoTopHazard;
    private bool Two_TopHazard_Machine_Ready;

    private float cooldownTimer_Mushrooms;
    private float cooldownLength_Mushrooms;
    private bool Mushrooms_Machine_Ready;

    private float cooldownTimer_Squirrel;
    private float cooldownLength_Squirrel;
    private bool Squirrel_Machine_Ready;

    public List<Vector3> Hazard_StartPos;
    public List<Vector3> Hazard_MaxPos;
    public List<Vector3> Hazard_MinPos;

    public GameObject my_Controller_Player;
    public GameObject[] machine_UI;
    public GameObject[] indicator_Images;

    private Animator mushroomBounce2;
    private Animator mushroomSpores;

    [Header("Audio")]
    public AudioClip[] machineSounds;
    public AudioSource machineSoundPlayer;

    private Color m_Team_One_Color, m_Team_Two_Color;

    private void Awake()
    {
        mushroomBounce2 = GameObject.Find("Walking Mushroom_bounce").GetComponent<Animator>();
        mushroomSpores = GameObject.Find("Walking Mushroom_spores").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Team_One_Color = Color.cyan;
        m_Team_Two_Color = Color.red;

        baseScale = mushroomBounce2.gameObject.transform.localScale;
        flippedScale = new Vector2(baseScale.x * -1, baseScale.y);
        indicator_Images[0].SetActive(true);
        indicator_Images[1].SetActive(false);
        //reset for jon
        originalRotation = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.rotation.x,
            Controlled_Hazard[Current_Haz_Num].transform.rotation.y,
            Controlled_Hazard[Current_Haz_Num].transform.rotation.z);

        Move_Rotation = new Vector3(0, 0, 90);
        //get an instance of the object spawner so we can spawn objects
        objectPool = ObjectSpawner.Instance;
        my_Controller_Player = null;
        for (int i = 0; i < machine_UI.Length; i++)
        {
            machine_UI[i].SetActive(false);
        }
        //only do this if this machine is of type "Background Cannon" 
        if (mach == MachineID.Two_Fairy)
        {
            Controlled_Hazard[0].SetActive(false);
        }


        //resetting cooldowns
        //IF MAKING COOLDOWN 3 SECONDS INSTEAD, OTHER MACHINES HAVE TO LAST LESS THAN 3 SECONDS (TREE, MUSHROOMS)
        cooldownTimer_Fairy = 0f;
        cooldownLength_Fairy = 5f;
        Fairy_Machine_Ready = true;

        cooldownTimer_TwoBottomHazard = 0f;
        cooldownLength_TwoBottomHazard = 5f;
        Two_BottomHazard_Machine_Ready = true;

        cooldownTimer_TwoTopHazard = 0f;
        cooldownLength_TwoTopHazard = 5f;
        Two_TopHazard_Machine_Ready = true;

        cooldownTimer_Mushrooms = 0f;
        cooldownLength_Mushrooms = 5f;
        Mushrooms_Machine_Ready = true;

        cooldownTimer_Squirrel = 0f;
        cooldownLength_Squirrel = 3f;
        Squirrel_Machine_Ready = true;

        machineSoundPlayer = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //fairy cooldown
        if (Fairy_Machine_Ready == false)
        {
            cooldownTimer_Fairy -= Time.deltaTime;
            if (cooldownTimer_Fairy <= 0)
            {
                Fairy_Machine_Ready = true;
                indicator_Images[0].SetActive(true);
                indicator_Images[1].SetActive(false);

                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //Bottom Hazard Cooldown
        if (Two_BottomHazard_Machine_Ready == false)
        {
            cooldownTimer_TwoBottomHazard -= Time.deltaTime;
            if(cooldownTimer_TwoBottomHazard <= 0)
            {
                Two_BottomHazard_Machine_Ready = true;
                indicator_Images[0].SetActive(true);
                indicator_Images[1].SetActive(false);
                machineSoundPlayer.clip = machineSounds[1];
                machineSoundPlayer.Play();
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //Top Hazard Cooldown
        if (Two_TopHazard_Machine_Ready == false)
        {
            cooldownTimer_TwoTopHazard -= Time.deltaTime;
            if (cooldownTimer_TwoTopHazard <= 0)
            {
                Two_TopHazard_Machine_Ready = true;
                indicator_Images[0].SetActive(true);
                indicator_Images[1].SetActive(false);
                machineSoundPlayer.clip = machineSounds[1];
                machineSoundPlayer.Play();
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //Mushrooms Cooldown
        if (Mushrooms_Machine_Ready == false)
        {
            cooldownTimer_Mushrooms -= Time.deltaTime;
            if (cooldownTimer_Mushrooms <= 0)
            {
                Mushrooms_Machine_Ready = true;
                indicator_Images[0].SetActive(true);
                indicator_Images[1].SetActive(false);
                machineSoundPlayer.clip = machineSounds[1];
                machineSoundPlayer.Play();
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //Squirrel Cooldown
        if (Squirrel_Machine_Ready == false)
        {
            cooldownTimer_Squirrel -= Time.deltaTime;
            if (cooldownTimer_Squirrel <= 0)
            {
                Squirrel_Machine_Ready = true;
                indicator_Images[0].SetActive(true);
                indicator_Images[1].SetActive(false);
                machineSoundPlayer.clip = machineSounds[1];
                machineSoundPlayer.Play();
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
            }
            else if (mach == MachineID.Two_TopHazard)
            {
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                Two_TopHazardControl();
            }
            else if (mach == MachineID.Two_Mushrooms)
            {
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                MushroomMachine();
            }
            else if (mach == MachineID.Two_Squirrel)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                SideCannonMovement();
            }
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

        /*if(vel.x < 0)
        {
            Controlled_Hazard[0].GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (vel.x > 0)
        {
            Controlled_Hazard[0].GetComponent<SpriteRenderer>().flipX = false;
        }*/

        //regular moving noise -G
        machineSoundPlayer.clip = machineSounds[1];
        machineSoundPlayer.Play();


        //if fairy hits a player
        if (fairyScript.fairyHitPlayer == true)
        {
            indicator_Images[0].SetActive(false);
            indicator_Images[1].SetActive(true);
            StartCoroutine(FairyResetInitialDelay());
            End_Control();
            Fairy_Machine_Ready = false;
            cooldownTimer_Fairy = cooldownLength_Fairy;
            GetComponent<BoxCollider2D>().enabled = false;

            //hit player noise -G
            machineSoundPlayer.clip = machineSounds[2];
            machineSoundPlayer.Play();

        }

        //this allows the player to move the crosshair
        //Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
           // + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);

        /*//Moving hazard left and righjt, with ranges stopping movement on walls
        if (Controlled_Hazard[0].transform.position.y > -2.15f && Controlled_Hazard[0].transform.position.y < 4.5f)
        {
            Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
        }
        else if (Controlled_Hazard[0].transform.position.y <= -2.15f)
        {
            if (verticalInput > 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }
        else if (Controlled_Hazard[0].transform.position.y >= -2.15f)
        {
            if (verticalInput < 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }*/

        if(Controlled_Hazard[0].transform.position.y < -2.15f)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                -2.15f, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        if(Controlled_Hazard[0].transform.position.y > 4.5f)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(Controlled_Hazard[Current_Haz_Num].transform.position.x,
                4.5f, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        if(Controlled_Hazard[0].transform.position.x < -7.4f)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(-7.4f,
                Controlled_Hazard[0].transform.position.y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        if (Controlled_Hazard[0].transform.position.x > 7.4f)
        {
            Controlled_Hazard[Current_Haz_Num].transform.position = new Vector3(7.4f,
                Controlled_Hazard[0].transform.position.y, Controlled_Hazard[Current_Haz_Num].transform.position.z);
        }
        //this allows the player to move the crosshair
        Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[Current_Haz_Num].GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }

    IEnumerator FairyResetInitialDelay()
    {
        yield return new WaitForSeconds(1.0f);
        fairyScript.gameObject.SetActive(false);
        fairyScript.fairyHitPlayer = false;
        fairyScript.transform.position = fairyScript.startPos;
        fairyScript.gameObject.GetComponent<ParticleSystem>().Stop();
    }

    //Spikes/Tree Bottom Hazard Support Machine
    void Two_BottomHazardControl()
    {
        //Moving the spikes/tree left/right before activating it
        vel.x = horizontalInput * speed;
        //this allows players to change which bottom hazard is currently selected (spikes/tree)
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

            //switch spikes for tree
            if (Current_Haz_Num == 1)
            {
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<SpriteRenderer>().enabled = false;
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<BoxCollider2D>().enabled = false;
                //Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.transform.position = Controlled_Hazard[0].GetComponent<Spike_Movement>().myStartPos;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<SpriteRenderer>().enabled = true;   
            }
            //switch tree for spikes
            else
            {
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<SpriteRenderer>().enabled = true;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<SpriteRenderer>().enabled = false;
                //Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<PolygonCollider2D>().enabled = false;
                //Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.transform.position = Controlled_Hazard[1].GetComponent<Tree_Movement>().myStartPos;
            }


            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                    break;
                case 1:
                    Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                    break;
                default:
                    Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }

        }
        
        //player activates (sends out) hazard (spikes/tree)
        if(myPlayer.GetButton("BasicAttack") && can_Use)
        {
            indicator_Images[0].SetActive(false);
            indicator_Images[1].SetActive(true);
            //if spikes
            if (Current_Haz_Num == 0)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Spike_Movement>().Spike_Active = true;
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<BoxCollider2D>().enabled = true;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.transform.position = Controlled_Hazard[1].GetComponent<Tree_Movement>().myStartPos;


                machineSoundPlayer.clip = machineSounds[8];
                machineSoundPlayer.Play();

            }
            //if tree
            else if(Current_Haz_Num == 1)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Tree_Movement>().Tree_Active = true;
                //Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<BoxCollider2D>().enabled = true;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<PolygonCollider2D>().enabled = true;
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.transform.position = Controlled_Hazard[0].GetComponent<Spike_Movement>().myStartPos;


                machineSoundPlayer.clip = machineSounds[9];
                machineSoundPlayer.Play();

            }

            //kick player off machine and put it on cooldown
            End_Control();
            Two_BottomHazard_Machine_Ready = false;
            cooldownTimer_TwoBottomHazard = cooldownLength_TwoBottomHazard;
            GetComponent<BoxCollider2D>().enabled = false;

        }
       
        //Moving hazard left and righjt, with ranges stopping movement on walls
        if(Controlled_Hazard[0].transform.position.x > -7.4f && Controlled_Hazard[0].transform.position.x < 7.4f)
        {
            Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
        }
        else if(Controlled_Hazard[0].transform.position.x <= -7.4f)
        {
            if(horizontalInput > 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }
        else if (Controlled_Hazard[0].transform.position.x >= -7.4f)
        {
            if (horizontalInput < 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }

    }

    //Branbull/Apples Support Machine
    public void Two_TopHazardControl()
    {
        //Moving the branbull/apple left/right before activating it
        vel.x = horizontalInput * speed;
        //this allows players to change which bottom hazard is currently selected (branbull/apple)
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

            //switch branbull for apple
            if (Current_Haz_Num == 1)
            {
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<SpriteRenderer>().enabled = false;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().branbullExploded.GetComponent<SpriteRenderer>().enabled = false;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().branbullExploded.GetComponent<BoxCollider2D>().enabled = false;
                //Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.transform.position = Controlled_Hazard[0].GetComponent<Branbull_Movement>().myStartPos;
                foreach (SpriteRenderer apple in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleSprites)
                {
                    apple.enabled = true;
                }
                switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
                {
                    case 2:
                        foreach (SpriteRenderer apple in Controlled_Hazard[Current_Haz_Num].GetComponent<Apple_Movement>().appleSprites)
                        {
                            apple.color = m_Team_Two_Color;
                        }
                        //Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                        break;
                    case 1:
                        foreach (SpriteRenderer apple in Controlled_Hazard[Current_Haz_Num].GetComponent<Apple_Movement>().appleSprites)
                        {
                            apple.color = m_Team_One_Color;
                        }
                        //Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                        break;
                    default:
                        foreach (SpriteRenderer apple in Controlled_Hazard[Current_Haz_Num].GetComponent<Apple_Movement>().appleSprites)
                        {
                            apple.color = Color.black;
                        }
                        //Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                }
            }
            //switch apple for branbull
            else
            {
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<SpriteRenderer>().enabled = true;
                foreach (SpriteRenderer apple in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleSprites)
                {
                    apple.enabled = false;
                }
                foreach (CircleCollider2D appleCollider in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleColliders)
                {
                    appleCollider.enabled = false;
                }
                //Controlled_Hazard[1].GetComponent<Apple_Movement>().myApple.transform.position = Controlled_Hazard[1].GetComponent<Apple_Movement>().myStartPos;
                switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
                {
                    case 2:
                        Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                        break;
                    case 1:
                        Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                        break;
                    default:
                        Controlled_Hazard[Current_Haz_Num].GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                }
            }

           

        }

        //player activates (sends out) hazard (branbull/apple)
        if (myPlayer.GetButton("BasicAttack") && can_Use)
        {
            indicator_Images[0].SetActive(false);
            indicator_Images[1].SetActive(true);
            //if branbull
            if (Current_Haz_Num == 0)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Branbull_Movement>().Branbull_Active = true;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<BoxCollider2D>().enabled = true;
                Controlled_Hazard[1].GetComponent<Apple_Movement>().myApple.transform.position = Controlled_Hazard[1].GetComponent<Apple_Movement>().myStartPos;


                machineSoundPlayer.clip = machineSounds[3];
                machineSoundPlayer.Play();

            }
            //if apple
            else if (Current_Haz_Num == 1)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Apple_Movement>().Apple_Active = true;
                foreach (CircleCollider2D appleCollider in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleColliders)
                {
                    appleCollider.enabled = true;
                }
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.transform.position = Controlled_Hazard[0].GetComponent<Branbull_Movement>().myStartPos;


                machineSoundPlayer.clip = machineSounds[4];
                machineSoundPlayer.Play();

            }

            //kick player off machine and put it on cooldown
            End_Control();
            Two_TopHazard_Machine_Ready = false;
            cooldownTimer_TwoTopHazard = cooldownLength_TwoTopHazard;
            GetComponent<BoxCollider2D>().enabled = false;

        }


        //Moving hazard left and righjt, with ranges stopping movement on walls
        if (Controlled_Hazard[0].transform.position.x > -7.4f && Controlled_Hazard[0].transform.position.x < 7.4f)
        {
            Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
        }
        else if (Controlled_Hazard[0].transform.position.x <= -7.4f)
        {
            if (horizontalInput > 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }
        else if (Controlled_Hazard[0].transform.position.x >= -7.4f)
        {
            if (horizontalInput < 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }

    }

    //controls the mushroom machine: Activate machine, toggle through two mushrooms (bounce pad/spores), move mushroom left and right, pick mushroom -> activate mushroom
    public void MushroomMachine()
    {
        mushroomBounce2.SetFloat("velocity", Mathf.Abs(vel.x));
        mushroomSpores.SetFloat("velocity", Mathf.Abs(vel.x));
        if(vel.x > 0)
        {
            mushroomBounce2.gameObject.transform.localScale = flippedScale;
            mushroomSpores.gameObject.transform.localScale = flippedScale;
        }
        else if(vel.x < 0)
        {
            mushroomBounce2.gameObject.transform.localScale = baseScale;
            mushroomSpores.gameObject.transform.localScale = baseScale;
        }
        //Moving the mushroom left/right before activating it
        vel.x = horizontalInput * speed;
        //this allows players to change which mushroom is currently selected (bounce pad/spores)
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

            //switch bounce for spores
            if (Current_Haz_Num == 1)
            {
                //Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.GetComponent<SpriteRenderer>().enabled = false;
                foreach (BoxCollider2D mushroomBounceCollider in Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBounceColliders)
                {
                    mushroomBounceCollider.enabled = false;
                }
                //Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.transform.position = Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myStartPos;
                //Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.GetComponent<SpriteRenderer>().enabled = true;
                Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBouncePrefab.SetActive(false);
                Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSpores.SetActive(true);
                //SET WALKING ANIMATION SPORES
            }
            //switch spores for bounce pad
            else
            {
                //Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.GetComponent<SpriteRenderer>().enabled = true;
                //Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.GetComponent<SpriteRenderer>().enabled = false;
                foreach (BoxCollider2D mushroomSporesCollider in Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSporesColliders)
                {
                    mushroomSporesCollider.enabled = false;
                }
                Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBouncePrefab.SetActive(true);
                Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSpores.SetActive(false);
                //Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.transform.position = Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myStartPos;
                //SET WALKING ANIMATION BOUNCE PAD
            }
        }

        //player activates bounce pad / spores
        if (myPlayer.GetButton("BasicAttack") && can_Use)
        {
            indicator_Images[0].SetActive(false);
            indicator_Images[1].SetActive(true);
            //if bounce pad
            if (Current_Haz_Num == 0)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_BouncePad>().MushroomBounce_Active = true;
                foreach (BoxCollider2D mushroomBounceCollider in Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBounceColliders)
                {
                    mushroomBounceCollider.enabled = true;
                }
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_BouncePad>().removeMushroomBounceTimer = Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_BouncePad>().removeMushroomBounceLength;
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_BouncePad>().MushroomBounceAreThere = true;
                Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.transform.position = Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myStartPos;


                machineSoundPlayer.clip = machineSounds[5];
                machineSoundPlayer.Play();
                //SET IDLE ANIMATION BOUNCE PAD 
            }
            //if spores
            else if (Current_Haz_Num == 1)
            {
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_Spores>().MushroomSpores_Active = true;
                foreach (BoxCollider2D mushroomSporesCollider in Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSporesColliders)
                {
                    mushroomSporesCollider.enabled = true;
                }
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_Spores>().removeMushroomSporesTimer = Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_Spores>().removeMushroomSporesLength;
                Controlled_Hazard[Current_Haz_Num].GetComponent<Mushroom_Spores>().MushroomSporesAreThere = true;
                Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.transform.position = Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myStartPos;


                machineSoundPlayer.clip = machineSounds[6];
                machineSoundPlayer.Play();
                //SET IDLE ANIMATION SPORES
            }

            //kick player off machine and put it on cooldown
            End_Control();
            Mushrooms_Machine_Ready = false;
            cooldownTimer_Mushrooms = cooldownLength_Mushrooms;
            GetComponent<BoxCollider2D>().enabled = false;

        }

        //Moving hazard left and righjt, with ranges stopping movement on walls
        if (Controlled_Hazard[0].transform.position.x > -7.4f && Controlled_Hazard[0].transform.position.x < 7.4f)
        {
            Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
        }
        else if (Controlled_Hazard[0].transform.position.x <= -7.4f)
        {
            if (horizontalInput > 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
            }
        }
        else if (Controlled_Hazard[0].transform.position.x >= -7.4f)
        {
            if (horizontalInput < 0)
            {
                Controlled_Hazard[0].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[0].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
                Controlled_Hazard[1].GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazard[1].GetComponent<Rigidbody2D>().position
                    + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
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
            Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.white;
            if (Current_Haz_Num < max_Machines_Amnt - 1)
            {
                Current_Haz_Num++;
            }
            else if (Current_Haz_Num == max_Machines_Amnt - 1)
            {
                Current_Haz_Num = 0;
            }
            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.black;
                    break;
            }
            //Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Do_Flash();
        }

        if (myPlayer.GetButtonDown("BasicAttack") && is_In_Use)
        {
            if (Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {
                objectPool.SpawnFromPool("CannonBall_Move_Right", Controlled_Hazard[Current_Haz_Num].transform.position,
                    Quaternion.Euler(Move_Rotation));
            }
            if (!Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {
                objectPool.SpawnFromPool("CannonBall_Move_Left", Controlled_Hazard[Current_Haz_Num].transform.position,
                    Quaternion.Euler(Move_Rotation));
            }
            indicator_Images[0].SetActive(false);
            indicator_Images[1].SetActive(true);
            End_Control();
            Squirrel_Machine_Ready = false;
            cooldownTimer_Squirrel = cooldownLength_Squirrel;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        

        if (Current_Haz_Num > max_Machines_Amnt - 1)
        {
            Current_Haz_Num = 0;
            switch (my_Controller_Player.GetComponent<AlternateSP>().teamID)
            {
                case 2:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.black;
                    break;
            }
        }


        if (verticalInput > 0.1f && Move_Rotation.z < Max_range)
        {
            if (Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {
                Move_Rotation.z += Time.deltaTime * speed;
            }
            if (!Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {
                Move_Rotation.z -= Time.deltaTime * speed;
            }
        }
        if (verticalInput < -0.1f && Move_Rotation.z > -Max_range)
        {

            if (Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {

                Move_Rotation.z -= Time.deltaTime * speed;
            }
            if (!Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Is_Facing_Right)
            {
                Move_Rotation.z += Time.deltaTime * speed;
            }
        }


        if (Move_Rotation.z > Max_range)
        {
            Move_Rotation.z = Max_range - 0.01f;
        }
        if (Move_Rotation.z < -Max_range)
        {
            Move_Rotation.z = -Max_range + 0.01f;
        }

        Controlled_Hazard[Current_Haz_Num].transform.rotation = Quaternion.Euler(Move_Rotation);

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
            //Controlled_Hazard[0].GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer fairySpriteChildren in Controlled_Hazard[0].GetComponentsInChildren<SpriteRenderer>())
            {
                fairySpriteChildren.enabled = true;
            }
            Controlled_Hazard[0].GetComponent<BoxCollider2D>().enabled = true;
            fairyScript.gameObject.GetComponent<ParticleSystem>().Play();
        }
        if(mach == MachineID.Two_BottomHazard)
        {

            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                    break;
                case 1:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                    break;
                default:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }

            Current_Haz_Num = 0;
            Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<SpriteRenderer>().enabled = true;
            Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<BoxCollider2D>().enabled = false;
            Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<SpriteRenderer>().enabled = false;
            //Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<BoxCollider2D>().enabled = false;
            Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<PolygonCollider2D>().enabled = false;
        }
        if (mach == MachineID.Two_TopHazard)
        {

            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                    break;
                case 1:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                    break;
                default:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }

            Current_Haz_Num = 0;
            Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<SpriteRenderer>().enabled = true;
            Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<BoxCollider2D>().enabled = false;
            Controlled_Hazard[0].GetComponent<Branbull_Movement>().branbullExploded.GetComponent<BoxCollider2D>().enabled = false;
            foreach (SpriteRenderer apple in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleSprites)
            {
                    apple.enabled = false;
            }
            foreach (CircleCollider2D appleCollider in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleColliders)
            {
                appleCollider.enabled = false;
            }
        }

        if(mach == MachineID.Two_Mushrooms)
        {

            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_Two_Color;
                    break;
                case 1:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = m_Team_One_Color;
                    break;
                default:
                    Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }

            Current_Haz_Num = 0;
            //Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.GetComponent<SpriteRenderer>().enabled = true;
            //Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.GetComponent<SpriteRenderer>().enabled = false;
            foreach (BoxCollider2D mushroomSporesCollider in Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSporesColliders)
            {
                mushroomSporesCollider.enabled = false;
            }
            Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.transform.position = Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myStartPos;
            Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBouncePrefab.SetActive(true);
        }

        if (mach == MachineID.Two_Squirrel)
        {
            //Controlled_Hazard[Current_Haz_Num].GetComponent<Side_Cannon_Behaviour>().Do_Flash();
            switch (my_Controller_Player.GetComponent<AlternateSP2>().teamID)
            {
                case 2:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazard[Current_Haz_Num].GetComponentInParent<SpriteRenderer>().color = Color.black;
                    break;
            }
        }

        machineSoundPlayer.clip = machineSounds[7];
        machineSoundPlayer.Play();

        Debug.Log("Player:"+playerNum+ " has activated hazzard: "+mach);
        machine_UI[0].SetActive(false);
        machine_UI[1].SetActive(false);
        for (int i = 2; i < machine_UI.Length; i++)
        {
            machine_UI[i].SetActive(true);
        }
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
        myPlayer = ReInput.players.GetPlayer(5);
        for (int i = 0; i < machine_UI.Length; i++)
        {
            machine_UI[i].SetActive(false);
        }
        if (mach == MachineID.Two_Fairy)
        {
            //Controlled_Hazard[0].transform.position = fairyScript.startPos;
            //Controlled_Hazard[0].SetActive(false);
            //Controlled_Hazard[0].GetComponent<SpriteRenderer>().enabled = false;
            foreach (SpriteRenderer fairySpriteChildren in Controlled_Hazard[0].GetComponentsInChildren<SpriteRenderer>())
            {
                fairySpriteChildren.enabled = false;
            }
            Controlled_Hazard[0].GetComponent<BoxCollider2D>().enabled = false;
            //fairyScript.gameObject.GetComponent<ParticleSystem>().Pause();

            if(Controlled_Hazard[0].GetComponent<FairyScript>().fairyHitPlayer == false)
            {
                //Controlled_Hazard[0].GetComponent<SpriteRenderer>().enabled = false;
                foreach (SpriteRenderer fairySpriteChildren in Controlled_Hazard[0].GetComponentsInChildren<SpriteRenderer>())
                {
                    fairySpriteChildren.enabled = false;
                }
                Controlled_Hazard[0].GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[0].GetComponent<FairyScript>().transform.position = Controlled_Hazard[0].GetComponent<FairyScript>().startPos;
                Controlled_Hazard[0].gameObject.GetComponent<ParticleSystem>().Stop();
            }

        }


        if(mach == MachineID.Two_BottomHazard)
        {
            //if just leaving machine and not activating either spikes or tree, disable sprites and colliders of spikes/trees | if one of them is active, you don't want to do anything
            if (Controlled_Hazard[0].GetComponent<Spike_Movement>().Spike_Active == false && Controlled_Hazard[1].GetComponent<Tree_Movement>().Tree_Active == false)
            {
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<SpriteRenderer>().enabled = false;
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[0].GetComponent<Spike_Movement>().mySpike.transform.position = Controlled_Hazard[0].GetComponent<Spike_Movement>().myStartPos;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<SpriteRenderer>().enabled = false;
                //Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.GetComponent<PolygonCollider2D>().enabled = true;
                Controlled_Hazard[1].GetComponent<Tree_Movement>().myTree.transform.position = Controlled_Hazard[1].GetComponent<Tree_Movement>().myStartPos;
            } 
            if(Current_Haz_Num == 0)
            {
                Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (Current_Haz_Num == 1)
            {
                Controlled_Hazard[1].GetComponent<SpriteRenderer>().color = Color.white;
            }

        }
        if (mach == MachineID.Two_TopHazard)
        {
            //if just leaving machine and not activating either branbull or apple, disable sprites and colliders of branbull/apple | if one of them is active, you don't want to do anything
            if (Controlled_Hazard[0].GetComponent<Branbull_Movement>().Branbull_Active == false && Controlled_Hazard[1].GetComponent<Apple_Movement>().Apple_Active == false)
            {
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<SpriteRenderer>().enabled = false;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.GetComponent<BoxCollider2D>().enabled = false;
                Controlled_Hazard[0].GetComponent<Branbull_Movement>().myBranbull.transform.position = Controlled_Hazard[0].GetComponent<Branbull_Movement>().myStartPos;
                foreach (SpriteRenderer apple in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleSprites)
                {
                    apple.enabled = false;
                }
                foreach (CircleCollider2D appleCollider in Controlled_Hazard[1].GetComponent<Apple_Movement>().appleColliders)
                {
                    appleCollider.enabled = false;
                }
                Controlled_Hazard[1].GetComponent<Apple_Movement>().myApple.transform.position = Controlled_Hazard[1].GetComponent<Apple_Movement>().myStartPos;
            }
            if(Current_Haz_Num == 0)
            {
                Controlled_Hazard[0].GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if(Current_Haz_Num == 1)
            {
                foreach (SpriteRenderer apple in Controlled_Hazard[Current_Haz_Num].GetComponent<Apple_Movement>().appleSprites)
                {
                    apple.color = Color.white;
                }
            }



        }
        //if just leaving machine and not activating either mushroom, disable sprites and colliders of mushrooms | if one of them is active, you don't want to do anything
        if (mach == MachineID.Two_Mushrooms)
        {
            if (Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().MushroomBounce_Active == false && Controlled_Hazard[1].GetComponent<Mushroom_Spores>().MushroomSpores_Active == false)
            {
                //Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.GetComponent<SpriteRenderer>().enabled = false;
                foreach (BoxCollider2D mushroomBounceCollider in Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBounceColliders)
                {
                    mushroomBounceCollider.enabled = false;
                }
                Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myMushroomBounce.transform.position = Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().myStartPos;

                //Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.GetComponent<SpriteRenderer>().enabled = false;
                foreach (BoxCollider2D mushroomSporesCollider in Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSporesColliders)
                {
                    mushroomSporesCollider.enabled = false;
                }
                Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myMushroomSpores.transform.position = Controlled_Hazard[1].GetComponent<Mushroom_Spores>().myStartPos;
                Controlled_Hazard[0].GetComponent<Mushroom_BouncePad>().mushroomBouncePrefab.SetActive(false);
                Controlled_Hazard[1].GetComponent<Mushroom_Spores>().mushroomSpores.SetActive(false);
            }


            
        }
        if (mach == MachineID.Two_Squirrel)
        {
            for (int i = 0; i < Controlled_Hazard.Length; i++)
            {
                Controlled_Hazard[i].GetComponentInParent<SpriteRenderer>().color = Color.white;
            }
        }

        Debug.Log("Player has deactivated machine: "+transform.name);
    }

    IEnumerator waitForUse()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Now can use");
        can_Use = true;
        other_can_Use = true;


        machineSoundPlayer.clip = machineSounds[0];
        machineSoundPlayer.Play();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!is_In_Use && other.gameObject.tag == "Player")
        {
            machine_UI[0].SetActive(true);
            machine_UI[1].SetActive(true);
            Debug.Log("Show UI");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!is_In_Use && other.gameObject.tag == "Player")
        {
            machine_UI[0].SetActive(false);
            machine_UI[1].SetActive(false);
        }
    }

}
