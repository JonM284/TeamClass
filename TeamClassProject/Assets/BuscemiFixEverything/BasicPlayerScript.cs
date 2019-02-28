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

	private bool moving, hitHead;

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
	Claire claireCharacter;
	Gillbert gillbertCharacter;
	Gnomercy gnomercyCharacter;

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
			speed = gnomercyCharacter.speed;
			weight = gnomercyCharacter.weight;
			gravityUp = gnomercyCharacter.gravityUp;
			gravityDown = gnomercyCharacter.gravityDown;
			jumpVel = gnomercyCharacter.jumpVel;
			maxDownVel = gnomercyCharacter.maxDownVel;
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

	}

    // Update is called once per frame
    void Update()
    {
		Movement();

		if (!onTopOfPlatform)
		{
			Gravity();
		}
    }

	private void FixedUpdate()
	{
		
	}

	private void LateUpdate()
	{
		
	}

	void Movement()
	{

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

		velocity.x = accel * speed;

		if(onPlatformTimer > 0)
		{
			if(direction == "Right")
			{
				gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
			}
			if(direction == "Left")
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

		//initial jump
		if (initialJumpTime > 0 && !hitHead)
		{
			velocity.y = jumpVel;
		}
		else if(initialJumpTime > 0 && hitHead)
		{
			velocity.y = 0;
		}

		//hold jump
		if (initialJumpTime <= 0 && jumpButtonPressed && holdJumpTime > 0 && !hitHead)
		{
			velocity.y = jumpVel;
		}
		else if(initialJumpTime <= 0 && jumpButtonPressed && holdJumpTime > 0 && hitHead)
		{
			velocity.y = 0;
		}

		initialJumpTime -= Time.deltaTime;

		//set timer that will let the player jump slightly off the platform
		if (onTopOfPlatform && velocity.y == 0)
        {
            onPlatformTimer = onPlatformTimerMax;
        }
        else
        {
            onPlatformTimer -= Time.deltaTime;
        }

		/*
		//jump logic
		if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0)
		{
			velocity.y = jumpVel;
		}
		*/
		rb.MovePosition(transform.position + velocity * Time.deltaTime);

		onTopOfPlatform = false;

	}

	void Gravity()
	{
		//gravity logic
		if (velocity.y > -maxDownVel)
		{ //if we haven't reached maxDownVel
			if (velocity.y > 0)
			{ //if player is moving up
				velocity.y -= gravityUp * Time.deltaTime;
			}
			else
			{ //if player is moving down
				velocity.y -= gravityDown * Time.deltaTime;
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
				velocity.y = 0; //stop vertical velocity
				if (contact.normal.y >= 0)
				{ //am I hitting the top of the platform?
					onTopOfPlatform = true;
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
					onTopOfPlatform = true;
				}
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		hitHead = false;
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
