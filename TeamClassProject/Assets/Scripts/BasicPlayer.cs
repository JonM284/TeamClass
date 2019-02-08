using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.ControllerExtensions;

public class BasicPlayer : MonoBehaviour {

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public Image healthBar;
    public Image regenableHealthBar;

    Rigidbody2D rb;

    private bool moving, startHitStun, inHitStun, preDashRight, preDashLeft, canDashRight, canDashLeft, dash;

    [Header("Movement Variables")]
    public float accel;
    public float accelMult;
    public float decelMult;
    [HideInInspector]
    public int speed;
    [HideInInspector]
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
    public float maxInitialJumpTime;
    private float initialJumpTime = 0;
    public float maxHoldJumpTime;
    private float holdJumpTime = 0;
    private bool jumpButtonPressed = false;
    public float maxDownVel;
    public float onPlatformTimer;
    public float onPlatformTimerMax;
    public bool onTopOfPlatform;
    [Space(10)]

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
    private float currentHealth;
    private float regenHeath;
    public float regenHeathMultiplier;
    public float characterSpeed;
    [HideInInspector]
    public float characterWeight;
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

        currentHealth = maxHealth;
        regenHeath = maxHealth;

        canDash = true;

        direction = "Right";

        if (makeFaceRight)
        {
            xScale = -gameObject.transform.localScale.x;
        }
        else
        {
            xScale = gameObject.transform.localScale.x;
        }
    }

    [HideInInspector]
    public enum animations { idle = 0, walk = 1, jump_start = 2, jump_land = 4, basic_neutral = 5, basic_forward = 6, basic_up = 7, basic_down = 8, neutral_air = 9, up_air = 10}
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(anim.GetInteger("State"));
        healthBar.fillAmount = currentHealth / maxHealth;
        regenableHealthBar.fillAmount = regenHeath / maxHealth;

        gotHitTimer -= Time.deltaTime;

        if (!inHitStun)
        {
            //i put the movement() stuff in fixed update
           // Movement();

        }

        if (!dash && !onTopOfPlatform)
        {
            Gravity();
        }

        if(anim.GetInteger("State") == (int)animations.neutral_air && onTopOfPlatform){
            anim.SetInteger("State", (int)animations.idle);
            isAttacking = false;
        }

        //float mySpeed = Mathf.Abs(rb.velocity.x);
        //anim.SetFloat("xSpeed", Mathf.Abs(velocity.x));
        //anim.SetFloat("yVel", rb.velocity.y);

        //neutral basic attack
        if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
        {
            if (myPlayer.GetButtonDown("BasicAttack"))
            {
                anim.SetInteger("State", (int)animations.basic_neutral);
                isAttacking = true;
                velocity.x = 0;
            }
        }

        //up basic attack
        if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
        {
            if (myPlayer.GetButtonDown("BasicAttack"))
            {
                anim.SetInteger("State", (int)animations.basic_up);
                isAttacking = true;
                velocity.x = 0;
            }
        }

        //forward basic attack
        if (((myPlayer.GetAxis("Horizontal") > .3f && myPlayer.GetAxis("Horizontal") > -.3f) || (myPlayer.GetAxis("Horizontal")) < .3f && myPlayer.GetAxis("Horizontal") < -.3f) && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && onTopOfPlatform)
        {
            if (myPlayer.GetButtonDown("BasicAttack"))
            {
                anim.SetInteger("State", (int)animations.basic_forward);
                isAttacking = true;
                velocity.x = 0;
            }
        }

        //neutral air attack
        if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") < .3f && Input.GetAxis("Vertical") > -.3f && !onTopOfPlatform)
        {
            if (myPlayer.GetButtonDown("BasicAttack"))
            {
                anim.SetInteger("State", (int)animations.neutral_air);
                isAttacking = true;
            }
        }

        //up air attack
        if (myPlayer.GetAxis("Horizontal") < .3f && myPlayer.GetAxis("Horizontal") > -.3f && Input.GetAxis("Vertical") > .3f && Input.GetAxis("Vertical") > -.3f && !onTopOfPlatform)
        {
            if (myPlayer.GetButtonDown("BasicAttack"))
            {
                anim.SetInteger("State", (int)animations.up_air);
                isAttacking = true;
            }
        }
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
                //sr.flipX = false;
                gameObject.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            }
            if (direction == "Left")
            {
                //sr.flipX = true;
                gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
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
            //velocity.y = jumpVel;
            //playerJump.Play();
            //anim.SetTrigger("jumpStart");
        }
    }
    

    private void FixedUpdate()
    {
        if(anim.GetInteger("State") == (int)animations.jump_land)
        {
            //velocity.x = 0;
        }

        initialJumpTime -= Time.fixedDeltaTime;
        //changed rb.velocity to just velocity and then put rb.moveposition outside of !inhitstun

        rb.MovePosition(transform.position + velocity * Time.deltaTime);

        //knockback stuff
        if (currentKnockbackTime/maxKnockbackTime < .98f)
        {
            Knockback();
            //velocity = (hitDirection * knockback);
        }
        if (!inHitStun && !isAttacking)
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
                if (anim.GetInteger("State") != (int)animations.jump_start && onTopOfPlatform && anim.GetInteger("State") != (int)animations.jump_land)
                {
                    anim.SetInteger("State", (int)animations.walk);
                }
            }
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
                if (anim.GetInteger("State") != (int)animations.jump_start && anim.GetInteger("State") != (int)animations.jump_land)
                {
                    anim.SetInteger("State", (int)animations.idle);
                }
            }

            if (onTopOfPlatform)
            {
                if (direction == "Right")
                {
                    //sr.flipX = false;
                    gameObject.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
                }
                if (direction == "Left")
                {
                    //sr.flipX = true;
                    gameObject.transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
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
            if (initialJumpTime <= 0)
            {
                holdJumpTime -= Time.fixedDeltaTime;
            }

            if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0)
            {
                initialJumpTime = maxInitialJumpTime;
                holdJumpTime = maxHoldJumpTime;
                jumpButtonPressed = true;
                anim.SetInteger("State", (int)animations.jump_start);
                //playerJump.Play();
                //anim.SetTrigger("jumpStart");
            }
            if (myPlayer.GetButtonUp("Jump"))
            {
                jumpButtonPressed = false;
            }

            //initial jump
            if (initialJumpTime > 0)
            {
              velocity.y = jumpVel;
            }

            //hold jump
            if(initialJumpTime <=0 && jumpButtonPressed && holdJumpTime > 0)
            {
              velocity.y = jumpVel;
            }


        }

    }

    private void LateUpdate()
    {

        onTopOfPlatform = false;

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
            regenHeath -= attackDamage * regenHeathMultiplier;
            velocity = new Vector3(0, 0, 0);
            maxDistance = distance;
            maxKnockbackTime = travelTime;
            currentKnockbackTime = 0;
            startPosition = transform.position;

            gotHitTimer = hitStun;
            knockback = attackForce;
            Vector3 dir = new Vector3(0, 0, 0);
            if (facingRight)
            {
                dir = Quaternion.AngleAxis(attackAngle, Vector3.forward) * Vector3.right;
                hitDirection = dir;
                endPosition = transform.position + (hitDirection.normalized * distance);
            }
            else
            {
                dir = Quaternion.AngleAxis(attackAngle, -Vector3.forward) * -Vector3.right;
                hitDirection = dir;
                endPosition = transform.position + (hitDirection.normalized * distance);
            }
            //rb.AddForce(dir * attackForce);
            // rb.AddForce(new Vector2(attackForce, 0));
        }
    }

    public bool FacingRight()
    {
        if ((makeFaceRight && transform.localScale.x < 0) || (!makeFaceRight && transform.localScale.x > 0) )
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
                  //if (onPlatformTimer < 0)
                  //{
                    Debug.Log("hi");
                    anim.SetInteger("State", (int)animations.jump_land);
                    isAttacking = false;
                    //}
                    onTopOfPlatform = true;
                        dashCount = dashCountMax;
                    //}
                }
            }
        }
    }

    public void EndLandAnim()
    {
        anim.SetInteger("State", (int)animations.idle);
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
