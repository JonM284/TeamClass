using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_Movement : MonoBehaviour
{

    public GameObject myApple;
    public Transform myEndPos;
    public Vector3 myStartPos;
    public bool Apple_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;
    public int damageToDealToPlayer;
    public SpriteRenderer[] appleSprites;
    public CircleCollider2D[] appleColliders;

    public float removeApplesTimer, removeApplesLength;
    public bool ApplesAreThere;
    public float fallSpeed;
    private Color myColor;

    public Vector2 attackAngleApple;

    // Start is called before the first frame update
    void Start()
    {
        removeApplesTimer = 0f;
        removeApplesLength = 2f;
        ApplesAreThere = false;
        myStartPos = myApple.transform.position;
        appleSprites = myApple.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer apple in appleSprites)
        {
            apple.enabled = false;
        }
        appleColliders = myApple.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D appleCollider in appleColliders)
        {
            appleCollider.enabled = false;
        }
        damageToDealToPlayer = 25;
        fallSpeed = 10f;
        myColor = new Color(255, 255, 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to send Apples, send Apples and do Activate_Apple();
        if (Apple_Active)
        {
            Activate_Apple();
        }

        //if the Apples are on map currently, count timer down til its less than 0, then....
        if (removeApplesTimer > 0 && ApplesAreThere == true)
        {
            removeApplesTimer -= Time.deltaTime;
            if (removeApplesTimer < 1)
            {
                myColor.a -= .02f;
                appleSprites[0].color = myColor;
                appleSprites[1].color = myColor;
                appleSprites[2].color = myColor;
            }

        }
        //...disable Apples and reset its position for next time machine is used
        if (removeApplesTimer <= 0 && ApplesAreThere == true)
        {
            ApplesAreThere = false;
            foreach (SpriteRenderer apple in appleSprites)
            {
                apple.enabled = false;
            }
            foreach (CircleCollider2D appleCollider in appleColliders)
            {
                appleCollider.enabled = false;
            }
            myApple.transform.position = myStartPos;
            fallSpeed = 10f;
            myColor.a = 1;
            appleSprites[0].color = myColor;
            appleSprites[1].color = myColor;
            appleSprites[2].color = myColor;
        }

    }

    //the function done to move the apple downwards
    public void Activate_Apple()
    {
        //myApple.transform.position = Vector3.Lerp(myApple.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        myApple.transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
        fallSpeed += .2f;
    }

    //the function that happens when the apple hits the invisible collider to stop
    public void Deactivate_Apple()
    {
        Apple_Active = false;
        ApplesAreThere = true;
        removeApplesTimer = removeApplesLength;
        foreach (CircleCollider2D appleCollider in appleColliders)
        {
            appleCollider.enabled = false;
        }
        myApple.transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //disables the apple's hitbox and starts timer til it disappears
        if (other.gameObject.name == "Apple_EndPos")
        {
            Deactivate_Apple();
        }
        //deals damage to player if apple hits player
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(damageToDealToPlayer, 90f, 100f, /*Stun Time*/.5f, 50f, .5f, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0.1f, 0.15f, 0.1f);
            other.gameObject.GetComponent<BasePlayer>().GetHit(damageToDealToPlayer, attackAngleApple, 100f, .5f, other.gameObject.GetComponent<BasePlayer>().FacingRight(), .1f, .15f, .1f);
        }
    }

}
