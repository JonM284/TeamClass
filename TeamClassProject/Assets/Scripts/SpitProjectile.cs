using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectile : MonoBehaviour
{

    public Vector3 velocity;
    public float maxDownVel;
    public float gravityUp;
    public float gravityDown;
    public float speed;
    private float currentMoveUpTimer;
    public float maxMoveUpTimer;

    [Header("Attack Attributes")]
    public float damage;
    public float angle;
    public float knockback;
    public float hitStun;
    public float distance;
    public float travelTime;
    public float playerNum;
    public Vector3 direction;
    public bool moveRight = true;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveUpTimer = maxMoveUpTimer;
    }

    public void SetPhysicsVariables(float maxDownVelocity, float gravityUp1, float gravityDown1, float speed1, float maxMoveUpTimer1)
    {
        maxDownVel = maxDownVelocity;
        gravityUp = gravityUp1;
        gravityDown = gravityDown1;
        speed = speed1;
        maxMoveUpTimer = maxMoveUpTimer1;
        currentMoveUpTimer = maxMoveUpTimer;
    }

    public void SetVariables(float damage1, float angle1, float knockback1, float hitStun1, float distance1, float travelTime1, int playerNum1)
    {
        damage = damage1;
        angle = angle1;
        knockback = knockback1;
        hitStun = hitStun1;
        distance = distance1;
        travelTime = travelTime1;
        playerNum = playerNum1;
    }

    // Update is called once per frame
    void Update()
    {
        

        Gravity();

        currentMoveUpTimer -= Time.deltaTime;

       
    }

    private void FixedUpdate()
    {
        if (maxMoveUpTimer > currentMoveUpTimer)
        {
            velocity.y = speed * (currentMoveUpTimer / maxMoveUpTimer);
        }

        GetComponent<Rigidbody2D>().MovePosition(transform.position + velocity * Time.fixedDeltaTime);
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
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
                if (other.transform.root.gameObject.GetComponent<BasicPlayerScript>().playerNum != playerNum)
                {
                    other.gameObject.GetComponent<BasicPlayerScript>().GetHit(damage, angle, knockback, hitStun, distance, travelTime, moveRight);
                    Destroy(gameObject);
                }
            }
            catch
            {

            }
        }
    }
}
