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

	Rigidbody2D rb;
	Animator anim;

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
	private float currentHealth;
	private float regenHeath;
	public float regenHeathMultiplier;
	public bool makeFaceRight;

	private float xScale;
	[HideInInspector]
	public bool isAttacking;

	private Vector3 hitDirection;
	private float gotHitTimer = 0;
	private float knockback = 0;

	private float maxKnockbackTime;
	private float currentKnockbackTime;
	private float maxDistance;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float hitAngle;

	void Awake()
	{
		//grabbing character specific values from their respective characters

		if (claire)
		{
			claireCharacter = this.GetComponent<Claire>();
			maxHealth = claireCharacter.maxHealth;
			currentHealth = maxHealth;
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
        Debug.Log(maxKnockbackTime);
        Debug.Log(currentKnockbackTime);
		healthBar.fillAmount = currentHealth / maxHealth;
		regenableHealthBar.fillAmount = regenHeath / maxHealth;

		if (!isAttacking)
		{
			Attack();
		}

        if (!isAttacking || !onTopOfPlatform)
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

		if (!isAttacking || !onTopOfPlatform)
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

			if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0)
			{
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

		rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

	}

	void Attack()
	{
		//neutral basic attack
		if (gotHitTimer < 0)
		{
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
			{

				if (myPlayer.GetButtonDown("BasicAttack"))
				{
					Debug.Log("Attack");
					anim.SetTrigger("BasicNeutral");
					isAttacking = true;
					accel = 0;
				}
			}

			//up basic attack
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
					anim.SetTrigger("BasicUp");
					isAttacking = true;
					accel = 0;
				}
			}

			//forward basic attack
			if (((myPlayer.GetAxis("Horizontal") > .3f && myPlayer.GetAxis("Horizontal") > -.3f) || (myPlayer.GetAxis("Horizontal")) < .3f && myPlayer.GetAxis("Horizontal") < -.3f) && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
					anim.SetTrigger("BasicForward");
					isAttacking = true;
					accel = 0;
				}
			}

			//neutral air attack
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && !onTopOfPlatform)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    anim.SetTrigger("NeutralAir");
                    isAttacking = true;
				}
			}

			//up air attack
			if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && !onTopOfPlatform)
			{
				if (myPlayer.GetButtonDown("BasicAttack"))
				{
                    anim.SetTrigger("UpAir");
                    isAttacking = true;
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
    public void GetHit(float attackDamage, float attackAngle, float attackForce, float hitStun, float distance, float travelTime, bool facingRight)//im probably missing a few arguments
    {
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

            gotHitTimer = hitStun;
            knockback = attackForce;
            Vector3 dir = new Vector3(0, 0, 0);
            if (facingRight)
            {
                dir = Quaternion.AngleAxis(attackAngle, Vector3.forward) * Vector3.right;
                hitDirection = dir;
                endPosition = transform.position + (hitDirection.normalized * distance);
                direction = "Right";
            }
            else
            {
                dir = Quaternion.AngleAxis(attackAngle, -Vector3.forward) * -Vector3.right;
                hitDirection = new Vector3(-dir.x, dir.y, dir.z);
                endPosition = transform.position + (hitDirection.normalized * distance);
                direction = "Left";
            }
            //rb.AddForce(dir * attackForce);
            // rb.AddForce(new Vector2(attackForce, 0));
        }
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
