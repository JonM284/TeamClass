using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : MonoBehaviour
{
    [Header("Watermelon Values")]
    public float maxHealth;
    public int speed;
    [Tooltip("Only change this by decimal point intervals, 1 is default. I.E. .7 or 1.4")]
    public float weight;
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float maxDownVel;

    
    [Header("Basic Neutral")]
    public float BN_Damage;
    public float BN_Angle;
    public float BN_Knockback;
    public float BN_HitStun;
    public float BN_Distance;
    public float BN_TravelTime;
    public float BN_ShakeDuration;
    public float BN_ShakeMagnitude;
    public float BN_ShakeSlowDown;


    [Header("Basic Forward")]
    public float BF_Damage;
    public float BF_Angle;
    public float BF_Knockback;
    public float BF_HitStun;
    public float BF_Distance;
    public float BF_TravelTime;
    public float BF_ShakeDuration;
    public float BF_ShakeMagnitude;
    public float BF_ShakeSlowDown;

    [Header("Basic Up")]
    public float BU_Damage;
    public float BU_Angle;
    public float BU_Knockback;
    public float BU_HitStun;
    public float BU_Distance;
    public float BU_TravelTime;
    public float BU_ShakeDuration;
    public float BU_ShakeMagnitude;
    public float BU_ShakeSlowDown;

    [Header("Basic Down")]
    public float BD_Damage;
    public float BD_Angle;
    public float BD_Knockback;
    public float BD_HitStun;
    public float BD_Distance;
    public float BD_TravelTime;
    public float BD_ShakeDuration;
    public float BD_ShakeMagnitude;
    public float BD_ShakeSlowDown;


    [Header("Neutral Air")]
    public float NA_Damage;
    public float NA_Angle;
    public float NA_Knockback;
    public float NA_HitStun;
    public float NA_Distance;
    public float NA_TravelTime;
    public float NA_ShakeDuration;
    public float NA_ShakeMagnitude;
    public float NA_ShakeSlowDown;

    [Header("Up Air")]
    public float UA_Damage;
    public float UA_Angle;
    public float UA_Knockback;
    public float UA_HitStun;
    public float UA_Distance;
    public float UA_TravelTime;
    public float UA_ShakeDuration;
    public float UA_ShakeMagnitude;
    public float UA_ShakeSlowDown;


    private float currentAttack;
    private int yDir;
    private bool latched, vining, goUp;
    private int vineUpCou;


    Rigidbody2D rb2D;
    BasicPlayerScript player;
    public Renderer vineWhip;
    public GameObject downVine;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        downVine.gameObject.SetActive(false);
        player = GetComponent<BasicPlayerScript>();
        rb2D = GetComponent<Rigidbody2D>();
        yDir = 1;
        vining = false;
        latched = false;
        goUp = true;
    }

    // Update is called once per frame
    void Update()
    {

        vineDNHappenings();

        if (latched==true)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionY;
            vineWhip.enabled = true;
        }
        else
        {
            vineWhip.enabled = false;
            rb2D.constraints = RigidbodyConstraints2D.None;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
        Vector2 startPos = transform.position;
        Vector2 rayDir = new Vector3(0, yDir);
        float rayLength = 3;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayLength);

        if (hit.collider == null)
        {
            latched = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            
                
                if (latched==true)
                {
                    latched = false;
                }
                
                else if (hit.collider != null)
                {
                    transform.position = new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y-1.5f);
                    latched = true;
                }
            
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (vining!=true)
            {
                vining = true;
                downVine.gameObject.SetActive(true);
            }
            
            

        }
        
    }

    public void WawaAttackController(int attackNum)
    {
        switch (attackNum)
        {
            case 2:
                player.anim.SetTrigger("BasicForward");
                player.isAttacking = true;
                if (player.wawa)
                {
                    player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                }
                break;

            case 4:
                player.anim.SetTrigger("BasicDown");
                player.isAttacking = true;
                if (player.wawa)
                {
                    print("wawaBDAttk");
                    player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                }
                break;

            

        }
    }


    private void vineDNHappenings()
    {
        if (vining == true)
        {
            downVine.gameObject.SetActive(true);
            if (goUp == true)
            {
                downVine.transform.position = new Vector2(downVine.transform.position.x, downVine.transform.position.y + .0004f);
                vineUpCou++;
                if (vineUpCou > 60)
                {
                    goUp = false;
                }
            }
            else
            {
                downVine.transform.position = new Vector2(downVine.transform.position.x, downVine.transform.position.y - .0004f);
                vineUpCou++;
                if (vineUpCou > 100)
                {
                    goUp = true;

                    vining = false;

                }
            }
        }
        else
        {
            vineUpCou = 0;
            downVine.gameObject.SetActive(false);
        }







    }

    private void NeutralBasic(GameObject enemy)
    {
        print("NB");
    }

    private void ForwardBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayerScript>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, BN_Distance, BN_TravelTime, player.FacingRight(), BN_ShakeDuration, BN_ShakeMagnitude, BN_ShakeSlowDown);
        print("FB");
    }

    private void UpBasic(GameObject enemy)
    {
        
    }

    private void DownBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayerScript>().GetHit(BD_Damage, BD_Angle, BD_Knockback, BD_HitStun, BD_Distance, BD_TravelTime, player.FacingRight(), BD_ShakeDuration, BD_ShakeMagnitude, BD_ShakeSlowDown);
    }

    private void NeutralAir(GameObject enemy)
    {
        
    }

    private void UpAir()
    {

    }



    public void CurrentAttack(int attackNum)
    {
        currentAttack = attackNum;
    }

    public void EndAttack()
    {
        currentAttack = 0;
        player.isAttacking = false;
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    /*
     * attack numbers
     * 0 = null
     * 1 = Basic Neutral
     * 2 = Basic Forward
     * 3 = Basic Up
     * 4 = Basic Down
     * 
     * 9 = neutral aerial
     * 
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (currentAttack)
            {
                case 0:
                    break;

                case 1:
                    NeutralBasic(other.gameObject);
                    break;

                case 2:
                    ForwardBasic(other.gameObject);
                    break;

                case 3:
                    UpBasic(other.gameObject);
                    break;

                case 4:
                    DownBasic(other.gameObject);
                    break;

                case 9:
                    NeutralAir(other.gameObject);
                    break;
            }
        }
    }
}
