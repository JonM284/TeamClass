﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.ControllerExtensions;

public class BasicPlayerScript : MonoBehaviour
{

	//the following is in order to use rewired
	[Tooltip("Reference for using rewired")]
	private Player myPlayer;
	[Header("Rewired")]
	[Tooltip("Number identifier for each player, must be above 0")]
	public int playerNum;
    public SpriteRenderer[] rigPieces;
    public int teamNum;
	private Shader outlineColor;

	public Image healthBar;
	public Image regenableHealthBar;

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
	public Animator anim;

	private bool moving, hitHead;

	public enum PlayerState { Fighter, Support }
	[Header("Am I Fighting or Support")]
	public PlayerState state;

	[Header("Movement Variables")]
	public float accel;
	public float accelMult;
	public float decelMult;
	[HideInInspector]
	public int speed;
	[HideInInspector]
	public float weight;
    public float gilbertFlightTime;
    [HideInInspector]
    public float currentGilbertFlightTime;

    //checking to see if the player is in a hit or recovery animation;
    bool inHitAnims;

	public Vector3 velocity;
	Vector3 previousPos, currentPos;
	Vector3 currentVelocity;
	public string direction;
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

	[Header("Which Character is This?")]
	public bool claire;
	public bool gillbert;
	public bool gnomercy;
    public bool wawa;
	Claire claireCharacter;
	Gillbert gillbertCharacter;
	Gnomercy gnomercyCharacter;
    Watermelon wawaCharacter;

    [Header("Character Variables")]
	public float maxHealth;
	public float currentHealth;
	public float regenHeath;
	public float regenHeathMultiplier;
	public bool makeFaceRight;
    private bool isFlying = false;

    float landtimer = 0;

	private float xScale;
	[HideInInspector]
	public bool isAttacking;
	bool isJumping;

	//this is to see if isAttacking is true when it shouldn't be
	float checkAttackTimer;

	//make the player stop in their tracks
	bool constrainPosition;

	private Vector3 hitDirection;
	private float gotHitTimer = 0;
	private float knockback = 0;

	private float maxKnockbackTime;
	private float currentKnockbackTime;
	private float maxDistance;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float hitAngle;
    public float stunTime = 0;
    private float maxUpVel = 4;

    [Header("Dash")]
    public float dashSpeed;
    public float maxDashTimer;
    private float currentDashTimer = 0;
    private float prevJoystickAxis = 0;
    private bool didntTurnAround = true;

    private float doAttackMovementTimer = 0;
    private Vector3 attackMovementDir = new Vector3(0, 0, 0);
    private float attackMovementSpeed = 0;
    private float attackTimerDelay = 0;


    bool findTeamController = false;

    GameObject mainCamera;


    [Header("Attack Zone Values")]
    public float attackZone;
    public float neutralZone;
    public float verticalForwardZone;
    public float horizontalUpwardZone;

    [Header("Visual Effects")]
    public GameObject hitEffect;

    [HideInInspector]
    public GameObject teamController;

    [HideInInspector]
    public bool canTurn = true;

    Animator healthAnim;

    private pauserScript m_my_Pauser;

    [Header("Vibration Variables")]
    [Tooltip("The magnitude of the vibration for the controller - light")]
    [Range(0, 1.0f)]
    public float light_Vib = 0.3f;
    [Tooltip("The magnitude of the vibration for the controller - heavy")]
    [Range(0, 1.0f)]
    public float Heavy_Vib = 0.5f;
    [Tooltip("The amount of time the virbtation will last in seconds")]
    [Range(0, 1.5f)]
    public float light_Time = 0.2f, Heavy_Time = 0.35f;

