﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gillbert : MonoBehaviour
{
    [Header("Gilbert Values")]
    public float maxHealth;
    public int speed;
    [Tooltip("Only change this by decimal point intervals, 1 is default. I.E. .7 or 1.4")]
    public float weight;
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float maxDownVel;
    private int playerNumber;

    [Header("Basic Attacks")]
    [Header("Basic Neutral")]
    public float BN_Damage;
    public Vector2 BN_Angle;
    public float BN_Knockback;
    public float BN_HitStun;
    public float BN_Distance;
    public float BN_TravelTime;
    public float BN_ShakeDuration;
    public float BN_ShakeMagnitude;
    public float BN_ShakeSlowDown;


    [Header("Basic Forward")]
    public float BF_Damage;
    public Vector2 BF_Angle;
    public float BF_Knockback;
    public float BF_HitStun;
    public float BF_Distance;
    public float BF_TravelTime;
    public float BF_ShakeDuration;
    public float BF_ShakeMagnitude;
    public float BF_ShakeSlowDown;


    [Header("Basic Up")]
    public float BU_Damage;
    public Vector2 BU_Angle;
    public float BU_Knockback;
    public float BU_HitStun;
    public float BU_Distance;
    public float BU_TravelTime;
    public float BU_ShakeDuration;
    public float BU_ShakeMagnitude;
    public float BU_ShakeSlowDown;
    public GameObject spitBall;
    public GameObject spawnSpitHere;
    public float BU_MaxDownVel;
    public float BU_GravityUp;
    public float BU_GravityDown;
    public float BU_XSpeed;
    public float BU_YSpeed;
    public float BU_MoveUpTimer;

    [Header("Basic Down")]
    public float BD_Damage;
    public float BD_StunTime;

      [Header("Air Attacks")]
    [Header("Neutral Air")]
    public float NA_Damage;
    public Vector2 NA_Angle;
    public float NA_Knockback;
    public float NA_HitStun;
    public float NA_Distance;
    public float NA_TravelTime;
    public float NA_ShakeDuration;
    public float NA_ShakeMagnitude;
    public float NA_ShakeSlowDown;

    [Header("Forward Air")]
    public float FA_Damage;
    public Vector2 FA_Angle;
    public float FA_Knockback;
    public float FA_HitStun;
    public float FA_Distance;
    public float FA_TravelTime;
    public float FA_ShakeDuration;
    public float FA_ShakeMagnitude;
    public float FA_ShakeSlowDown;

    [Header("Up Air")]
    public float UA_Damage;
    public Vector2 UA_Angle;
    public float UA_Knockback;
    public float UA_HitStun;
    public float UA_Distance;
    public float UA_TravelTime;
    public float UA_ShakeDuration;
    public float UA_ShakeMagnitude;
    public float UA_ShakeSlowDown;
    public GameObject spawnIceShotHere1;
    public float bulletSpeed1;

    [Header("Down Air")]
    public float DA_Damage;
    public Vector2 DA_Angle;
    public float DA_Knockback;
    public float DA_HitStun;
    public float DA_Distance;
    public float DA_TravelTime;
    public float DA_ShakeDuration;
    public float DA_ShakeMagnitude;
    public float DA_ShakeSlowDown;
    public GameObject spitBall1;
    public GameObject spawnSpitHere1;
    public float DA_MaxDownVel;
    public float DA_GravityUp;
    public float DA_GravityDown;
    public float DA_XSpeed;
    public float DA_YSpeed;
    public float DA_MoveUpTimer;

    [Header("Neutral Heavy")]
    public float NH_Damage;
    public Vector2 NH_Angle;
    public float NH_Knockback;
    public float NH_HitStun;
    public float NH_Distance;
    public float NH_TravelTime;
    public float NH_ShakeDuration;
    public float NH_ShakeMagnitude;
    public float NH_ShakeSlowDown;

    [Header("Forward Heavy")]
    public float FH_Damage;
    public Vector2 FH_Angle;
    public float FH_Knockback;
    public float FH_HitStun;
    public float FH_Distance;
    public float FH_TravelTime;
    public float FH_ShakeDuration;
    public float FH_ShakeMagnitude;
    public float FH_ShakeSlowDown;

    [Header("Down Heavy")]
    public float DH1_Damage;
    public Vector2 DH1_Angle;
    public float DH1_Knockback;
    public float DH1_HitStun;
    public float DH1_Distance;
    public float DH1_TravelTime;
    public float DH1_ShakeDuration;
    public float DH1_ShakeMagnitude;
    public float DH1_ShakeSlowDown;

    [Header("Ult Attack")]
    public float U_Damage;
    public Vector2 U_Angle;
    public float U_Knockback;
    public float U_HitStun;
    public float U_Distance;
    public float U_TravelTime;
    public float U_ShakeDuration;
    public float U_ShakeMagnitude;
    public float U_ShakeSlowDown;
    public GameObject fireball;
    public float U_MaxDownVel;
    public float U_GravityUp;
    public float U_GravityDown;
    public float U_XSpeed;
    public float U_YSpeed;
    public float U_MoveUpTimer;


    private float ultAttackTime = 0;
    private bool ultActive = false;

    private float currentAttack;

    BasePlayer player;


    [Header("Audio")]
    public AudioClip[] GilbertSounds;
    public AudioSource GilbertSoundPlayer;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<BasePlayer>();
        playerNumber = GetComponent<BasePlayer>().playerNum;

        //setting AudioSource
        GilbertSoundPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        ultAttackTime -= Time.deltaTime;
        if (ultActive)
        {
            player.currentGilbertFlightTime -= Time.deltaTime;
        }

        if (ultActive)
        {
            if(ultAttackTime < 0)
            {
                UltAttack();
                ultAttackTime = 1;
            }
        }
        
        if(player.currentGilbertFlightTime < 0)
        {
            if(ultActive == true)
            {
                player.isAttacking = false;
            }
            ultActive = false;
        }
        */
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
   * 10 = up aerial
   * 11 = forward aerial
   * 12 = down aerial
   * 
   * 
   * 20 = neutral heavy
   * 21 = forward heavy
   * 22 = down heavy 
   * 
   * 69 = ult
   * 
   */
    public void GilbertAttackController(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                //basic neutral
                GilbertSoundPlayer.clip = GilbertSounds[0];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 2:
                //basic forward
                GilbertSoundPlayer.clip = GilbertSounds[0];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 3:
                //basic up
                GilbertSoundPlayer.clip = GilbertSounds[1];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 4:
                //basic down
                GilbertSoundPlayer.clip = GilbertSounds[2];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 9:
                //neutral air
                GilbertSoundPlayer.clip = GilbertSounds[0];
                GilbertSoundPlayer.Play();

                break;

            case 10:
                //up air
                GilbertSoundPlayer.clip = GilbertSounds[3];
                GilbertSoundPlayer.Play();

                break;

            case 11:
                //forward air
                GilbertSoundPlayer.clip = GilbertSounds[3];
                GilbertSoundPlayer.Play();

                break;

            case 12:
                //down air
                GilbertSoundPlayer.clip = GilbertSounds[4];
                GilbertSoundPlayer.Play();

                break;

            case 20:
                //heavy neutral
                GilbertSoundPlayer.clip = GilbertSounds[5];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 21:
                //heavy forward
                GilbertSoundPlayer.clip = GilbertSounds[6];
                GilbertSoundPlayer.Play();

                break;

            case 22:
                //heavy down
                GilbertSoundPlayer.clip = GilbertSounds[7];
                GilbertSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 69:
                player.isAttacking = true;
                ultActive = true;
                player.currentGilbertFlightTime = 10;

                break;


            default:
                break;
        }
    }


    
    private void NeutralBasic(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, player.FacingRight(), BN_ShakeDuration, BN_ShakeMagnitude, BN_ShakeSlowDown);
    }

    private void ForwardBasic(GameObject enemy) 
    {
        enemy.GetComponent<BasePlayer>().GetHit(BF_Damage, BF_Angle, BF_Knockback, BF_HitStun, player.FacingRight(), BF_ShakeDuration, BF_ShakeMagnitude, BF_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(BF_Damage);
    }

    private void UpBasic(GameObject enemy)
    {
        GameObject spit = Instantiate(spitBall, spawnSpitHere.transform.position, Quaternion.identity);
        spit.GetComponent<SpitProjectile>().SetVariables(BU_Damage, BU_Angle, BU_Knockback, BU_HitStun, BU_Distance, BU_TravelTime, playerNumber, BU_ShakeDuration, BU_ShakeMagnitude, BU_ShakeSlowDown);
        spit.GetComponent<SpitProjectile>().SetPhysicsVariables(BU_MaxDownVel, BU_GravityUp, BU_GravityDown, BU_XSpeed, BU_YSpeed, BU_MoveUpTimer);

        GameObject spit1 = Instantiate(spitBall, spawnSpitHere.transform.position, Quaternion.identity);
        spit1.GetComponent<SpitProjectile>().SetVariables(BU_Damage, BU_Angle, BU_Knockback, BU_HitStun, BU_Distance, BU_TravelTime, playerNumber, BU_ShakeDuration, BU_ShakeMagnitude, BU_ShakeSlowDown);
        spit1.GetComponent<SpitProjectile>().SetPhysicsVariables(BU_MaxDownVel, BU_GravityUp, BU_GravityDown, -BU_XSpeed, BU_YSpeed, BU_MoveUpTimer);

        spit.GetComponent<SpitProjectile>().player = player;
        spit1.GetComponent<SpitProjectile>().player = player;
    }

    private void DownBasic(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().currentHealth -= BD_Damage;
        //need to fix stun
        //enemy.GetComponent<BasePlayer>().stunTime = BD_StunTime;
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(BD_Damage);
    }

    private void NeutralAir(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(NA_Damage, NA_Angle, NA_Knockback, NA_HitStun, player.FacingRight(), NA_ShakeDuration, NA_ShakeMagnitude, NA_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(NA_Damage);
    }

    private void ForwardAir(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(FA_Damage, FA_Angle, FA_Knockback, FA_HitStun, player.FacingRight(), FA_ShakeDuration, FA_ShakeMagnitude, FA_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(FA_Damage);
    }

    private void UpAir(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(UA_Damage, UA_Angle, UA_Knockback, UA_HitStun, player.FacingRight(), UA_ShakeDuration, UA_ShakeMagnitude, UA_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(UA_Damage);
    }

    private void DownAir(GameObject enemy)
    {
        GameObject spit1 = Instantiate(spitBall1, spawnSpitHere1.transform.position, Quaternion.identity);
        spit1.GetComponent<SpitProjectile>().SetVariables(DA_Damage, DA_Angle, DA_Knockback, DA_HitStun, DA_Distance, DA_TravelTime, playerNumber, DA_ShakeDuration, DA_ShakeMagnitude, DA_ShakeSlowDown);
        spit1.GetComponent<SpitProjectile>().SetPhysicsVariables(DA_MaxDownVel, DA_GravityUp, DA_GravityDown, DA_XSpeed, DA_YSpeed, DA_MoveUpTimer);
        spit1.GetComponent<SpitProjectile>().player = player;
    }

    private void NeutralHeavy(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(NH_Damage, NH_Angle, NH_Knockback, NH_HitStun, player.FacingRight(), NH_ShakeDuration, NH_ShakeMagnitude, NH_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(NH_Damage);
    }

    private void ForwardHeavy(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(FH_Damage, FH_Angle, FH_Knockback, FH_HitStun, player.FacingRight(), FH_ShakeDuration, FH_ShakeMagnitude, FH_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(FH_Damage);
    }

    private void DownHeavy(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(DH1_Damage, DH1_Angle, DH1_Knockback, DH1_HitStun, player.FacingRight(), DH1_ShakeDuration, DH1_ShakeMagnitude, DH1_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(DH1_Damage);
    }


    private void UltAttack()
    {
        GameObject fireballs = Instantiate(fireball, spawnSpitHere1.transform.position, Quaternion.identity);
        fireball.GetComponent<SpitProjectile>().SetVariables(U_Damage, U_Angle, U_Knockback, U_HitStun, U_Distance, U_TravelTime, playerNumber, U_ShakeDuration, U_ShakeMagnitude, U_ShakeSlowDown);
        fireball.GetComponent<SpitProjectile>().SetPhysicsVariables(U_MaxDownVel, U_GravityUp, U_GravityDown, U_XSpeed, U_YSpeed, U_MoveUpTimer);
        fireball.GetComponent<SpitProjectile>().player = player;
    }   

    public void CurrentAttack(int attackNum)
    {
        currentAttack = attackNum;
    }

    public void EndAttack()
    {
        currentAttack = 0;
        player.isAttacking = false;
        player.ResetTriggers();
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
     * 11 = forwardAir
     * 
     * 
     * 20 = neutral heavy
     * 21 = forward heavy
     * 22 = down heavy part 1
     * 23 = down heavy part 2
     * 
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            try
            {
                if (other.transform.root.gameObject.GetComponent<BasePlayer>().playerNum != playerNumber)
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
                            ForwardAir(other.gameObject);
                            break;

                        case 20:
                            NeutralHeavy(other.gameObject);
                            break;

                        case 21:
                            ForwardHeavy(other.gameObject);
                            break;

                        case 22:
                            DownHeavy(other.gameObject);
                            break;

                    }
                }
            }
            catch
            {

            }
        }
    }

}
