using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class BasicPlayer : MonoBehaviour {

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    Rigidbody2D rb;

    private bool moving, startHitStun, inHitStun, preDashRight, preDashLeft, canDashRight, canDashLeft, dash;

    [Header("Movement Variables")]
    public float accel;
    public float accelMult;
    public float decelMult;
    public int speed;
    public float weight;
    //public int dashSpeed;

    public Vector3 velocity;
    public string direction;

    public int dashCount;
    public int dashCountMax;
    public bool canDash;
    [Space(10)]

    [Header("Gravity Variables")]
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float maxDownVel;
    public float onPlatformTimer;
    public float onPlatformTimerMax;
    public bool onTopOfPlatform;
    [Space(10)]

    int health;
    SpriteRenderer sr;
    public Animator anim;
    AudioSource playerJump;

    public enum PlayerState { Fighter, Support }
    [Header("Player State")]
    public PlayerState state;

    [Header("Which Character is This?")]
    public bool claire;
    public bool gillbert;
    public bool gnomercy;
    string characterName;
    Claire claireCharacter;
    Gillbert gillbertCharacter;
    Gnomercy gnomercyCharacter;

    [Header("Character Variables")]
    public float maxHealth;
    public float characterSpeed;
    public float characterWeight;

    void Awake()
    {

        if (claire)
        {
            characterName = "claire";
        }
        if (gillbert)
        {
            characterName = "gillbert";
        }
        if (gnomercy)
        {
            characterName = "gnomercy";
        }

        //Rewired Code
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);
    }

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerJump = GetComponent<AudioSource>();
        health = 5;

        canDash = true;

        direction = "Right";
    }

    
	
	// Update is called once per frame
	void Update () {

        if (!inHitStun)
        {

            Movement();

        }

        if (!dash && !onTopOfPlatform)
        {
            Gravity();
        }

        //float mySpeed = Mathf.Abs(rb.velocity.x);
        //anim.SetFloat("xSpeed", Mathf.Abs(velocity.x));
        //anim.SetFloat("yVel", rb.velocity.y);
    }

    void Movement()
    {
        //seing which way the player is moving
        if(myPlayer.GetAxisRaw("Horizontal") > 0)
        {
            direction = "Right";
        }
        else if(myPlayer.GetAxisRaw("Horizontal") < 0)
        {
            direction = "Left";
        }

        if (Mathf.Abs(myPlayer.GetAxis("Horizontal")) >= .01)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            if (direction == "Right")
            {
                if (accel < myPlayer.GetAxis("Horizontal"))
                {
                    accel += accelMult;
                }
                else
                {
                    accel = myPlayer.GetAxis("Horizontal");
                }
            }
            if(direction == "Left")
            {
                if(accel > myPlayer.GetAxis("Horizontal"))
                {
                    accel -= accelMult;
                }
                else
                {
                    accel = myPlayer.GetAxis("Horizontal");
                }
            }
            anim.SetInteger("State", 1);
        }
        else
        {
            if(direction == "Right")
            {
                if(accel > 0)
                {
                    accel -= decelMult;
                }
                else
                {
                    accel = 0;
                }
            }
            if(direction == "Left")
            {
                if(accel < 0)
                {
                    accel += decelMult;
                }
                else
                {
                    accel = 0;
                }
            }
            anim.SetInteger("State", 0);
        }

        if (onTopOfPlatform)
        {
            if (direction == "Right")
            {
                sr.flipX = false;
            }
            if (direction == "Left")
            {
                sr.flipX = true;
            }
        }
        /*
        if (velocity.x >= .01)
        {
            direction = "Right";
            sr.flipX = false;
        }
        if (velocity.x <= -.01)
        {
            direction = "Left";
            sr.flipX = true;
        }
        */

        //horizontal movement
        if (!dash)
        {
            velocity.x = accel * speed;
        }
        /*else
        {
            if(velocity.y > 0)
            {
                velocity = velocity.normalized * dashSpeed;
            }
            else if (direction == "Right")
            {
                velocity = new Vector2(1 * dashSpeed, 0);
            }
            else if(direction == "Left")
            {
                velocity = new Vector2(-1 * dashSpeed, 0);
            }
            
        }
        */

        //dash mechanics
        /*
        if(Input.GetButtonDown("Fire1") && dashCount > 0 && canDash)
        {
            StartCoroutine(Dash());
        }
        */
        /*
        if (velocity.x >= 1 && Input.GetButtonDown("Fire1") && dashCount > 0)
        {
            StartCoroutine(Dash());
        }
        if (velocity.x <= -1 && Input.GetButtonDown("Fire1") && dashCount > 0)
        {
            StartCoroutine(Dash());
        }
        */

        //set timer that will let the player jump slightly off the platform
        if (onTopOfPlatform && velocity.y == 0)
        {
            onPlatformTimer = onPlatformTimerMax;
        }
        else
        {
            onPlatformTimer -= Time.deltaTime;
        }

        //jump logic
        if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0)
        {
            velocity.y = jumpVel;
            //playerJump.Play();
            //anim.SetTrigger("jumpStart");
        }
    }

    private void FixedUpdate()
    {

        rb.MovePosition(transform.position + velocity * Time.deltaTime);

    }

    private void LateUpdate()
    {

        onTopOfPlatform = false;

    }

    void Gravity()
    {
        //gravity logic
        if (velocity.y > -maxDownVel)
        { //if we haven't reached maxDownVel
            if (velocity.y > 0)
            { //if player is moving up
                velocity.y -= gravityUp * weight * Time.deltaTime;
            }
            else
            { //if player is moving down
                velocity.y -= gravityDown * weight * Time.deltaTime;
            }
        }
    }

    
    //Ignore this code
    IEnumerator Dash()
    {
        canDash = false;
        dashCount--;
        dash = true;
        yield return new WaitForSeconds(.2f);
        dash = false;
        canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?
                    /*
                    if (collisionInfo.gameObject.tag == "Boss")
                    {


                        Debug.Log("Hit");
                    }
                    else
                    */
                    //{
                        onTopOfPlatform = true;
                        dashCount = dashCountMax;
                    //}
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                if (velocity.y < 0)
                {
                    velocity.y = 0; //stop vertical velocity
                }
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?
                    onTopOfPlatform = true;
                    dashCount = dashCountMax;
                }
            }
        }
    }

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
            switch (playerNum)
            {
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
