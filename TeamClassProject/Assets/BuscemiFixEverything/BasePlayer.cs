using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class BasePlayer : MonoBehaviour
{

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;
    [Space(10)]

    [Header("Movement Variables")]
    [HideInInspector]
    public int speed;
    [HideInInspector]
    public float weight;
    [Tooltip("How long Gillbert can fly for at max")]
    public float gilbertFlightTime;
    [HideInInspector]
    public float currentGilbertFlightTime;

    public Vector3 velocity;
    public string direction;

    public float xScale;

    //separating when the player can move verses when they are in knockback
    enum playerState { FreeMovement, Knockback}
    playerState player;


    Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;

    [Space(10)]

    [Header("Gravity Variables")]
    public float gravityUp;
    public float gravityDown;
    public float maxDownVel;
    public float jumpVel;
    public float fallMult;
    public float lowJumpMult;
    public float onPlatformTimer;
    public float onPlatformTimerMax;
    public bool onTopOfPlatform;
    [Space(10)]

    [Header("Which Character is This?")]
    public bool claire;
    public bool gillbert;
    public bool gnomercy;
    public bool wawa;
    Claire claireCharacter;
    Gillbert gillbertCharacter;
    Gnomercy gnomercyCharacter;
    Watermelon wawaCharacter;
    [Space(10)]

    [Header("Character Variables")]
    public float maxHealth;
    public float currentHealth;
    public float regenHeath;
    public float regenHeathMultiplier;
    public bool makeFaceRight;
    private bool isFlying = false;
    [Space(10)]

    [Header("Attack Variables")]
    public bool isAttacking;
    public Vector2 joyPos;

    [Header("Joystick Deadzone")]
    public float horizontalDZ;

    private void Awake()
    {

        if (claire)
        {
            claireCharacter = this.GetComponent<Claire>();
            maxHealth = claireCharacter.maxHealth;
            currentHealth = maxHealth;
            regenHeath = maxHealth;
            speed = claireCharacter.speed;
            weight = claireCharacter.weight;
            gravityUp = claireCharacter.gravityUp;
            gravityDown = claireCharacter.gravityDown;
            jumpVel = claireCharacter.jumpVel;
            maxDownVel = claireCharacter.maxDownVel;
        }
        if (gillbert)
        {
            gillbertCharacter = this.GetComponent<Gillbert>();
            maxHealth = gillbertCharacter.maxHealth;
            currentHealth = maxHealth;
            regenHeath = maxHealth;
            speed = gillbertCharacter.speed;
            weight = gillbertCharacter.weight;
            gravityUp = gillbertCharacter.gravityUp;
            gravityDown = gillbertCharacter.gravityDown;
            jumpVel = gillbertCharacter.jumpVel;
            maxDownVel = gillbertCharacter.maxDownVel;
            currentGilbertFlightTime = gilbertFlightTime;

        }
        if (gnomercy)
        {
            gnomercyCharacter = this.GetComponent<Gnomercy>();
            maxHealth = gnomercyCharacter.maxHealth;
            currentHealth = maxHealth;
            regenHeath = maxHealth;
            speed = gnomercyCharacter.speed;
            weight = gnomercyCharacter.weight;
            gravityUp = gnomercyCharacter.gravityUp;
            gravityDown = gnomercyCharacter.gravityDown;
            jumpVel = gnomercyCharacter.jumpVel;
            maxDownVel = gnomercyCharacter.maxDownVel;
        }
        if (wawa)
        {
            wawaCharacter = this.GetComponent<Watermelon>();
            maxHealth = wawaCharacter.maxHealth;
            currentHealth = maxHealth;
            speed = wawaCharacter.speed;
            weight = wawaCharacter.weight;
            gravityUp = wawaCharacter.gravityUp;
            gravityDown = wawaCharacter.gravityDown;
            jumpVel = wawaCharacter.jumpVel;
            maxDownVel = wawaCharacter.maxDownVel;
        }

        //Rewired Code
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);

    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        //making the player face a certain way
        if (makeFaceRight)
        {
            xScale = gameObject.transform.localScale.x;
        }
        else
        {
            xScale = -gameObject.transform.localScale.x;
        }

        player = playerState.FreeMovement;

    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        //if (!isAttacking)
        {

            Attack();

        }

    }

    private void FixedUpdate()
    {

        FixedMovement();

        Gravity();

        //always running this so that everything can be based off of gravity
        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

    }

    void Movement()
    {
        if(player == playerState.FreeMovement)
        {

            if (Mathf.Abs(myPlayer.GetAxis("Horizontal")) > horizontalDZ)
            {
                velocity.x = myPlayer.GetAxis("Horizontal") * speed;
                anim.SetFloat("xVel", 1);
            }
            else
            {
                velocity.x = 0;
                anim.SetFloat("xVel", 0);
            }

            anim.SetFloat("yVel", velocity.y);

            if(velocity.x > 0)
            {
                direction = "Right";
            }
            else if(velocity.x < 0)
            {
                direction = "Left";
            }

            if (onPlatformTimer > -.15)
            {
                if (direction == "Right")
                {
                    gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
                }
                if (direction == "Left")
                {
                    gameObject.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
                }
            }

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
                anim.ResetTrigger("Jump");
                anim.SetTrigger("Jump");
            }

            if (velocity.y < 0)
            {
                velocity += Vector3.up * -gravityDown * (fallMult - 1) * Time.deltaTime;
            }
            else if (velocity.y > 0 && !myPlayer.GetButton("Jump"))
            {
                velocity += Vector3.up * -gravityUp * (lowJumpMult - 1) * Time.deltaTime;
            }

        }
    }

    void FixedMovement()
    {

        

    }

    void Attack()
    {

        if (player == playerState.FreeMovement)
        {

            //checking to see where the joystick is
            joyPos = new Vector2(myPlayer.GetAxis("Horizontal"), myPlayer.GetAxis("Vertical"));

            if (myPlayer.GetButtonDown("BasicAttack"))
            {

                if(Mathf.Abs(joyPos.x) < horizontalDZ && Mathf.Abs(joyPos.y) < horizontalDZ)
                {
                    anim.SetFloat("Attack", 0);
                }

                anim.ResetTrigger("Basic");
                anim.SetTrigger("Basic");
                isAttacking = true;

            }
        }

    }

    void Gravity()
    {
        //gravity logic
        if (velocity.y > -maxDownVel)
        { //if we haven't reached maxDownVel
            if (velocity.y > 0)
            { //if player is moving up
                velocity.y -= gravityUp * Time.fixedDeltaTime;
            }
            else
            { //if player is moving down
                velocity.y -= gravityDown * Time.fixedDeltaTime;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                if (collisionInfo.gameObject.tag != "Player")
                {
                    velocity.y = 0; //stop vertical velocity
                    if (contact.normal.y >= 0)
                    { //am I hitting the top of the platform?
                        onTopOfPlatform = true;
                        anim.ResetTrigger("Land");
                        anim.SetTrigger("Land");
                        //anim.ResetTrigger("Land");
                        velocity.y = 0;
                    }
                    //am I hitting the bottom of a platform?
                    if (contact.normal.y < 0)
                    {

                        velocity.y = 0;

                    }
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
                    //velocity.y = 0; //stop vertical velocity
                }
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?
                    if (velocity.y < 0)
                    {

                        velocity.y = 0; //stop vertical velocity

                    }
                    onTopOfPlatform = true;
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
