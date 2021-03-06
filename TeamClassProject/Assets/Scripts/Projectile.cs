﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool moveRight = true;
    public float speed;

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

    public BasePlayer player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVariables(float damage1, Vector2 angle1, float knockback1, float hitStun1, float distance1, float travelTime1, float speed1, int playerNum1, float duration, float magnitude, float slowDown)
    {
        damage = damage1;
        angle = angle1;
        knockback = knockback1;
        hitStun = hitStun1;
        distance = distance1;
        travelTime = travelTime1;
        speed = speed1;
        playerNum = playerNum1;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeSlowDown = slowDown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.up = direction;

        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            try
            {
                if (other.transform.root.gameObject.GetComponent<BasePlayer>().playerNum != playerNum)
                {
                    other.gameObject.GetComponent<BasePlayer>().GetHit(damage, angle, knockback, hitStun, moveRight, shakeDuration, shakeMagnitude, shakeSlowDown);
                    player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(damage);
                    Debug.Log("pn" + other.transform.root.gameObject.GetComponent<BasePlayer>().playerNum);
                    Debug.Log("pnm1 " + playerNum);
                    Destroy(gameObject);
                }
            }
            catch
            {

            }
        }

        if (other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
        
    }
}
