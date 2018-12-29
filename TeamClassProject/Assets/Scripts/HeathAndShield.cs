using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathAndShield : MonoBehaviour
{
    /* please put your name at the end of important comments so that we know who wrote what, so that if one of us has a problem understanding
     * what the code does, we can directly ask the person, instead of asking everyone
     * also im putting this block comment at the top of every script, sorry if it gets repetitive
     * -Ganderman Dan
     */

    // this script should be where all the health and shield related stuff goes. This also includes functions that take care of taking damage. -Ganderman Dan    



    [Header("Health and Shield")]
    public float maxHealth;
    private float currentHealth;
    public float maxShield, shieldWaitTime, shieldRegenSpeed; //dont know if we will have shield properties in the game but i put it here just in case -Ganderman Dan
    private float currentShield;
    /*
     * shields, (at least what im thinking, will be a small portion of health that that must fully deplete before the players "health" starts to ...
     * go down. Shields will also have the ability to regenerate after a period of not getting hit.)
     * 
     * shieldWaitTime - is the amount of time you cant take damage for before you start to regenerate your armor
     * shieldRegenSpeed - is how fast you regen your shield
     * -Ganderman Dan
     */
    [HideInInspector]
    public bool isInvinsible;
    private float isInvinsibleTimer;

    void Start()
    {
        
    }

    //i dont think we will need to use FixedUpdate() in this script, but i put it here just in case, and so we dont forget about it
    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        
    }
}
