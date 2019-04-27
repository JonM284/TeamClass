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

    private float xScale;

    //separating when the player can move verses when they are in knockback
    enum playerState { FreeMovement, Knockback}
    playerState player;

    [HideInInspector]
    public Rigidbody2D rb;

    [Space(10)]

    [Header("Gravity Variables")]
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float maxDownVel;
    public float maxInitialJumpTime;
    private float initialJumpTime = 0;
    public float maxHoldJumpTime;
    private float holdJumpTime = 0;
    private bool jumpButtonPressed = false;
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
        
        //making the player face a certain way
        if (makeFaceRight)
        {
            xScale = -gameObject.transform.localScale.x;
        }
        else
        {
            xScale = gameObject.transform.localScale.x;
        }

        player = playerState.FreeMovement;

    }

    // Update is called once per frame
    void Update()
    {

        Movement();

    }

    private void FixedUpdate()
    {

        Gravity();

        //always running this so that everything can be based off of gravity
        rb.MovePosition(transform.position + velocity * speed * Time.fixedDeltaTime);

    }

    void Movement()
    {
        if(player == playerState.FreeMovement)
        {

            velocity.x = myPlayer.GetAxis("Horizontal");

            if(velocity.x > .01)
            {
                direction = "Right";
            }
            else
            {
                direction = "Left";
            }

            if (onPlatformTimer > 0)
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

            //jump logic
            if (initialJumpTime <= 0)
            {
                holdJumpTime -= Time.deltaTime;
            }

            if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0 && !isAttacking)
            {
                isJumping = true;
                initialJumpTime = maxInitialJumpTime;
                holdJumpTime = maxHoldJumpTime;
                jumpButtonPressed = true;
                StartCoroutine(DoJumpAnim());
            }
            if (myPlayer.GetButtonUp("Jump"))
            {
                jumpButtonPressed = false;
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

        }
    }

    void FixedMovement()
    {
        //initial jump
        if (initialJumpTime > 0 && !hitHead)
        {
            velocity.y = jumpVel;
        }
        /*else if (initialJumpTime > 0)
        {
            velocity.y = 0;
        }
        */

        //hold jump
        if (initialJumpTime <= 0 && jumpButtonPressed && holdJumpTime > 0 && !hitHead)
        {
            velocity.y = jumpVel;
        }
        /*else if (initialJumpTime <= 0 && jumpButtonPressed && holdJumpTime > 0 && hitHead)
        {
            velocity.y = 0;
        }
        */
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