    void Awake()
	{
        


        mainCamera = GameObject.Find("Main Camera");

		//grabbing character specific values from their respective characters

		if (claire)
		{
			claireCharacter = this.GetComponent<Claire>();
			maxHealth = claireCharacter.maxHealth;
			currentHealth = maxHealth;
            regenHeath = maxHealth;
			speed = claireCharacter.speed;
			weight = claireCharacter.weight;
			//gravityUp = claireCharacter.gravityUp;
			//gravityDown = claireCharacter.gravityDown;
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

        rigPieces = GetComponentsInChildren<SpriteRenderer>();
        if (playerNum == 1)
        {
            foreach (SpriteRenderer sprite in rigPieces)
            {
                if (sprite != null)
                {
                    sprite.sortingLayerName = "Player 1";
                }
            }
        }
        else
        {
            if (playerNum == 2)
            {
                foreach (SpriteRenderer sprite in rigPieces)
                {
                    if (sprite != null)
                    {
                        sprite.sortingLayerName = "Player 2";
                    }
                }
            }
        }

		outlineColor = Shader.Find("GUI/Text");

		if (teamNum == 1)
		{
			foreach (SpriteRenderer sprite in rigPieces)
			{
				if (sprite.sortingLayerName == "Outline")
				{
					sprite.material.shader = outlineColor;
					sprite.color = Color.red;
				}
				
			}
		}

		if (teamNum == 2)
		{
			foreach (SpriteRenderer sprite in rigPieces)
			{
				if (sprite.sortingLayerName == "Outline")
				{
					sprite.material.shader = outlineColor;
					sprite.color = Color.blue;
				}

			}
		}

	}

	// Start is called before the first frame update
	void Start()
    {

        m_my_Pauser = pauserScript.pauser_Instance;
        rb = GetComponent<Rigidbody2D>();
		if (GetComponent<Animator>() != null)
		{
			anim = GetComponent<Animator>();
		}
		else
		{
			anim = null;
		}

		//making the player face a certain way
		if (makeFaceRight)
		{
			xScale = -gameObject.transform.localScale.x;
		}
		else
		{
			xScale = gameObject.transform.localScale.x;
		}

		//this isn't working properly since it makes the player go back down to 0 in between frames
		StartCoroutine(CalcVelocity());

        healthAnim = healthBar.GetComponent<Animator>();

	}

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Horizontal: " + myPlayer.GetAxis("Horizontal"));
        Debug.Log("Vertical: " + myPlayer.GetAxis("Vertical"));
        attackTimerDelay -= Time.deltaTime;
        landtimer -= Time.deltaTime;

        if (findTeamController == false)
        {
            if (teamNum == 1)
            {
                teamController = GameObject.Find("Team1");
            }
            else if (teamNum == 2)
            {
                teamController = GameObject.Find("Team2");
            }

            findTeamController = true;
        }

        if (myPlayer.GetButtonDown("Switch"))
        {
            Debug.Log(teamController.name);
            try {
                    teamController.GetComponent<SwitchHandler>().BeginSwap(playerNum);
            }catch {
            }
        }

        if (myPlayer.GetButtonDown("Pause"))
        {
            m_my_Pauser.Pauser();
        }

        //<3 for Justin  
        //-Love Dan 🦆
        //Here's additional recognition for our one true leader throughout this endeavorous task
        //set upon us, for without him we are nothing.
        //Praise be to our one and only team manager Patrick ♥♥♥♥♥♥♥♥

        //I hate you Pat. You are not only a regular garbage person, but you are THE garbage man, who eats trash but isn't Danny Devito.

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (onTopOfPlatform && gillbert && currentGilbertFlightTime < gilbertFlightTime)
        {
            if(currentGilbertFlightTime < 0)
            {
                currentGilbertFlightTime = 0;
            }
            currentGilbertFlightTime += Time.deltaTime * 2;
        }


		//Debug.Log(isAttacking);
        stunTime -= Time.deltaTime;

		if (healthBar != null && regenableHealthBar != null)
		{
			healthBar.fillAmount = currentHealth / maxHealth;
			regenableHealthBar.fillAmount = regenHeath / maxHealth;
		}

		if (!isAttacking && !isJumping && stunTime <= 0)
		{
			Attack();
		}

        if (doAttackMovementTimer > 0 && attackTimerDelay < 0)
        {
            doAttackMovementTimer -= Time.deltaTime;
        }

        if (stunTime <= 0 && !inHitAnims && doAttackMovementTimer <= 0)
        {
            Movement();
        }
		//Reset the velocity whenever the player attacks
		/*
        else
		{
			accel = 0;
			velocity = new Vector3(0, 0, 0);
		}
		*/
	}

