using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public SpriteRenderer[] rigPieces;
    public int teamNum;

    [Header("UI Stuff")]
    public Image healthBar;
    public Image regenableHealthBar;
    Animator healthAnim;

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
    public enum playerState { FreeMovement, Knockback}
    public playerState player;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public GameObject teamController;

    private pauserScript m_my_Pauser;

    GameObject mainCamera;

    bool findTeamController = false;

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
    public float deadZone;

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

    private void Awake()
    {

        mainCamera = GameObject.Find("Main Camera");

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

    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthAnim = healthBar.GetComponent<Animator>();
        m_my_Pauser = pauserScript.pauser_Instance;

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
        //resetting the health in case it ever goes above
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        //setting the health
        if (healthBar != null && regenableHealthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            regenableHealthBar.fillAmount = regenHeath / maxHealth;
        }

        Movement();

        if (!isAttacking)
        {

            Attack();

        }

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
            //Hey Nick. Itsa me. Your good pal Nolan. Let me know if you find this
        }

        if (myPlayer.GetButtonDown("Switch"))
        {
            Debug.Log(teamController.name);
            try
            {
                teamController.GetComponent<SwitchHandler>().BeginSwap(playerNum);
            }
            catch
            {
            }
        }

        if (myPlayer.GetButtonDown("Pause"))
        {
            m_my_Pauser.Pauser();
        }

    }

    private void FixedUpdate()
    {

        FixedMovement();

        Gravity();

        //always running this so that everything can be based off of gravity
        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

    }

    private void LateUpdate()
    {
        onTopOfPlatform = false;
    }

    void Movement()
    {
        if(player == playerState.FreeMovement)
        {

            if (Mathf.Abs(myPlayer.GetAxis("Horizontal")) > deadZone)
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

            if (onPlatformTimer > .05 && !isAttacking)
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
            if (myPlayer.GetButtonDown("Jump") && onPlatformTimer > 0 && !isAttacking)
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

        anim.SetBool("isAttacking", isAttacking);


        if (player == playerState.FreeMovement)
        {

            //checking to see where the joystick is
            joyPos = new Vector2(myPlayer.GetAxis("Horizontal"), myPlayer.GetAxis("Vertical"));

            if (onPlatformTimer > .05f)
            {

                if (myPlayer.GetButtonDown("BasicAttack"))
                {

                    if (Mathf.Abs(joyPos.x) < deadZone && Mathf.Abs(joyPos.y) < deadZone)
                    {
                        anim.SetFloat("Attack", 0);

                    }
                    if ((Mathf.Abs(joyPos.x) > deadZone) && (Mathf.Abs(joyPos.x) >= Mathf.Abs(joyPos.y)))
                    {
                        anim.SetFloat("Attack", 1);
                    }
                    if (joyPos.y > deadZone && joyPos.y > Mathf.Abs(joyPos.x))
                    {
                        anim.SetFloat("Attack", 2);
                    }
                    if (Mathf.Abs(joyPos.y) > deadZone && joyPos.y < joyPos.x && Mathf.Abs(joyPos.y) > joyPos.x)
                    {
                        anim.SetFloat("Attack", 3);
                    }

                    anim.ResetTrigger("Basic");
                    anim.SetTrigger("Basic");
                    isAttacking = true;

                }

                if (myPlayer.GetButtonDown("HeavyAttack"))
                {

                    if (Mathf.Abs(joyPos.x) < deadZone && Mathf.Abs(joyPos.y) < deadZone)
                    {
                        anim.SetFloat("Attack", 0);

                    }
                    if ((Mathf.Abs(joyPos.x) > deadZone) && (Mathf.Abs(joyPos.x) >= Mathf.Abs(joyPos.y)))
                    {
                        anim.SetFloat("Attack", 1);
                    }
                    if (joyPos.y > deadZone && joyPos.y > Mathf.Abs(joyPos.x))
                    {
                        anim.SetFloat("Attack", 2);
                    }
                    if (Mathf.Abs(joyPos.y) > deadZone && joyPos.y < joyPos.x && Mathf.Abs(joyPos.y) > joyPos.x)
                    {
                        anim.SetFloat("Attack", 3);
                    }

                    anim.ResetTrigger("Heavy");
                    anim.SetTrigger("Heavy");
                    isAttacking = true;

                }
            }
            else
            {
                if (myPlayer.GetButtonDown("BasicAttack") || myPlayer.GetButtonDown("HeavyAttack"))
                {

                    if (Mathf.Abs(joyPos.x) < deadZone && Mathf.Abs(joyPos.y) < deadZone)
                    {
                        anim.SetFloat("Attack", 0);

                    }
                    if ((Mathf.Abs(joyPos.x) > deadZone) && (Mathf.Abs(joyPos.x) >= Mathf.Abs(joyPos.y)))
                    {
                        anim.SetFloat("Attack", 0);
                    }
                    if (joyPos.y > deadZone && joyPos.y > Mathf.Abs(joyPos.x))
                    {
                        anim.SetFloat("Attack", 1);
                    }
                    if (Mathf.Abs(joyPos.y) > deadZone && joyPos.y < joyPos.x && Mathf.Abs(joyPos.y) > joyPos.x)
                    {
                        anim.SetFloat("Attack", 2);
                    }

                    anim.ResetTrigger("Air");
                    anim.SetTrigger("Air");
                    isAttacking = true;

                }
            }
            //ultimate abilities
            /*
            if (myPlayer.GetButtonDown("Ultimate"))
            {
                if (teamController.GetComponent<SwitchHandler>().specialMeter[2].fillAmount >= .2)
                {
                    teamController.GetComponent<SwitchHandler>().currentUltNum = 0;

                    if (claire) { claireCharacter.ClaireAttackController(69); }

                    if (gillbert) { gillbertCharacter.GilbertAttackController(69); }

                    teamController.GetComponent<SwitchHandler>().UpdateUltBar(0);
                }
            }
            */

        }

    }
    /// <summary>
    /// This function gets called when an enemy hits you
    /// </summary>
    /// <param name="attackDamage">is the how much the players health/armor goes down.</param>
    /// <param name="attackAngle">is the angle you get sent flying when you get hit. Use x and y variables to determine the angle, y should never be less than 0[*possibly* affected by player weight]</param>
    /// <param name="attackForce"> is how far back you get sent flying. Use x and y variables to determine if you want to have more upward velocity than x or so on.[affected by player weight]</param>
    /// <param name="hitStun">is how long the player has to wait before they can do anything</param>
    /// <param name="facingRight">Checks which way the player is facing when they do the attack so that it knows whether or not to reverse the knockback</param>
    /// <param name="duration">How long the screen shake lasts</param>
    /// <param name="magnitude">How agressively the screen shakes</param>
    /// <param name="slowDown">How quickly the camera stops shaking</param>
    public void GetHit(float attackDamage, Vector2 attackAngle, float attackForce, float hitStun, bool facingRight, float duration, float magnitude, float slowDown)//im probably missing a few arguments
    {
        if (claire && claireCharacter.shield)
        {
            //do nothing
        }
        else
        {
            player = playerState.Knockback;
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
            teamController.GetComponent<SwitchHandler>().UpdateUltBar(attackDamage);
            currentHealth -= attackDamage;
            regenHeath -= attackDamage * regenHeathMultiplier;
            velocity = new Vector3(0, 0, 0);
            velocity = new Vector3(-attackAngle.x * attackForce, attackAngle.y * attackForce, velocity.z);
            direction = "Right";
            if (facingRight)
            {
                velocity = attackAngle * attackForce;
                direction = "Left";
            }
            else 
            {
                velocity = new Vector3(-attackAngle.x * attackForce, attackAngle.y * attackForce, velocity.z);
                direction = "Right";
            }
            StartCoroutine(HitStun(hitStun));
            isAttacking = false;
            mainCamera.GetComponent<ShakeScreenScript>().SetVariables(duration, magnitude, slowDown);
        }
    }

    IEnumerator HitStun(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        player = playerState.FreeMovement;
    }

    public void ResetTriggers()
    {
        anim.ResetTrigger("Basic");
        anim.ResetTrigger("Heavy");
        anim.ResetTrigger("Land");
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
                if (collisionInfo.gameObject.tag != "Player")
                {
                    velocity.y = 0; //stop vertical velocity
                    if (contact.normal.y >= 0)
                    { //am I hitting the top of the platform?
                        onTopOfPlatform = true;
                        anim.ResetTrigger("Land");
                        anim.SetTrigger("Land");
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
