using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnomercy : MonoBehaviour
{
    [Header("Gnomercy Values")]
    public float maxHealth;
    public int speed;
    public float weight;
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float maxDownVel;
	private int playerNumber;

	[Header("Basic Attacks")]
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

    [Header("Air Attacks")]
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
    public GameObject spawnIceShotHere1;
	public float bulletSpeed1;

	[Header("Forward Air")]
	public float FA_Damage;
	public float FA_Angle;
	public float FA_Knockback;
	public float FA_HitStun;
	public float FA_Distance;
	public float FA_TravelTime;
	public float FA_ShakeDuration;
	public float FA_ShakeMagnitude;
	public float FA_ShakeSlowDown;

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

    [Header("Forward Heavy")]
	public float FH_Damage;
	public float FH_Angle;
	public float FH_Knockback;
	public float FH_HitStun;
	public float FH_Distance;
	public float FH_TravelTime;
    public float FH_ShakeDuration;
    public float FH_ShakeMagnitude;
    public float FH_ShakeSlowDown;

    [Header("Down Heavy")]
	public float DH_Damage;
	public float DH_Angle;
	public float DH_Knockback;
	public float DH_HitStun;
	public float DH_Distance;
	public float DH_TravelTime;
    public float DH_ShakeDuration;
    public float DH_ShakeMagnitude;
    public float DH_ShakeSlowDown;

    [Header("Up Heavy")]
	public float UH_Damage;
	public float UH_Angle;
	public float UH_Knockback;
	public float UH_HitStun;
	public float UH_Distance;
	public float UH_TravelTime;
    public float UH_ShakeDuration;
    public float UH_ShakeMagnitude;
    public float UH_ShakeSlowDown;

    BasicPlayerScript player;

	private float currentAttack;

    [Header("Audio")]
    public AudioClip[] GnomercySounds;
    public AudioSource GnomercySoundPlayer;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
		player = this.GetComponent<BasicPlayerScript>();
		playerNumber = GetComponent<BasicPlayerScript>().playerNum;

        //setting AudioSource
        GnomercySoundPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void GnomercyAttackController(int attackNum)
	{
		switch (attackNum)
		{
			case 1:
				player.anim.SetTrigger("BasicNeutral");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

			case 2:
				player.anim.SetTrigger("BasicForward");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

                GnomercySoundPlayer.clip = GnomercySounds[0];
                GnomercySoundPlayer.Play();

                break;

			case 3:
				player.anim.SetTrigger("BasicUp");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

			case 4:
				player.anim.SetTrigger("BasicDown");
				player.isAttacking = true;
				player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
				break;

			case 9:
				player.anim.SetTrigger("NeutralAir");
				player.isAttacking = true;
				break;

			case 10:
				player.anim.SetTrigger("UpAir");
				player.isAttacking = true;
				break;

			case 14:
				player.anim.SetTrigger("ForwardAir");
				player.isAttacking = true;
				break;

			case 20:
				player.anim.SetTrigger("HeavyNeutral");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

			case 21:
				player.anim.SetTrigger("HeavyForward");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

			case 22:
				player.anim.SetTrigger("HeavyDown");
				player.isAttacking = true;
                player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

			case 23:
				player.anim.SetTrigger("HeavyUp");
				player.isAttacking = true;
				player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
				break;


			default:
				break;
		}
	}

	private void NeutralBasic(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(BN_Damage, BN_Angle, BN_Knockback, BN_HitStun, BN_Distance, BN_TravelTime, player.FacingRight(), BN_ShakeDuration, BN_ShakeMagnitude, BN_ShakeSlowDown);
	}

	private void ForwardBasic()
	{
		GameObject bullet = Instantiate(iceShot, spawnIceShotHere.transform.position, Quaternion.identity);
		bullet.GetComponent<Projectile>().SetVariables(BF_Damage, BF_Angle, BF_Knockback, BF_HitStun, BF_Distance, BF_TravelTime, bulletSpeed, playerNumber, BF_ShakeDuration, BF_ShakeMagnitude, BF_ShakeSlowDown);
		bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
		if (player.FacingRight())
		{
			bullet.GetComponent<Projectile>().direction = new Vector3(-1, 0, 0);
		}
		else
		{
			bullet.GetComponent<Projectile>().direction = new Vector3(1, 0, 0);
		}
	}

	private void UpBasic(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(BU_Damage, BU_Angle, BU_Knockback, BU_HitStun, BU_Distance, BU_TravelTime, player.FacingRight(), BU_ShakeDuration, BU_ShakeMagnitude, BU_ShakeSlowDown);
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
		GameObject bullet = Instantiate(iceShot, spawnIceShotHere1.transform.position, Quaternion.identity);
		bullet.GetComponent<Projectile>().SetVariables(UA_Damage, UA_Angle, UA_Knockback, UA_HitStun, UA_Distance, UA_TravelTime, bulletSpeed1, playerNumber, UA_ShakeDuration, UA_ShakeMagnitude, UA_ShakeSlowDown);
		bullet.GetComponent<Projectile>().direction = new Vector3(0, 1, 0);
		bullet.GetComponent<Projectile>().moveRight = player.FacingRight();
	}

	private void ForwardAir(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(FA_Damage, FA_Angle, FA_Knockback, FA_HitStun, FA_Distance, FA_TravelTime, player.FacingRight(), FA_ShakeDuration, FA_ShakeMagnitude, FA_ShakeSlowDown);
	}

	private void NeutralHeavy(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(NH_Damage, NH_Angle, NH_Knockback, NH_HitStun, NH_Distance, NH_TravelTime, player.FacingRight(), NH_ShakeDuration, NH_ShakeMagnitude, NH_ShakeSlowDown);
	}

	private void ForwardHeavy(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(FH_Damage, FH_Angle, FH_Knockback, FH_HitStun, FH_Distance, FH_TravelTime, player.FacingRight(), FH_ShakeDuration, FH_ShakeMagnitude, FH_ShakeSlowDown);
	}

	private void DownHeavy(GameObject enemy)
	{
		//do gnomercy's dirt projectile
		//enemy.GetComponent<BasicPlayerScript>().GetHit(DH1_Damage, DH1_Angle, DH1_Knockback, DH1_HitStun, DH1_Distance, DH1_TravelTime, player.FacingRight(), DH1_ShakeDuration, DH1_ShakeMagnitude, DH1_ShakeSlowDown);
	}

	private void UpHeavy(GameObject enemy)
	{
		enemy.GetComponent<BasicPlayerScript>().GetHit(UH_Damage, UH_Angle, UH_Knockback, UH_HitStun, UH_Distance, UH_TravelTime, player.FacingRight(), UH_ShakeDuration, UH_ShakeMagnitude, UH_ShakeSlowDown);
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
     * 9 = neutral air
     * 
	 * 14 = forward air
     * 
     * 20 = neutral heavy
     * 21 = forward heavy
     * 22 = down heavy
     * 23 = Up Heavy
     * 
     */
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			try
			{
				if (other.transform.root.gameObject.GetComponent<BasicPlayerScript>().playerNum != playerNumber)
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

						case 14:
							ForwardAir(other.gameObject);
							break;

						case 20:
							NeutralHeavy(other.gameObject);
							break;

						case 21:
							ForwardHeavy(other.gameObject);
							break;

						case 23:
							UpHeavy(other.gameObject);
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