 void FixedUpdate()
	{
        if(doAttackMovementTimer > 0 && attackTimerDelay < 0)
        {
            doAttackMovement();
        }

        if(gotHitTimer > 0)
        {
            anim.SetBool("hitstun", true);
        }
        else
        {
            anim.SetBool("hitstun", false);
        }

		if (stunTime <= 0 && !inHitAnims && doAttackMovementTimer <= 0)
		{
            FixedMovement();
        }
   

        if (!onTopOfPlatform && state == PlayerState.Fighter)
		{
			Gravity();
		}

        //knockback stuff
        if (currentKnockbackTime / maxKnockbackTime < .98f)
        {
            Knockback();
            //velocity = (hitDirection * knockback);
        }

    }

	private void LateUpdate()
	{

		onTopOfPlatform = false;
        prevJoystickAxis = myPlayer.GetAxis("Horizontal");
    }

	/// <summary>
	/// This is going to control everything that the player would need to move around the game
	/// If the player needs something that doesn't require any physics, make sure it goes here
	/// </summary>
	void Movement()
	{

		//animation logic for fighter and support
		anim.SetFloat("xVel", Mathf.Abs(velocity.x));
		anim.SetFloat("yVel", velocity.y);
        anim.SetBool("isFlying", isFlying);
        anim.SetFloat("PlatformTimer", onPlatformTimer);

        //animation logic for just the fighter
        anim.SetBool("isAttacking", isAttacking);

		gotHitTimer -= Time.deltaTime;

        //seing which way the player is moving
        if (!isAttacking)
        {
            if (myPlayer.GetAxisRaw("Horizontal") > 0)
            {
                direction = "Right";
            }
            else if (myPlayer.GetAxisRaw("Horizontal") < 0)
            {
                direction = "Left";
            }
        }

		if (Mathf.Abs(myPlayer.GetAxis("Horizontal")) >= .12 || Mathf.Abs(myPlayer.GetAxis("Horizontal")) <= -.12)
		{
			moving = true;
		}
		else
		{
			moving = false;
		}

		//Aceleration code
		if (moving)
		{
            if(((prevJoystickAxis < .7f && prevJoystickAxis >.6f) || (prevJoystickAxis > -.7f && prevJoystickAxis <-.6f)) && currentDashTimer < 0 && velocity.x < .5f && velocity.x > -.5f && onTopOfPlatform)
            {
                currentDashTimer = maxDashTimer;
                didntTurnAround = true;
            }

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
			if (direction == "Left")
			{
				if (accel > myPlayer.GetAxis("Horizontal"))
				{
					accel -= accelMult;
				}
				else
				{
					accel = myPlayer.GetAxis("Horizontal");
				}
			}
		}
		//Deceleration code
		else
		{
			if (direction == "Right")
			{
				if (accel > 0)
				{
                    if (onTopOfPlatform) { accel -= decelMult; }
                   // if (onTopOfPlatform) { accel -= decelMult / 4; } //discard probably :D
                }
				else
				{
					accel = 0;
				}
			}
			if (direction == "Left")
			{
				if (accel < 0)
				{
                    if (onTopOfPlatform) { accel += decelMult; }
                    //if (onTopOfPlatform) { accel += decelMult / 4; } //discard probably :D
                }
				else
				{
					accel = 0;
				}
			}
		}

		//this is movement that the player needs only when it is a fighter
		if (state == PlayerState.Fighter)
		{
			if (onPlatformTimer > 0 && !isAttacking && !inHitAnims)
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

			if (gillbert)
			{
				if (myPlayer.GetButton("Jump") && holdJumpTime <= 0 && !onTopOfPlatform && currentGilbertFlightTime > 1f)
				{
                    currentGilbertFlightTime -= Time.deltaTime;
                    if (velocity.y < maxUpVel)
                    {
                        velocity.y += 1.5f;
                    }

                    isFlying = true;
                }
                else
                {
                    isFlying = false;
                }
			}

		}

	}

