using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCannon_projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 vel;
    public float maxTimer;
    private float timer;
    private Vector3 Floor;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;
        rb = GetComponent<Rigidbody2D>();
        Floor = new Vector3(0,-3,0);
    }

    private void OnEnable()
    {
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Projectile_Move();
    }

    void Projectile_Move()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector2 current_Target = Floor - transform.position;
            Quaternion.Lerp(transform.rotation, Quaternion.Euler(current_Target), Time.deltaTime * speed);
            timer = -1;
        }

        rb.MovePosition(transform.position + transform.right * speed * Time.deltaTime); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.gameObject.SetActive(false);
    }
}
