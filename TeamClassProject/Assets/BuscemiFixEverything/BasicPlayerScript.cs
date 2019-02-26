using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	Rigidbody2D rb;

	private bool moving;

	[Header("Movement Variables")]
	public int speed;

	public Vector3 velocity;
	[Space(10)]

	[Header("Gravity Variables")]
	public float gravityUp;
	public float gravityDown;
	public float jumpVel;
	public float maxDownVel;
	public float onPlatformTimer;
	public float onPlatformTimerMax;
	public bool onTopOfPlatform;

	private void Awake()
	{
		//Rewired Code
		myPlayer = ReInput.players.GetPlayer(playerNum - 1);
		ReInput.ControllerConnectedEvent += OnControllerConnected;
		CheckController(myPlayer);
	}

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
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

		velocity.x = myPlayer.GetAxisRaw("Horizontal") * speed;

		if (Mathf.Abs(velocity.x) >= 1)
		{
			moving = true;
		}
		else
		{
			moving = false;
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
		}

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