	void FixedMovement()
	{

		velocity.x = accel * speed;

		//this is movement that the player needs only when it is a fighter
		if (state == PlayerState.Fighter)
		{
			//stopping the jump velocity if you hit your head
			if (hitHead && velocity.y > 0)
			{
				velocity.y = 0;
			}

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

			initialJumpTime -= Time.fixedDeltaTime;
		}



        if (currentDashTimer > 0)
        {
            if (prevJoystickAxis >= .5f && myPlayer.GetAxis("Horizontal") < 0)
            {
                didntTurnAround = false;
            }else
            if (prevJoystickAxis <= -.5f && myPlayer.GetAxis("Horizontal") > 0)
            {
                didntTurnAround = false;
            }
            
            if(didntTurnAround)
            {
                float dashMultiplier = ((currentDashTimer / maxDashTimer) * 100) / 3;
                velocity.x *= dashSpeed * dashMultiplier;
            }

        }

        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

	}

	void Attack()
	{
		//neutral basic attack
		if (gotHitTimer < 0)
		{
            
			if (myPlayer.GetAxis("Horizontal") < neutralZone && myPlayer.GetAxis("Horizontal") > -neutralZone && myPlayer.GetAxis("Vertical") < neutralZone && myPlayer.GetAxis("Vertical") > -neutralZone && onPlatformTimer > 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    //if (claire) { claireCharacter.ClaireAttackController(1); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(2); }
				}
			}
            
            //forward basic attack
            if (((myPlayer.GetAxis("Horizontal") > attackZone && myPlayer.GetAxis("Horizontal") > -attackZone) || (myPlayer.GetAxis("Horizontal")) < attackZone && myPlayer.GetAxis("Horizontal") < -attackZone) && Input.GetAxis("Vertical") <= verticalForwardZone && Input.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(2); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(2); }

                    if (wawa) { wawaCharacter.WawaAttackController(2); }
                }
            }

            //up basic attack
            if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") > attackZone && myPlayer.GetAxis("Vertical") > -attackZone && onPlatformTimer > 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    //if (claire) { claireCharacter.ClaireAttackController(3); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(3); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(3); }
				}
			}
                       
            //Down basic attack
            if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") < attackZone && myPlayer.GetAxis("Vertical") < -attackZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(4); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(4); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(4); }

					if (wawa) {wawaCharacter.WawaAttackController(4); }
                }
            }

            //neutral air attack
            if (myPlayer.GetAxis("Horizontal") < neutralZone && myPlayer.GetAxis("Horizontal") > -neutralZone && myPlayer.GetAxis("Vertical") < neutralZone && myPlayer.GetAxis("Vertical") > -neutralZone && onPlatformTimer < 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
				{
                    //if (claire) { claireCharacter.ClaireAttackController(9); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(9); }

                    if (wawa) { wawaCharacter.WawaAttackController(9); }

                }
			}

			//up air attack
			if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") > attackZone && myPlayer.GetAxis("Vertical") > -attackZone && onPlatformTimer < 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
				{
                    //if (claire) { claireCharacter.ClaireAttackController(10); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(9); }
                }
			}

            //forward air attack
            if (direction == "Right")
            {
                if (myPlayer.GetAxis("Horizontal") > attackZone && myPlayer.GetAxis("Horizontal") > -attackZone && myPlayer.GetAxis("Vertical") <= verticalForwardZone && myPlayer.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer < 0)
                {
                    if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                    {
                        //if (claire) { claireCharacter.ClaireAttackController(9); }

                        if (gillbert) { gillbertCharacter.GilbertAttackController(11); }

                        if (gnomercy) { gnomercyCharacter.GnomercyAttackController(14); }

                        if (wawa) { wawaCharacter.WawaAttackController(14); }
                    }
                }
            }
            if (direction == "Left")
            {
                if (myPlayer.GetAxis("Horizontal") < attackZone && myPlayer.GetAxis("Horizontal") < -attackZone && myPlayer.GetAxis("Vertical") <= verticalForwardZone && myPlayer.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer < 0)
                {
                    if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                    {
                        //if (claire) { claireCharacter.ClaireAttackController(9); }

                        if (gillbert) { gillbertCharacter.GilbertAttackController(11); }

                        //if (gnomercy) { gnomercyCharacter.GnomercyAttackController(11); }

                        if (wawa) { wawaCharacter.WawaAttackController(14); }
                    }
                }
            }

            //back air attack
            if (direction == "Right")
            {
                if (myPlayer.GetAxis("Horizontal") < attackZone && myPlayer.GetAxis("Horizontal") < -attackZone && myPlayer.GetAxis("Vertical") <= verticalForwardZone && myPlayer.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer < 0)
                {
                    if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                    {
                        
                        //if (claire) { claireCharacter.ClaireAttackController(9); }

                        if (gillbert) { gillbertCharacter.GilbertAttackController(9); }

                        //if (gnomercy) { gnomercyCharacter.GnomercyAttackController(11); }

                        if (wawa) { wawaCharacter.WawaAttackController(10); }
                    }
                }
            }
            if (direction == "Left")
            {
                if (myPlayer.GetAxis("Horizontal") > attackZone && myPlayer.GetAxis("Horizontal") > -attackZone && myPlayer.GetAxis("Vertical") <= verticalForwardZone && myPlayer.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer < 0)
                {
                    if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                    {
                        
                        //if (claire) { claireCharacter.ClaireAttackController(9); }

                        if (gillbert) { gillbertCharacter.GilbertAttackController(9); }

                        //if (gnomercy) { gnomercyCharacter.GnomercyAttackController(11); }

                        if (wawa) { wawaCharacter.WawaAttackController(10); }
                    }
                }
            }


            //down air attack
            if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") < attackZone && myPlayer.GetAxis("Vertical") < -attackZone && onPlatformTimer < 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                {

					//if (claire) { claireCharacter.ClaireAttackController(12); }

					if (wawa) { wawaCharacter.WawaAttackController(11); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(12); }


                }
            }

            //Neutral Heavy
            if (myPlayer.GetAxis("Horizontal") < neutralZone && myPlayer.GetAxis("Horizontal") > -neutralZone && myPlayer.GetAxis("Vertical") < neutralZone && myPlayer.GetAxis("Vertical") > -neutralZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(20); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(20); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(20); }

					if (wawa) { wawaCharacter.WawaAttackController(20); }
                }
            }

            //forward heavy attack
            if (((myPlayer.GetAxis("Horizontal") > attackZone && myPlayer.GetAxis("Horizontal") > -attackZone) || (myPlayer.GetAxis("Horizontal")) < attackZone && myPlayer.GetAxis("Horizontal") < -attackZone) && myPlayer.GetAxis("Vertical") <= verticalForwardZone && myPlayer.GetAxis("Vertical") >= -verticalForwardZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(21); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(21); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(21); }
				}
            }

            //Down Heavy
            if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") < attackZone && myPlayer.GetAxis("Vertical") < -attackZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(22); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(22); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(22); }
				}
            }

            //Up Heavy
            if (myPlayer.GetAxis("Horizontal") < horizontalUpwardZone && myPlayer.GetAxis("Horizontal") > -horizontalUpwardZone && myPlayer.GetAxis("Vertical") > attackZone && myPlayer.GetAxis("Vertical") > -attackZone && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    //if (claire) { claireCharacter.ClaireAttackController(23); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(20); }

                    if (gnomercy) { gnomercyCharacter.GnomercyAttackController(23); }
                }
            }
            


            //ult
            // if (myPlayer.GetButtonDown("Ult"))
            if (myPlayer.GetButtonDown("Ultimate")) 
            {
                if (teamController.GetComponent<SwitchHandler>().specialMeter[2].fillAmount >= .2)
                {
                    teamController.GetComponent<SwitchHandler>().currentUltNum = 0;

                    //if (claire) { claireCharacter.ClaireAttackController(69); }

                    //if (gillbert) { gillbertCharacter.GilbertAttackController(69); }

                    teamController.GetComponent<SwitchHandler>().UpdateUltBar(0);
                }
            }
        }
	}

    public void ResetTriggers()
    {
        anim.ResetTrigger("BasicNeutral");
        anim.ResetTrigger("BasicForward");
        anim.ResetTrigger("BasicUp");
        anim.ResetTrigger("BasicDown");
        anim.ResetTrigger("HeavyNeutral");
        anim.ResetTrigger("HeavyForward");
        anim.ResetTrigger("HeavyUp");
        anim.ResetTrigger("HeavyDown");
        anim.ResetTrigger("land");
        anim.ResetTrigger("Jump");
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

    public void Knockback()
    {
        currentKnockbackTime += Time.deltaTime;

        //exponential knockback movement
        float progress = currentKnockbackTime / maxKnockbackTime;
        progress = Mathf.Sin(progress * Mathf.PI * 0.5f);
        // velocity = startPosition - Vector3.Lerp(startPosition, endPosition, progress * Time.deltaTime);
        rb.MovePosition(Vector3.Lerp(startPosition, endPosition, progress * Time.deltaTime));
    }

    /// <summary>
    /// This function gets called when an enemy hits you
    /// </summary>
    /// <param name="attackDamage">is the how much the players health/armor goes down.</param>
    /// <param name="attackAngle">is the angle you get sent flying when you get hit. [*possibly* affected by player weight]</param>
    /// <param name="attackForce"> is how far back you get sent flying. [affected by player weight]</param>
    /// <param name="hitStun">is how long the player has to wait before they can do anything</param>
    /// <param name="distance">How far does the enemy fly</param>
    /// <param name="travelTime">How long should it take for the enemy to get to that distance</param>
    /// <param name="facingRight">Checks which way the player is facing when they do the attack so that it knows whether or not to reverse the knockback</param>
    /// <param name="duration">How long the screen shake lasts</param>
    /// <param name="magnitude">How agressively the screen shakes</param>
    /// <param name="slowDown">How quickly the camera stops shaking</param>
    public void GetHit(float attackDamage, float attackAngle, float attackForce, float hitStun, float distance, float travelTime, bool facingRight, float duration, float magnitude, float slowDown)//im probably missing a few arguments
    {
        if (claire && claireCharacter.shield)
        {
            //do nothing
        }
        else
        {
            healthAnim.ResetTrigger("gotHit");
            healthAnim.SetTrigger("gotHit");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //Jon put this here
            if (attackDamage < 100)
            {
                myPlayer.SetVibration(0, light_Vib, light_Time);
            }
            else if (attackDamage >= 100)
            {
                myPlayer.SetVibration(0, Heavy_Vib, Heavy_Time);
            }

            if (isJumping)
            {
                isJumping = false;
            }

            if (gotHitTimer < 0)
            {
                teamController.GetComponent<SwitchHandler>().UpdateUltBar(attackDamage);
                currentHealth -= attackDamage;
                hitAngle = attackAngle;
                regenHeath -= attackDamage * regenHeathMultiplier;
                velocity = new Vector3(0, 0, 0);
                maxDistance = distance;
                maxKnockbackTime = travelTime;
                currentKnockbackTime = 0;
                startPosition = transform.position;
                isAttacking = false;

                mainCamera.GetComponent<ShakeScreenScript>().SetVariables(duration, magnitude, slowDown);

                gotHitTimer = hitStun;
                knockback = attackForce;
                Vector3 dir = new Vector3(0, 0, 0);
                if (gotHitTimer > 0)
                {
                    if (facingRight)
                    {
                        dir = Quaternion.AngleAxis(attackAngle, Vector3.forward) * Vector3.right;
                        hitDirection = new Vector3(-dir.x, dir.y, dir.z);
                        endPosition = transform.position + (hitDirection.normalized * distance);
                        direction = "Right";
                    }
                    else
                    {
                        dir = Quaternion.AngleAxis(attackAngle, Vector3.forward) * Vector3.right;
                        hitDirection = dir;
                        endPosition = transform.position + (hitDirection.normalized * distance);
                        direction = "Left";
                    }
                }
                //rb.AddForce(dir * attackForce);
                // rb.AddForce(new Vector2(attackForce, 0));
            }
        }
    }

    void doAttackMovement()
    {

        //velocity = attackMovementDir * attackMovementSpeed;
        //rb.MovePosition(transform.position + attackMovementDir * attackMovementSpeed * Time.deltaTime);
        Debug.Log("Attack Timer: " + doAttackMovementTimer);

        if (direction == "Right")
        {
            Vector3 move = new Vector3(attackMovementSpeed, 0, 0);
            rb.MovePosition(transform.position + move * Time.deltaTime);
        }
        if (direction == "Left")
        {
            Vector3 move = new Vector3(-attackMovementSpeed, 0, 0);
            rb.MovePosition(transform.position + move * Time.deltaTime);
        }


    }


    public void AttackMovement(float speed, float duration, float delayTime, Vector2 direction)
    {
        attackMovementSpeed = speed;
        doAttackMovementTimer = duration;
        attackMovementDir = direction;
        attackTimerDelay = delayTime;
    }

        

	IEnumerator CalcVelocity()
	{
		while (Application.isPlaying)
		{
			// Position at frame start
			previousPos = transform.position;
			// Wait till it is the end of the frame
			yield return new WaitForEndOfFrame();
			// Calculate velocity: Velocity = DeltaPosition / DeltaTime
			currentVelocity = (previousPos - transform.position) / Time.deltaTime;
			//Debug.Log(velocity);
		}
	}

    IEnumerator CheckAttacking()
    {
        while(Application.isPlaying)
        //checking the isAttacking boolean and making sure it isn't on when it shouldn't be. 
        //This is specific to each character since we need to give the name of the animation that is currently playing so it will either stay here or eventually it will be moved to their own specific scripts.
        if (claire)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Crystal Idle"))
            {
                yield return new WaitForEndOfFrame();
                isAttacking = false;
            }
        }

        if (gnomercy)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Mercy Idle"))
            {
                yield return new WaitForEndOfFrame();
                isAttacking = false;
            }
        }

        if (gillbert)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Fish Idle"))
            {
                yield return new WaitForEndOfFrame();
                isAttacking = false;
            }
        }

        if (wawa)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Wawa Idle"))
            {
                yield return new WaitForEndOfFrame();
                isAttacking = false;
            }
        }
    }

	IEnumerator DoJumpAnim()
	{
		anim.SetTrigger("Jump");
		yield return new WaitForSeconds(.2f);
		anim.ResetTrigger("Jump");
	}

	void HasJumped()
	{
		isJumping = false;
	}

    void StartGetUp()
    {
        inHitAnims = true;
    }

    void EndGetUp()
    {
        inHitAnims = false;
    }

    public bool FacingRight()
	{
		if ((makeFaceRight && transform.localScale.x < 0) || (!makeFaceRight && transform.localScale.x > 0))
		{
			//Debug.Log("Right");
			return true;
		}
		else
		{
			//Debug.Log("Left");
			return false;
		}
	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		foreach (ContactPoint2D contact in collisionInfo.contacts)
		{
			//am I coming from the top/bottom?
			if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
			{
				velocity.y = 0; //stop vertical velocity
                hitHead = true;
				if (contact.normal.y >= 0)
				{ //am I hitting the top of the platform?

                    isJumping = false;
                    //isAttacking = false;
                    if (landtimer < 0)
                    {
                        anim.SetTrigger("land");
                        landtimer = 1;
                    }
					onTopOfPlatform = true;
					hitHead = false;
					velocity.y = 0;
                    if (collisionInfo.gameObject.tag != "Player")
                    {
                        if (maxKnockbackTime > 0)
                        {
                            maxKnockbackTime /= 4;
                            knockback /= 4;
                            gotHitTimer = 0;
                            maxDistance = 0;
                        }
                    }
                    if(collisionInfo.gameObject.tag == "Platform")
                    {
                        this.transform.parent = collisionInfo.transform;
                        anim.SetBool("OnPlatform", true);
                        
                    }
                }
				//am I hitting the bottom of a platform?
				if(contact.normal.y < 0)
				{
					//hitHead = true;
					//velocity.y = 0;
                    //gotHitTimer = 0;
                    //maxKnockbackTime = 0;
                   
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
                        initialJumpTime = 0;
                       
					}
					onTopOfPlatform = true;
				}
			}
		}
	}

    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Platform")
        {
            this.transform.parent = null;
            anim.SetBool("OnPlatform", false);
            
        }
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?
                    if (velocity.y < 0)
                    {
                        //velocity.y = 0; //stop vertical velocity
                        onTopOfPlatform = false;
                        isJumping = true;
                    }
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
