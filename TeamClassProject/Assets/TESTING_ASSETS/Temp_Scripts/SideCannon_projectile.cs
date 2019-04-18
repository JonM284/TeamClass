using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCannon_projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 vel;
    public float maxTimer;
    private float timer, original_Speed;
    private Vector3 Floor;
    public bool move_Right;
    public ParticleSystem explosion_Particles, trail_Particles;

    [Header("Audio")]
    public AudioClip[] CannonSounds;
    public AudioSource CannonSoundPlayer;

    private void Awake()
    {
        original_Speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;
        
        rb = GetComponent<Rigidbody2D>();
        Floor = new Vector3(0,-3,0);

        //Cannon Explosion
        CannonSoundPlayer.clip = CannonSounds[0];
        CannonSoundPlayer.Play();
    }

    private void OnEnable()
    {
        timer = maxTimer;
        speed = original_Speed;
        if (gameObject.GetComponent<SpriteRenderer>() != null) {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }else
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
        trail_Particles.Play();
        explosion_Particles.Stop();
        explosion_Particles.gameObject.SetActive(false);
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
        if (move_Right) {
            rb.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
        }else
        {
            rb.MovePosition(transform.position - transform.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Has touched something");
        if(other.gameObject.tag != "Level2EndPos")
        {
            StartCoroutine(wait_To_Deactivate());
            float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
            if (other.gameObject.tag == "Player")
            {
                if (move_Right)
                    other.gameObject.GetComponent<BasicPlayerScript>().GetHit(150f, 45, 12, 0.2f, 100, .2f, false, 0.1f, 0.3f, 0.2f);

                if (!move_Right)
                    other.gameObject.GetComponent<BasicPlayerScript>().GetHit(150f, 45, 12, 0.2f, 100, .2f, true, 0.1f, 0.3f, 0.2f);
            }
        }

    }


    public IEnumerator wait_To_Deactivate()
    {
        speed = 0;
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        explosion_Particles.gameObject.SetActive(true);
        explosion_Particles.Play();
        trail_Particles.Stop();


        //Cannon Explosion
        CannonSoundPlayer.clip = CannonSounds[1];
        CannonSoundPlayer.Play();

        yield return new WaitForSeconds(0.5f);
        explosion_Particles.Stop();
        explosion_Particles.gameObject.SetActive(false);
        this.gameObject.SetActive(false);


    }
}
