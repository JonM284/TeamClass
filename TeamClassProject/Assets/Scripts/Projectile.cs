using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool moveRight = true;
    public float speed;

    [Header("Attack Attributes")]
    public float damage;
    public float angle;
    public float knockback;
    public float hitStun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVariables(float damage1, float angle1, float knockback1, float hitStun1, float speed1, bool direction)
    {
        damage = damage1;
        angle = angle1;
        knockback = knockback1;
        hitStun = hitStun1;
        speed = speed1;
        moveRight = direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveRight)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayer>().GetHit(damage, angle, knockback, hitStun, moveRight);
        }
        Destroy(gameObject);
    }
}
