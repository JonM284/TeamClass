using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class BasicAttack : MonoBehaviour
{
    /* please put your name at the end of important comments so that we know who wrote what, so that if one of us has a problem understanding
     * what the code does, we can directly ask the person, instead of asking everyone
     * also im putting this block comment at the top of every script, sorry if it gets repetitive
     * -Ganderman Dan 🦆
     */

    /* this script should be where all the Basic Attack stuff is handled 
     * When an enemy is hit, they call one of these functions so that the attack properties can be enforced
     * 
     * But perhaps a separate script could make thinks more organized
     *   -Ganderman Dan  🦆
     */

    //If you make a new variable or script, please explain what it does in their respective reference sheets, just so every understands what each variable is used for -Ganderman Dan 🦆



    //these variables wont be put in the reference sheets because it should be super obvious what they do, and the content of what they do woulb be repeated 100 times
    [Header("Grounded Forward Attack Attributes")]
    public BoxCollider2D FA_Hitbox;
    public float FA_Damage;
    public float FA_Angle;
    public float FA_Knockback;
    public float FA_HitStun;

    private Animator anim;
    private Movement movementScript;

    //rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    void Awake()
    {
        if (playerNum <= 0 || playerNum >= 5)
        {
            Debug.LogError("Player Num MUST be greater than 0 and less than 5");
        }
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        movementScript = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GroundedForwardAttack(GameObject enemy)
    {
        FA_Hitbox.enabled = false; //I turned it of so the enemy only gets hit once
            enemy.GetComponent<Movement>().GetHit(FA_Damage, FA_Angle, FA_Knockback, FA_HitStun, movementScript.FacingRight());
    }

    public void GroundedDownAttack()
    {

    }

    public void GroundedUpAttack()
    {

    }

    public void AerialForwardAttack()
    {

    }

    public void AerialDownAttack()
    {

    }

    public void AerialUpAttack()
    {

    }
}
