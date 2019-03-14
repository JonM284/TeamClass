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


    [Header("Basic Forward")]
    public float BF_Damage;
    public float BF_Angle;
    public float BF_Knockback;
    public float BF_HitStun;
    public float BF_Distance;
    public float BF_TravelTime;

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
  

    private float currentAttack;

    BasicPlayerScript player;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BasicPlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void NeutralBasic(GameObject enemy)
    {
        
    }

    private void ForwardBasic(GameObject enemy)
    {
        enemy.GetComponent<BasicPlayerScript>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, BN_Distance, BN_TravelTime, player.FacingRight());
    }

    private void UpBasic(GameObject enemy)
    {
        
    }

    private void DownBasic(GameObject enemy)
    {
        
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
