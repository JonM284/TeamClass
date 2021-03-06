﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branbull_Movement : MonoBehaviour
{

    public GameObject myBranbull;
    public Transform myEndPos;
    public Vector3 myStartPos;
    public bool Branbull_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;
    public int damageToDealToPlayerOnImpact;
    public GameObject branbullExploded;

    public float removeBranbullTimer, removeBranbullLength;
    public bool BranbullIsThere;
    public float fallSpeed;

    private Color myColor;

    public Vector2 attackAngleBrambleSendPlayerDown;
    public Vector2 attackAngleBrambleSendPlayerUp;

    // Start is called before the first frame update
    void Start()
    {
        removeBranbullTimer = 0f;
        removeBranbullLength = 4.0f;
        BranbullIsThere = false;
        branbullExploded = GameObject.Find("Branbull_Exploded");
        myStartPos = myBranbull.transform.position;
        myBranbull.GetComponent<SpriteRenderer>().enabled = false;
        branbullExploded.GetComponent<SpriteRenderer>().enabled = false;
        myBranbull.GetComponent<BoxCollider2D>().enabled = false;
        branbullExploded.GetComponent<BoxCollider2D>().enabled = false;
        fallSpeed = 10f;

        damageToDealToPlayerOnImpact = 50;
        myColor = new Color(255, 255, 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to send Branbull, send Branbull and do Activate_Branbull();
        if (Branbull_Active)
        {
            Activate_Branbull();
        }

        //if the Brandbull is on map currently, count timer down til its less than 0, then....
        if (removeBranbullTimer > 0 && BranbullIsThere == true)
        {
            removeBranbullTimer -= Time.deltaTime;
            if (removeBranbullTimer < 1)
            {
                myColor.a -= .02f;
                branbullExploded.GetComponent<SpriteRenderer>().color = myColor;
            }
        }
        //...disable Brandbull and reset its position for next time machine is used
        if (removeBranbullTimer <= 0 && BranbullIsThere == true)
        {
            BranbullIsThere = false;
            myBranbull.GetComponent<SpriteRenderer>().enabled = false;
            myBranbull.GetComponent<BoxCollider2D>().enabled = false;
            branbullExploded.GetComponent<SpriteRenderer>().enabled = false;
            branbullExploded.GetComponent<BoxCollider2D>().enabled = false;
            myBranbull.transform.position = myStartPos;
            fallSpeed = 10f;
            myColor.a = 1f;
            branbullExploded.GetComponent<SpriteRenderer>().color = myColor;
        }


    }

    //the function done to move the branbull downwards
    public void Activate_Branbull()
    {
        //myBranbull.transform.position = Vector3.Lerp(myBranbull.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        myBranbull.transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
        fallSpeed += .1f;
    }

    //the function that happens when the branbull hits the floor or platform to explode branbull

    IEnumerator DeactivateDelay()
    {
        Branbull_Active = false;
        //myBranbull.transform.position = myStartPos;
        myBranbull.GetComponent<SpriteRenderer>().enabled = false;
        myBranbull.GetComponent<BoxCollider2D>().enabled = false;
        BranbullIsThere = true;
        removeBranbullTimer = removeBranbullLength;
        yield return new WaitForSeconds(0.1f);
        branbullExploded.GetComponent<SpriteRenderer>().enabled = true;
        branbullExploded.GetComponent<BoxCollider2D>().enabled = true;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        //explodes the branbull when it hits the floor or a platform
        if (Branbull_Active == true && other.gameObject.name == "platformLeft")
        {
            StartCoroutine(DeactivateDelay());
            //Deactivate_Branbull();
            transform.position = new Vector3(transform.position.x, 2.06f, transform.position.z);
        }
        if (Branbull_Active == true && other.gameObject.name == "platformRight_left")
        {
            StartCoroutine(DeactivateDelay());
            //Deactivate_Branbull();
            transform.position = new Vector3(transform.position.x, -1.024f, transform.position.z);
        }
        if (Branbull_Active == true && other.gameObject.name == "platformRight_right")
        {
            StartCoroutine(DeactivateDelay());
            //Deactivate_Branbull();
            transform.position = new Vector3(transform.position.x, -.364f, transform.position.z);
        }
        if (Branbull_Active == true && other.gameObject.name == "stump")
        {
            StartCoroutine(DeactivateDelay());
            //Deactivate_Branbull();
            transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);
        }
        if (Branbull_Active && other.gameObject.tag == "Floor")
        {
            StartCoroutine(DeactivateDelay());
            transform.position = new Vector3(transform.position.x, -1.84f, transform.position.z);
        }



    }
    private void OnTriggerStay2D(Collider2D other)
    {
        /*//if the player is standing on exploded branbull it deals damage every frame
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(2f, 0, 0, 0, 0, 0, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0, 0,0 );
        }*/
        if (myBranbull.GetComponent<SpriteRenderer>().enabled == true && other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(20f, 270f, 200f, 0.2f, 60f, .2f, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0.1f, 0.3f, 0.2f);
            other.gameObject.GetComponent<BasePlayer>().GetHit(damageToDealToPlayerOnImpact, attackAngleBrambleSendPlayerDown, 20f, .2f, other.gameObject.GetComponent<BasePlayer>().FacingRight(), .1f, .3f, .2f);
        }
        if (other.gameObject.transform.position.x > transform.position.x)
        {
            if (branbullExploded.GetComponent<SpriteRenderer>().enabled == true && other.gameObject.tag == "Player")
            {
                //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(35f, 45, 150, 0.2f, 100f, 0.5f, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0.1f, 0.3f, 0.2f);
                other.gameObject.GetComponent<BasePlayer>().GetHit(35f, attackAngleBrambleSendPlayerUp, 15, .2f, other.gameObject.GetComponent<BasePlayer>().FacingRight(), .1f, .3f, .2f);
            }
        }
    }


}
