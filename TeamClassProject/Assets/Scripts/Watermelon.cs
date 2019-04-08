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
    private int playerNumber;

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

    [Header("Down Air")]
    public float DA_Damage;
    public float DA_Angle;
    public float DA_Knockback;
    public float DA_HitStun;
    public float DA_Distance;
    public float DA_TravelTime;
    public float DA_ShakeDuration;
    public float DA_ShakeMagnitude;
    public float DA_ShakeSlowDown;
    public GameObject melonDown;
    public GameObject spawnMelonJDHere;
    public float melDSpeed;


    private float currentAttack;
    private int yDir;
    private bool latched, vining, goUp;
    private int vineUpCou;


    Rigidbody2D rb2D;
    BasicPlayerScript player;
    // public Renderer vineWhip;
    //public GameObject downVine;

    [Header("Neutral Heavy")]
    public float NH_Damage;
    public float NH_Angle;
    public float NH_Knockback;
    public float NH_HitStun;
    public float NH_Distance;
    public float NH_TravelTime;
    public float NH_ShakeDuration;
    public float NH_ShakeMagnitude;
    public float NH_ShakeSlowDown;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
       // downVine.gameObject.SetActive(false);
        player = GetComponent<BasicPlayerScript>();
        playerNumber = GetComponent<BasicPlayerScript>().playerNum;
        rb2D = GetComponent<Rigidbody2D>();
        yDir = 1;
        vining = false;
        latched = false;
        goUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
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
                    player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                }
                break;

            case 9:
                player.anim.SetTrigger("NeutralAir");
                player.isAttacking = true;
                //if (player.wawa)
                //{
               //     player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

              //  }
                break;

            case 11:
                player.anim.SetTrigger("DownAir");
                player.isAttacking = true;
                //if (player.wawa)
              //  {
                //   player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

               // }
                break;

            case 12:
                player.anim.SetTrigger("BackAir");
                player.isAttacking = true;
                //if (player.wawa)
                //{
                 //   player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //}
                break;

            case 20:
                player.anim.SetTrigger("HeavyNeutral");
                player.isAttacking = true;
                if (player.wawa)
                {
                    player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                }
                break;




        }
    }


    private void vineDNHappenings()
    {
        if (vining == true)
        {
            //downVine.gameObject.SetActive(true);
            if (goUp == true)
            {
                //downVine.transform.position = new Vector2(downVine.transform.position.x, downVine.transform.position.y + .0004f);
                vineUpCou++;
                if (vineUpCou > 60)
                {
                    goUp = false;
                }
            }
            else
            {
                //downVine.transform.position = new Vector2(downVine.transform.position.x, downVine.transform.position.y - .0004f);
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
            //downVine.gameObject.SetActive(false);
        }







    }

    private void NeutralBasic(GameObject enemy)
    {
        print("NB");
    }

    private void ForwardBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayerScript>().GetHit(BF_Damage, BF_Angle, BF_Knockback, BF_HitStun, BF_Distance, BF_TravelTime, player.FacingRight(), BF_ShakeDuration, BF_ShakeMagnitude, BF_ShakeSlowDown);
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
        enemy.GetComponent<BasicPlayerScript>().GetHit(NA_Damage, NA_Angle, NA_Knockback, NA_HitStun, NA_Distance, NA_TravelTime, player.FacingRight(), NA_ShakeDuration, NA_ShakeMagnitude, NA_ShakeSlowDown);
    }

    private void UpAir()
    {

    }

    private void DownAir()
    {
        Debug.Log("hi");
        GameObject melonDownJ = Instantiate(melonDown, spawnMelonJDHere.transform.position, Quaternion.identity);
        melonDownJ.GetComponent<Projectile>().SetVariables(DA_Damage, DA_Angle, DA_Knockback, DA_HitStun, DA_Distance, DA_TravelTime, melDSpeed, playerNumber, DA_ShakeDuration, DA_ShakeMagnitude, DA_ShakeSlowDown);
        melonDownJ.GetComponent<Projectile>().direction = new Vector3(0, -1, 0);
        melonDownJ.GetComponent<Projectile>().moveRight = player.FacingRight();
    }

    private void NeutralHeavy(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayerScript>().GetHit(NH_Damage, NH_Angle, NH_Knockback, NH_HitStun, NH_Distance, NH_TravelTime, player.FacingRight(), NH_ShakeDuration, NH_ShakeMagnitude, NH_ShakeSlowDown);
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

                case 11:
                    DownAir();
                    break;

                case 20:
                    NeutralHeavy(other.gameObject);
                    break;
            }
        }
    }
}
