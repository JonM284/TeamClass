using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltManager : MonoBehaviour
{
    /* please put your name at the end of important comments so that we know who wrote what, so that if one of us has a problem understanding
     * what the code does, we can directly ask the person, instead of asking everyone
     * also im putting this block comment at the top of every script, sorry if it gets repetitive
     * -Ganderman Dan
     */

    /* this script should be where all the ulimate related stuff should go, im not even sure if we will have ultimate abilities in the game, i know we... 
     * brought it up at one meeting, but i forgot if we were including it, but here it is anyway
     *          -Ganderman Dan 
     */



    [Header("Ultimate Attributes")]
    public float maxCharge;
    private float currentCharge;
    public float chargeRate;
    [Tooltip("This number helps determine how much charge you get from getting hit, and hitting enemies")]
    public float chargeMultiplier;
    [Tooltip("The number of 'ult blocks' you have that makes up your ult bar, go to reference page for further explanation.")]
    public float numUltBlocks;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
