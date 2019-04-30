using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectile : MonoBehaviour
{

    public Vector3 velocity;
    public float maxDownVel;
    public float gravityUp;
    public float gravityDown;
    public float xSpeed;
    public float ySpeed;
    private float currentMoveUpTimer;
    public float maxMoveUpTimer;

    [Header("Attack Attributes")]
    public float damage;
    public Vector2 angle;
    public float knockback;
    public float hitStun;
    public float distance;
    public float travelTime;
    public float playerNum;
    public float shakeDuration;
    public float shakeMagnitude;
    public float shakeSlowDown;
    public Vector3 direction;
    public bool moveRight = true;
    public BasePlayer player;

    private SpriteRenderer sr;
    public Sprite[] spitSprites;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveUpTimer = maxMoveUpTimer;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetPhysicsVariables(float maxDownVelocity, float gravityUp1, float gravityDown1, float xSpeed1, float ySpeed1, float maxMoveUpTimer1)
    {
        maxDownVel = maxDownVelocity;
        gravityUp = gravityUp1;
        gravityDown = gravityDown1;
        xSpeed = xSpeed1;
        ySpeed = ySpeed1;
        maxMoveUpTimer = maxMoveUpTimer1;
        currentMoveUpTimer = maxMoveUpTimer;
    }

    public void SetVariables(float damage1, Vector2 angle1, float knockback1, float hitStun1, float distance1, float travelTime1, int playerNum1, float duration, float magnitude, float slowDown)
    {
        damage = damage1;
        angle = angle1;
        knockback = knockback1;
        hitStun = hitStun1;
        distance = distance1;
        travelTime = travelTime1;
        playerNum = playerNum1;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeSlowDown = slowDown;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Veg" + Mathf.Abs(velocity.y));
        /*
        if(Mathf.Abs(velocity.y) < 1)
        {
            sr.sprite = spitSprites[0];
        }else if(Mathf.Abs(velocity.y) < 3.5f)
        {
            sr.sprite = spitSprites[1];
        }
        else if(Mathf.Abs(velocity.y) < 7f)
        {
            sr.sprite = spitSprites[2];
        }
        else
        {
            sr.sprite = spitSprites[3];
        }
        */

        Gravity();

        currentMoveUpTimer -= Time.deltaTime;

       
    }

    private void FixedUpdate()
    {
        if (maxMoveUpTimer > currentMoveUpTimer)
        {
            velocity.x = xSpeed;
            velocity.y = ySpeed * (currentMoveUpTimer / maxMoveUpTimer);
        }

        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

    
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            if(other.transform.position.x <= transform.position.x)
            {
                moveRight = true;
            }
            else
            {
                moveRight = false;
            }

            try
            {
                if (other.transform.root.gameObject.GetComponent<BasePlayer>().playerNum != playerNum)
                {
                    other.gameObject.GetComponent<BasePlayer>().GetHit(damage, angle, knockback, hitStun, moveRight, shakeDuration, shakeMagnitude, shakeSlowDown);
                    player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(damage);
                    Destroy(gameObject);
                }
            }
            catch
            {

            }
        }
    }
}
