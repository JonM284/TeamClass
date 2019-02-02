using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class Movement : MonoBehaviour
{
    /* please put your name at the end of important comments so that we know who wrote what, so that if one of us has a problem understanding
     * what the code does, we can directly ask the person, instead of asking everyone
     * also im putting this block comment at the top of every script, sorry if it gets repetitive
     * -Ganderman Dan 🦆
     */

    /* this script should be where all the movement related stuff should go. 
     * I think we should also put knockback related stuff here since that has to do with movement. 
     *            (ex. angle you fly when you get hit, and how far you fly back when you get hit, etc.)
     * But perhaps a separate script could make thinks more organized
     * We'll figure it out when we get there I guess :)  -Ganderman Dan  🦆
     */

    //If you make a new variable or script, please explain what it does in their respective reference sheets, just so every understands what each variable is used for -Ganderman Dan 🦆

    [Header("This is for testing purposes, it will not stay like this")]
        //I will make it so when you choose your character depending on what team your on, your tag will change accordingly, this is more hitbox collision purposes")]
    public bool isBot;

    [Header("Jump & Gravity Modifiers")]
    public float aerialJumpForce;
    public float groundedJumpForce;
    public float maxJumps;
    public float holdJumpForce;
    [Tooltip("How long you continue to rise while holding the jump button")]
    public float maxHoldJumpTime;
    [Tooltip("How strong the gravity is while the player is moving upward")]
    public float upwardGravity;
    [Tooltip("How strong the gravity is while the player is moving downward")]
    public float downwardGravity;
    public float maxDownwardVelocity;

    [Header("Movement")]
    public float onGroundTimerMax;
    [HideInInspector]
    public float onGroundTimer;
    public float groundSpeed;
    public float maxGroundVelocity;
    [Tooltip("How fast you stop")]
    public float haltSpeed;
    public float attackHaltSpeed;
    [Tooltip("How quickly you can stop moving one direction to go the other")]
    public float turnSpeed;
    public float airSpeed;
    public float maxAirVelocity;
    [Tooltip("Make this number 1, this should only change when dealing with environmental hazards that alter your movement")]
    public float movementMultiplier;
    [Tooltip("A little push when you initiate movement or switch directions while on the ground")]
    public float groundedThrustForce;
    [Tooltip("A little push when you initiate movement or switch directions while in the air")]
    public float aerialThrustForce;



    [Header("Other Character Attributes")]
    public float weight;  
    public bool canWallCling;
    [Tooltip("How far the joystick needs to be pushed before the character stops walking and starts running. Number must be between 0 and 1 (Thrust will also be applied at this speed")]
    public float runTransitionAxis;
    [HideInInspector]
    public float hitStun;
    


    //attack variables
    private bool isAttackingOnGround = false;
    private bool isAttackingInAir = false;



    //private movement checkers
    private bool moveRight = false;
    private bool moveLeft = false;
    private bool isTouchingGround;
    private float horAxisPos;
    private bool thrustReset = true;
    private bool thrustActive = true;
    private bool activateJump = false;
    private float currentJumps;
    private float currentHoldJumpTimer = 0;

    private float xScale;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private BasicAttack ba;

    public RaycastHit2D raycast;

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;


    void Awake()
    {
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ba = GetComponent<BasicAttack>();

        currentJumps = maxJumps;

        xScale = gameObject.transform.localScale.x;

        if(xScale < 0)
        {
            xScale *= -1;
        }
    }

    /* if you're doing something with the rigidbody and movement, do it here, it's better
     * BUT DONT DO PLAYER INPUT HERE, do input in Update(), and the physics here in FixedUpdate()
     * If that confuses you, heres an example. In Update, set a bool called moveRight == true if 'd' is pressed. Then in FixedUpdate(), do if(moveRight == true){ physics crap etc. }
     * check here for reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
     * more about the differnces between the two update functions https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
     * -Ganderman Dan 🦆
     */
    void FixedUpdate()
    {
        if(currentHoldJumpTimer > 0)
        {
            rb.AddForce(transform.up * (holdJumpForce));
        }

        //grounded movement stuff
        if (isTouchingGround)
        {

            if (moveRight) //movement going right
            {
                if (!isAttackingInAir && !isAttackingOnGround)
                {
                    anim.SetInteger("State", 1);
                   // if (Input.GetAxis("Horizontal") > .1f)
                    //{
                        gameObject.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
                        //sr.flipX = false;
                   // }
                }

                if (thrustReset && horAxisPos > runTransitionAxis && rb.velocity.x < maxGroundVelocity * .75f)
                {
                    rb.AddForce(transform.right * (groundedThrustForce / movementMultiplier));
                    thrustReset = false;
                }
                if (rb.velocity.x < maxGroundVelocity * horAxisPos)   //movement takes into account how far the joystick is moved
                {
                    if (rb.velocity.x < -1)    //helps you quickly change direction
                    { 
                        rb.AddForce(transform.right * (groundSpeed * horAxisPos) * turnSpeed);
                    }   
                    else { rb.AddForce(transform.right * (groundSpeed * (horAxisPos / movementMultiplier))); } //if your not trying to change direction, movement is done here
                }
            }else if (moveLeft) //movement going left
            {
                if (!isAttackingInAir && !isAttackingOnGround)
                {
                    anim.SetInteger("State", 1);
                   // if (Input.GetAxis("Horizontal") < -.1f)
                   // {
                        gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
                        //sr.flipX = true;
                    //}
                }

                if (thrustReset && horAxisPos < -runTransitionAxis && rb.velocity.x > -maxGroundVelocity * .75f)
                {
                    rb.AddForce(-transform.right * (groundedThrustForce / movementMultiplier));
                    thrustReset = false;
                }
                if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(maxGroundVelocity * horAxisPos))//movement takes into account how far the joystick is moved -Ganderman Dan 🦆
                {
                    if (rb.velocity.x > 1)  //helps you quickly change direction
                    { 
                        rb.AddForce(-transform.right * Mathf.Abs(groundSpeed * horAxisPos) * turnSpeed);
                    }
                    else { rb.AddForce(-transform.right * Mathf.Abs(groundSpeed * (horAxisPos / movementMultiplier))); } //if your not trying to change direction, movement is done here
                }
            }
            else
            {
                /*
                 * this determines your 'slideyness' when you put the joystick back into neutral position
                 * the higher the halt speed the faster you stop
                 * the lower the halt speed that most slippery you slide
                 * -GandermanDan 🦆
                 */
                if (rb.velocity.x > 1)
                {
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        rb.AddForce(-transform.right * (haltSpeed));
                    }
                    else
                    {
                        rb.AddForce(-transform.right * (attackHaltSpeed));
                    }
                }
                else if(rb.velocity.x < -1)
                {
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        rb.AddForce(transform.right * (haltSpeed));
                    }
                    else
                    {
                        rb.AddForce(transform.right * (attackHaltSpeed));
                    }
                }
                else
                {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        anim.SetInteger("State", 0);
                    }
                }
            }

            if (activateJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(transform.up * (groundedJumpForce));
                activateJump = false;
                currentJumps -= 1;
            }
        }


        //Aerial movement stuff
        if (!isTouchingGround)
        {
            if (rb.velocity.y > 0 && rb.velocity.y > -maxDownwardVelocity)
            {
                rb.AddForce(-transform.up * (upwardGravity * weight));
            }
            else if (rb.velocity.y <= 0 && rb.velocity.y > -maxDownwardVelocity)
            {
                rb.AddForce(-transform.up * (downwardGravity * weight));
            }


            if (moveRight) //movement going right
            {
                if (!isAttackingInAir && !isAttackingOnGround)
                {
                    anim.SetInteger("State", 1);
                    gameObject.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
                }

                if (thrustReset && horAxisPos > runTransitionAxis && rb.velocity.x < maxAirVelocity * .75f)
                {
                    rb.AddForce(transform.right * (aerialThrustForce / movementMultiplier));
                    thrustReset = false;
                }
                if (rb.velocity.x < maxAirVelocity * horAxisPos)   //movement takes into account how far the joystick is moved
                {
                    if (rb.velocity.x < -1)    //helps you quickly change direction
                    {
                        rb.AddForce(transform.right * (airSpeed * horAxisPos) * turnSpeed);
                    }
                    else { rb.AddForce(transform.right * (airSpeed * (horAxisPos / movementMultiplier))); } //if your not trying to change direction, movement is done here
                }
            }
            else if (moveLeft) //movement going left
            {
                if (!isAttackingInAir && !isAttackingOnGround)
                {
                    anim.SetInteger("State", 1);
                    gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
                }

                if (thrustReset && horAxisPos < -runTransitionAxis && rb.velocity.x > -maxAirVelocity * .75f)
                {
                    rb.AddForce(-transform.right * (aerialThrustForce / movementMultiplier));
                    thrustReset = false;
                }
                if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(maxAirVelocity * horAxisPos))//movement takes into account how far the joystick is moved -Ganderman Dan 🦆
                {
                    if (rb.velocity.x > 1)  //helps you quickly change direction
                    {
                        rb.AddForce(-transform.right * Mathf.Abs(airSpeed * horAxisPos) * turnSpeed);
                    }
                    else { rb.AddForce(-transform.right * Mathf.Abs(airSpeed * (horAxisPos / movementMultiplier))); } //if your not trying to change direction, movement is done here
                }
            }
            else
            {
                /*
                 * this determines your 'slideyness' when you put the joystick back into neutral position
                 * the higher the halt speed the faster you stop
                 * the lower the halt speed that most slippery you slide
                 * -GandermanDan 🦆
                 */

                if (rb.velocity.x > 1)
                {
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        rb.AddForce(-transform.right * (haltSpeed));
                    }
                    else
                    {
                        rb.AddForce(-transform.right * (attackHaltSpeed));
                    }
                }
                else if (rb.velocity.x < -1)
                {
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        rb.AddForce(transform.right * (haltSpeed));
                    }
                    else
                    {
                        rb.AddForce(transform.right * (attackHaltSpeed));
                    }
                }
                else
                {
                    if (!isAttackingInAir && !isAttackingOnGround)
                    {
                        anim.SetInteger("State", 0);
                    }
                }
            }





            if (activateJump)
            {
                /*
                float jumpPercent = (currentJumps / maxJumps) * 1.6f;
                if(jumpPercent < 1f)
                {
                    jumpPercent = 1f;
                }
                Debug.Log(" jumps percent: " + jumpPercent);
                */
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(transform.up * (aerialJumpForce));
                activateJump = false;
                currentJumps -= 1; 
            }
        }
        

    }

    /*ALL PLAYER INPUT goes here
     *anything that doesn't have to do with rigidbody movement, do here
     * 
     * Joystick numbers:
     * "joystick button 0" = ☐
     * "joystick button 1" = ✕
     * "joystick button 2" = ◯
     * "joystick button 3" = △
     */
    void Update()
    {
        
        if(currentHoldJumpTimer >= 0)
        {
            currentHoldJumpTimer -= Time.deltaTime;
        } 

        if (!isBot)
        {
            /*
            if(Input.GetKeyDown("joystick button 0"))
            {
                Debug.Log("はい");
            }
            */

            //set timer that will let the player jump slightly off the platform
            if (isTouchingGround && rb.velocity.y <= 0)
            {
                onGroundTimer = onGroundTimerMax;
            }
            else
            {
                onGroundTimer -= Time.deltaTime;
            }
            if (onGroundTimer <= 0)
            {
                isTouchingGround = false;
            }


            if (!isAttackingOnGround && !isAttackingInAir)
            {
                if (myPlayer.GetAxis("Horizontal") > .1f)//set right to true 
                {
                    moveRight = true;
                    horAxisPos = myPlayer.GetAxis("Horizontal");
                }
                else
                {
                    moveRight = false;
                }

                if (myPlayer.GetAxis("Horizontal") < -.1f)//set left to true 
                {
                    moveLeft = true;
                    horAxisPos = myPlayer.GetAxis("Horizontal");
                }
                else
                {
                    moveLeft = false;
                }

                if (myPlayer.GetAxis("Horizontal") > -.1f && myPlayer.GetAxis("Horizontal") < .1f)
                { //When joystick is set back to neutral ready up the next movement thrust
                    thrustReset = true;
                }

                //if you press the X button
                if (myPlayer.GetButtonDown("Jump"))
                {
                    if (currentJumps > 0)
                    {
                        activateJump = true;
                        currentHoldJumpTimer = maxHoldJumpTime;
                    }
                }

                if (myPlayer.GetButtonUp("Jump"))
                {
                   currentHoldJumpTimer = 0;
                }
            }

            //dont click on the link below 
            //https://www.youtube.com/watch?v=dQw4w9WgXcQ
            //-Ganderman Dan 🦆

            //if you press the Square button
            if (myPlayer.GetButtonDown("BasicAttack") && (!isAttackingInAir || !isAttackingOnGround))
            {
                if (isTouchingGround == true)
                {
                    //do grounded forward attack
                    moveLeft = false;
                    moveRight = false;
                    isAttackingOnGround = true;
                    anim.SetInteger("State", 2);
                }
                if (isTouchingGround == false)
                {
                    //do aerial forward attack

                }
            }
        }
    }

    //this gets called at the end of the attack animation to signal this script that you can move again because your attack is over -Ganderman Dan 🦆
    public void CanAttackAgain()
    {
        isAttackingOnGround = false;
        isAttackingInAir = false;

        if (isTouchingGround)
        {
            anim.SetInteger("State", 0);//idle
        }
        if (!isTouchingGround)
        {
            //****This is temporary, make this the falling while in air animation when you have one***************
            anim.SetInteger("State", 0);//idle falling anim
        }
    }


    /* This function gets called when an enemy hits you
     * What the arguments are for:
     *      attackDamage- is the how much the players health/armor goes down.
     *      attackAngle- is the angle you get sent flying when you get hit. [*possibly* affected by player weight]
     *      attackForce- is how far back you get sent flying. [affected by player weight]
     *      hitStun- is how long the player has to wait before they can do anything
     *      -Ganderman Dan 🦆
     */

    public void GetHit(float attackDamage, float attackAngle, float attackForce, float hitStun, bool facingRight)//im probably missing a few arguments
    {
        Vector3 dir = new Vector3(0,0,0);
        if (facingRight)
        {
            dir = Quaternion.AngleAxis(attackAngle, Vector3.forward) * Vector3.right;
        }
        else
        {
            dir = Quaternion.AngleAxis(attackAngle, -Vector3.forward) * -Vector3.right;
        }
        rb.AddForce(dir * attackForce);
       // rb.AddForce(new Vector2(attackForce, 0));
    }

    public bool FacingRight()
    {
        if(gameObject.transform.localScale.x > 0)
        {
            return true;
        }else
        {
            return false;
        }
    }

    /*
     * Both the OnCollisionEnter2D and OnCollisionStay2D methods are determining when the player is on the ground 
     * This is basically Corbetta's code from DGD-6 but I use it almost everytime I make a platformer because I feel like it's a good basis to start and I haven't thought of a better way to do it 
     * -Buscrubs
     */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); //stop vertical velocity
                if (contact.normal.y >= 0 && collision.gameObject.tag == "Floor")
                { //am I hitting the top of the platform?
                    isTouchingGround = true;
                    currentJumps = maxJumps;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                //velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0 && collision.gameObject.tag == "Floor")
                { //am I hitting the top of the platform?
                    isTouchingGround = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    currentJumps = maxJumps;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "GroundedForwardAttack")//****This will change
        {
            other.gameObject.GetComponentInParent<BasicAttack>().GroundedForwardAttack(gameObject);
        }

        // if (other.gameObject.tag == "Floor")
        // {
        //     isTouchingGround = true;
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Floor")
        //{
        //    isTouchingGround = false;
       // }
    }


    // everything that follows is for rewired, please do not change.
    //-Jon Mendez

    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(myPlayer);
    }

    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4
            switch (playerNum) {
                case 4:
                    ds4.SetLightColor(Color.yellow);
                    break;
                case 3:
                    ds4.SetLightColor(Color.green);
                    break;
                case 2:
                    ds4.SetLightColor(Color.blue);
                    break;
                case 1:
                    ds4.SetLightColor(Color.red);
                    break;
                default:
                    ds4.SetLightColor(Color.white);
                    Debug.LogError("Player Num is 0, please change to a number >0");
                    break;
            }
            

        }
    }
}
