using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire : MonoBehaviour
{
    [Header("Claire Values")]
    public float maxHealth;
    public int speed;
    public float weight;

    [Header("Basic Attacks")]
    [Header("Basic Neutral")]
    public float BN_Damage;
    public float BN_Angle;
    public float BN_Knockback;
    public float BN_HitStun;
    public float BN_Distance;
    public float BN_TravelTime;


    [Header("Basic Forward")]
    public float BF_Damage;
    public float BF_Angle;
    public float BF_Knockback;
    public float BF_HitStun;
    public float BF_Distance;
    public float BF_TravelTime;
    public GameObject iceShot;
    public GameObject spawnIceShotHere; 
    public float bulletSpeed;

    [Header("Basic Up")]
    public float BU_Damage;
    public float BU_Angle;
    public float BU_Knockback;
    public float BU_HitStun;
    public float BU_Distance;
    public float BU_TravelTime;

    [Header("Basic Down")]
    public float BD_Damage;
    public float BD_Angle;
    public float BD_Knockback;
    public float BD_HitStun;
    public float BD_Distance;
    public float BD_TravelTime;

    [Header("Air Attacks")]
    [Header("Neutral Air")]
    public float NA_Damage;
    public float NA_Angle;
    public float NA_Knockback;
    public float NA_HitStun;
    public float NA_Distance;
    public float NA_TravelTime;

    [Header("Up Air")]
    public float UA_Damage;
    public float UA_Angle;
    public float UA_Knockback;
    public float UA_HitStun;
    public float UA_Distance;
    public float UA_TravelTime;
    public GameObject spawnIceShotHere1;
    public float bulletSpeed1;

    private float currentAttack;

    BasicPlayer player;


    private void Awake()
    {
        if(this.GetComponent<BasicPlayer>() != null)
        {
            player = this.GetComponent<BasicPlayer>();
            player.maxHealth = maxHealth;
            player.speed = speed;
            player.weight = weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(player.anim.GetInteger("State") == 1)
        {
            EndAttack();
        } 
    }

    private void NeutralBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayer>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, BN_Distance, BN_TravelTime, player.FacingRight());
    }

    private void ForwardBasic()
    {
        //Debug.Log("Hi");
        GameObject bullet = Instantiate(iceShot, spawnIceShotHere.transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetVariables(BF_Damage, BF_Angle, BF_Knockback, BF_HitStun, BF_Distance, BF_TravelTime, bulletSpeed);
        bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
        if (player.FacingRight())
        {
            bullet.GetComponent<Projectile>().direction = new Vector3(1, 0, 0);
        }
        else
        {
            bullet.GetComponent<Projectile>().direction = new Vector3(-1, 0, 0);
        }
    }

    private void UpBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayer>().GetHit(BU_Damage, BU_Angle, BU_Knockback, BU_HitStun, BU_Distance, BU_TravelTime, player.FacingRight());
    }

    private void DownBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayer>().GetHit(BD_Damage, BD_Angle, BD_Knockback, BD_HitStun, BD_Distance, BD_TravelTime, player.FacingRight());
    }

    private void NeutralAir(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayer>().GetHit(NA_Damage, NA_Angle, NA_Knockback, NA_HitStun, NA_Distance, NA_TravelTime, player.FacingRight());
    }

    private void UpAir()
    {
        GameObject bullet = Instantiate(iceShot, spawnIceShotHere1.transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetVariables(UA_Damage, UA_Angle, UA_Knockback, UA_HitStun, UA_Distance, UA_TravelTime, bulletSpeed1);
        bullet.GetComponent<Projectile>().direction = new Vector3(0, 1, 0);
        bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
    }

    

    public void CurrentAttack(int attackNum)
    {
        currentAttack = attackNum;
    }

    public void EndAttack()
    {
        currentAttack = 0;
        player.anim.SetInteger("State", 0);
        player.isAttacking = false;
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
        if(other.gameObject.tag == "Player")
        {
            switch(currentAttack)
            {
                case 0:
                    break;

                case 1:
                    NeutralBasic(other.gameObject);
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
