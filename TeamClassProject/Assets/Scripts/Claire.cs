﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire : MonoBehaviour
{
    [Header("Claire Values")]
    public float maxHealth;
    public int speed;
    public int ultSpeed;
    public int regularSpeed;
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
    public GameObject iceShot;
    public GameObject spawnIceShotHere; 
    public float bulletSpeed;

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

    [Header("Basic Down")]
    public float BD_Damage;
    public Vector2 BD_Angle;
    public float BD_Knockback;
    public float BD_HitStun;
    public float BD_Distance;
    public float BD_TravelTime;
    public float BD_ShakeDuration;
    public float BD_ShakeMagnitude;
    public float BD_ShakeSlowDown;

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

    [Header("Up Heavy Part 1")]
    public float UH1_Damage;
    public Vector2 UH1_Angle;
    public float UH1_Knockback;
    public float UH1_HitStun;
    public float UH1_Distance;
    public float UH1_TravelTime;
    public float UH1_ShakeDuration;
    public float UH1_ShakeMagnitude;
    public float UH1_ShakeSlowDown;

    [Header("Up Heavy Part 2")]
    public float UH2_Damage;
    public Vector2 UH2_Angle;
    public float UH2_Knockback;
    public float UH2_HitStun;
    public float UH2_Distance;
    public float UH2_TravelTime;
    public float UH2_ShakeDuration;
    public float UH2_ShakeMagnitude;
    public float UH2_ShakeSlowDown;

    [Header("Down Heavy")]
    public float DH_Damage;
    public Vector2 DH_Angle;
    public float DH_Knockback;
    public float DH_HitStun;
    public float DH_Distance;
    public float DH_TravelTime;
    public float DH_ShakeDuration;
    public float DH_ShakeMagnitude;
    public float DH_ShakeSlowDown;
    public bool shield;

    [Header("Ult Attack")]
    public GameObject storm;
    public bool hasStorm;
    public float ultDuration;
    public float ultDamage;

    private float ultAttackTime = 0;
    public bool ultActive = false;

    private float currentAttack;

    BasePlayer player;

    [Header("Audio")]
    public AudioClip[] ClaireSounds;
    public AudioSource ClaireSoundPlayer;


    private void Awake()
    {
         
    }

    // Start is called before the first frame update
    void Start()
    {
		player = this.GetComponent<BasePlayer>();
        playerNumber = player.playerNum;

        //setting AudioSource
        ClaireSoundPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //ultAttackTime -= Time.deltaTime;

        if (ultActive)
        {
            if (!hasStorm)
            {
                var ult = Instantiate(storm, transform.position, Quaternion.identity);
                ult.GetComponent<ClaireUlt>().duration = ultDuration;
                ult.GetComponent<ClaireUlt>().duration = ultDuration;
                ult.GetComponent<ClaireUlt>().thisPlayer = this.gameObject;
                hasStorm = true;
            }
        }

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
   * 11 = down aerial
   * 
   * 20 = neutral heavy
   * 21 = forward heavy
   * 22 = down heavy 
   * 23 = up heavy
   */
   
    public void ClaireAttackController(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                //neutral basic
                ClaireSoundPlayer.clip = ClaireSounds[0];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 2:
                //forward basic
                ClaireSoundPlayer.clip = ClaireSounds[1];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 3:
                //up basic
                ClaireSoundPlayer.clip = ClaireSounds[2];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

			case 4:
                //down basic
                player.anim.SetBool("Slide", true);

                break;

            case 9:

                ClaireSoundPlayer.clip = ClaireSounds[0];
                ClaireSoundPlayer.Play();

                break;

            case 10:

                ClaireSoundPlayer.clip = ClaireSounds[4];
                ClaireSoundPlayer.Play();

                break;

			case 11:

                player.anim.SetBool("DownPunch", true);
				break;

			case 12:

				break;

			case 20:
                //neutral heavy
                ClaireSoundPlayer.clip = ClaireSounds[5];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;

            case 21:
                //forward heavy
                ClaireSoundPlayer.clip = ClaireSounds[6];
                ClaireSoundPlayer.Play();

                break;

            case 22:
                //up heavy
                ClaireSoundPlayer.clip = ClaireSounds[8];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;
            case 23:
                //down heavy
                shield = true;

                ClaireSoundPlayer.clip = ClaireSounds[7];
                ClaireSoundPlayer.Play();

                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                break;


            case 69:
                //player.isAttacking = true;
                ultActive = true;
                break;

            default:
                break;
        }
    }
    
	
    
    private void NeutralBasic(GameObject enemy)
    {
		enemy.GetComponent<BasePlayer>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, player.FacingRight(), BN_ShakeDuration, BN_ShakeMagnitude, BN_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(BN_Damage);
    }

    private void ForwardBasic()
    {
        GameObject bullet = Instantiate(iceShot, spawnIceShotHere.transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetVariables(BF_Damage, BF_Angle, BF_Knockback, BF_HitStun, BF_Distance, BF_TravelTime, bulletSpeed, playerNumber, BF_ShakeDuration, BF_ShakeMagnitude, BF_ShakeSlowDown);
        bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
        bullet.GetComponent<Projectile>().player = player;
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
        enemy.GetComponent<BasePlayer>().GetHit(BU_Damage, BU_Angle, BU_Knockback, BU_HitStun, player.FacingRight(), BU_ShakeDuration, BU_ShakeMagnitude, BU_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(BU_Damage);
    }

    private void DownBasic(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(BD_Damage, BD_Angle, BD_Knockback, BD_HitStun, player.FacingRight(), BD_ShakeDuration, BD_ShakeMagnitude, BD_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(BD_Damage);
    }

    private void NeutralAir(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(NA_Damage, NA_Angle, NA_Knockback, NA_HitStun, player.FacingRight(), NA_ShakeDuration, NA_ShakeMagnitude, NA_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(NA_Damage);
    }

    private void UpAir()
    {
        GameObject bullet = Instantiate(iceShot, spawnIceShotHere1.transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetVariables(UA_Damage, UA_Angle, UA_Knockback, UA_HitStun, UA_Distance, UA_TravelTime, bulletSpeed1, playerNumber, UA_ShakeDuration, UA_ShakeMagnitude, UA_ShakeSlowDown);
        bullet.GetComponent<Projectile>().direction = new Vector3(0, 1, 0);
        bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
        bullet.GetComponent<Projectile>().player = player;
    }

	private void DownAir(GameObject enemy)
	{
		enemy.GetComponent<BasePlayer>().GetHit(DA_Damage, DA_Angle, DA_Knockback, DA_HitStun, player.FacingRight(), DA_ShakeDuration, DA_ShakeMagnitude, DA_ShakeSlowDown);
		player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(DA_Damage);
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
        
    }

    private void UpHeavyPart1(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(UH1_Damage, UH1_Angle, UH1_Knockback, UH1_HitStun, player.FacingRight(), UH1_ShakeDuration, UH1_ShakeMagnitude, UH1_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(UH1_Damage);
    }

    private void UpHeavyPart2(GameObject enemy)
    {
        enemy.GetComponent<BasePlayer>().GetHit(UH2_Damage, UH2_Angle, UH2_Knockback, UH2_HitStun, player.FacingRight(), UH2_ShakeDuration, UH2_ShakeMagnitude, UH2_ShakeSlowDown);
        player.teamController.GetComponent<SwitchHandler>().UpdateUltBar(UH2_Damage);
    }

    private void UltAttack()
    {
        

        
    }
    
    public void CurrentAttack(int attackNum)
    {
        currentAttack = attackNum;
    }

    public void EndAttack()
    {
        player.ResetTriggers();
        currentAttack = 0;
        player.isAttacking = false;
        if (shield)
        {
            shield = false;
        }
        if (player.anim.GetBool("Slide"))
        {
            player.anim.SetBool("Slide", false);
        }
        if (player.anim.GetBool("DownPunch"))
        {
            player.anim.SetBool("DownPunch", false);
        }
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	}

    public void FistLand()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
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
     * 
     * 20 = neutral heavy
     * 21 = forward heavy
     * 22 = down heavy 
     * 23 = up heavy part 1
     * 24 = up heavy part 2
     * 
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            try
            {
                if (other.transform.root.gameObject.GetComponent<BasePlayer>().playerNum != playerNumber)
                {
                    if(other.GetComponent<BasePlayer>().player != BasePlayer.playerState.Knockback)
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

						case 12:
							DownAir(other.gameObject);
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

                        case 23:
                            UpHeavyPart1(other.gameObject);
                            break;
                        case 24:
                            UpHeavyPart2(other.gameObject);
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
