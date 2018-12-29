using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /* please put your name at the end of important comments so that we know who wrote what, so that if one of us has a problem understanding
     * what the code does, we can directly ask the person, instead of asking everyone
     * also im putting this block comment at the top of every script, sorry if it gets repetitive
     * -Ganderman Dan
     */

    /* this script should be where all the movement related stuff should go. 
     * I think we should also put knockback related stuff here since that has to do with movement. 
     *            (ex. angle you fly when you get hit, and how far you fly back when you get hit, etc.)
     * But if you'd rather have a seperate script for that, let me know  -Ganderman Dan 
     */

    [Header("Movement Modifiers")]
    public float gravity;
    public float walkSpeed, airSpeed, runSpeed; //I dont know if running is in the game, but i put the variable there just in case -Ganderman Dan 
    public float numJumps, jumpForce;
    public float weight;  
    public bool canWallCling;
    [HideInInspector]
    public float hitStun; 


    void Start()
    {
        
    }

   /* if you're doing something with the rigidbody and movement, do it here, it's better
    * check here for reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
    * more about the differnces between the two update functions https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
    * -Ganderman Dan
    */
    void FixedUpdate()
    {
        
    }

    //anything that doesn't have to do with rigidbody movement, do here
    void Update()
    {

    }


    /* This function gets called when an enemy hits you
     * What the arguments are for:
     *      attackDamage- is the how much the players health/armor goes down.
     *      attackAngle- is the angle you get sent flying when you get hit. [*possibly* affected by player weight]
     *      attackForce- is how far back you get sent flying. [affected by player weight]
     *      hitStun- is how long the player has to wait before they can do anything
     *      -Ganderman Dan
     */ 
    public void GetHit(float attackDamage, float attackAngle, float attackForce, float hitStun)//im probably missing a few arguments
    {
        
    }
}
