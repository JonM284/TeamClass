using System.Collections;
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

	public Vector3 velocity;
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
	private float regenHeath;
	public float regenHeathMultiplier;
	public bool makeFaceRight;

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

    [Header("Dash")]
    public float dashSpeed;
    public float maxDashTimer;
    private float currentDashTimer = 0;
    private float prevJoystickAxis = 0;
    private bool didntTurnAround = true;

    GameObject mainCamera;
    

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

	}

    // Update is called once per frame
    void Update()
    {
        currentDashTimer -= Time.deltaTime;

        //<3 for Justin  
        //-Love Dan 🦆
        //Here's additional recognition for our one true leader throughout this endeavorous task
        //set upon us, for without him we are nothing.
        //Praise be to our one and only team manager Patrick ♥♥♥♥♥♥♥♥
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

		//Debug.Log(isAttacking);
        stunTime -= Time.deltaTime;

		//checking the isAttacking boolean and making sure it isn't on when it shouldn't be. 
		//This is specific to each character since we need to give the name of the animation that is currently playing so it will either stay here or eventually it will be moved to their own specific scripts.
		if (claire)
		{
			if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Crystal Idle"))
			{
				checkAttackTimer += Time.deltaTime;
			}
			else
			{
				checkAttackTimer = 0;
			}
			if (checkAttackTimer >= .1)
			{
				isAttacking = false;
			}
		}

		if (gillbert)
		{
			if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Fish Idle"))
			{
				checkAttackTimer += Time.deltaTime;
			}
			else
			{
				checkAttackTimer = 0;
			}
			if (checkAttackTimer >= .1)
			{
				isAttacking = false;
			}
		}

        if (wawa)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Wawa Idle"))
            {
                checkAttackTimer += Time.deltaTime;
            }
            else
            {
                checkAttackTimer = 0;
            }
            if (checkAttackTimer >= .1)
            {
                isAttacking = false;
            }
        }

		if (healthBar != null && regenableHealthBar != null)
		{
			healthBar.fillAmount = currentHealth / maxHealth;
			regenableHealthBar.fillAmount = regenHeath / maxHealth;
		}

		if (!isAttacking && !isJumping && stunTime <= 0)
		{
			Attack();
		}

        if (!isAttacking || !onTopOfPlatform && stunTime <= 0)
        {
            Movement();
        }

    }

 void FixedUpdate()
	{
        if(gotHitTimer > 0)
        {
            anim.SetBool("hitstun", true);
        }
        else
        {
            anim.SetBool("hitstun", false);
        }

		if (!isAttacking || onPlatformTimer < 0 && stunTime <= 0)
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

		//animation logic for just the fighter
		anim.SetBool("isAttacking", isAttacking);

		gotHitTimer -= Time.deltaTime;

		//seing which way the player is moving
		if (myPlayer.GetAxisRaw("Horizontal") > 0)
		{
			direction = "Right";
		}
		else if (myPlayer.GetAxisRaw("Horizontal") < 0)
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
					accel -= decelMult;
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
					accel += decelMult;
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
			if (onPlatformTimer > 0 && !isAttacking)
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

			if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0)
			{
                isJumping = true;
                initialJumpTime = maxInitialJumpTime;
				holdJumpTime = maxHoldJumpTime;
				jumpButtonPressed = true;
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
				if (jumpButtonPressed)
				{
					velocity.y += .4f;
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
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer > 0)
			{

				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    if (claire) { claireCharacter.ClaireAttackController(1); }

                    //if (gillbert) { gillbertCharacter.GilbertAttackController(1); }
				}
			}
            
            //forward basic attack
            if (((myPlayer.GetAxis("Horizontal") > .3f && myPlayer.GetAxis("Horizontal") > -.3f) || (myPlayer.GetAxis("Horizontal")) < .3f && myPlayer.GetAxis("Horizontal") < -.3f) && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack"))
                {
                    if (claire) { claireCharacter.ClaireAttackController(2); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(2); }

                    if (wawa) { wawaCharacter.WawaAttackController(2); }
                }
            }

            //up basic attack
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer > 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    if (claire) { claireCharacter.ClaireAttackController(3); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(3); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(3); }
				}
			}


            //Down basic attack
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") < -.3f && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack"))
                {
                    if (claire) { claireCharacter.ClaireAttackController(4); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(4); }

                    if (wawa) {wawaCharacter.WawaAttackController(4); }
                }
            }

            //neutral air attack
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer < 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    if (claire) { claireCharacter.ClaireAttackController(9); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(9); }

                    if (wawa) { wawaCharacter.WawaAttackController(9); }

                }
			}

			//up air attack
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer < 0)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    if (claire) { claireCharacter.ClaireAttackController(10); }

                    //if (gillbert) { gillbertCharacter.GilbertAttackController(10); }
                }
			}

            //down air attack
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") < -.3f && onPlatformTimer < 0)
            {
                if (myPlayer.GetButtonDown("BasicAttack"))
                {
                    if (wawa) { wawaCharacter.WawaAttackController(11); }

                   
                }
            }

            //Neutral Heavy
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    if (claire) { claireCharacter.ClaireAttackController(20); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(20); }

                    if (wawa) { wawaCharacter.WawaAttackController(20); }
                }
            }

            //forward heavy attack
            if (((myPlayer.GetAxis("Horizontal") > .3f && myPlayer.GetAxis("Horizontal") > -.3f) || (myPlayer.GetAxis("Horizontal")) < .3f && myPlayer.GetAxis("Horizontal") < -.3f) && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    if (claire) { claireCharacter.ClaireAttackController(21); }

                    //if (gillbert) { gillbertCharacter.GilbertAttackController(21); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(21); }
				}
            }

            //Down Heavy
            if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") < -.3f && onPlatformTimer > 0)
            {
                if (myPlayer.GetButtonDown("HeavyAttack"))
                {
                    if (claire) { claireCharacter.ClaireAttackController(22); }

                    //if (gillbert) { gillbertCharacter.GilbertAttackController(22); }

					if (gnomercy) { gnomercyCharacter.GnomercyAttackController(22); }
				}
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

		rb.constraints = RigidbodyConstraints2D.None;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (isJumping)
		{
			isJumping = false;
		}

        if (gotHitTimer < 0)
        {
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
            //rb.AddForce(dir * attackForce);
            // rb.AddForce(new Vector2(attackForce, 0));
        }
    }

	void HasJumped()
	{
		isJumping = false;
	}

    public bool FacingRight()
	{
		if ((makeFaceRight && transform.localScale.x < 0) || (!makeFaceRight && transform.localScale.x > 0))
		{
			Debug.Log("Right");
			return true;
		}
		else
		{
			Debug.Log("Left");
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
				if (contact.normal.y >= 0)
				{ //am I hitting the top of the platform?
					anim.SetTrigger("land");
					onTopOfPlatform = true;
					hitHead = false;
					velocity.y = 0;
				}
				//am I hitting the bottom of a platform?
				if(contact.normal.y < 0)
				{
					hitHead = true;
					velocity.y = 0;
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
