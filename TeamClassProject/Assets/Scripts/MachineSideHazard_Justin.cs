using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSideHazard_Justin : MonoBehaviour
{
    float sideHazardMovement;
    float moveSpeed = 2f;


    public bool sideHazardMachineEnabled;
    public GameObject sideHazard;
    private Vector2 sideHazardStartPos;

    private bool sideHazardShouldLerp;
    private bool sideHazardMovingForward;
    float sideHazardLerpSpeed;
    float sideHazardLerpTimer;

    bool sideHazardReady;
    float sideHazardCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        sideHazardStartPos = sideHazard.transform.position;
        sideHazardLerpSpeed = 30f;
        sideHazardReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(sideHazardMachineEnabled == true && sideHazardReady == true)
        {
            sideHazardMovement = Input.GetAxisRaw("Vertical") * moveSpeed;

            //move up
            if (sideHazardMovement > 0)
            {
                sideHazard.transform.position += Vector3.up * Time.deltaTime * moveSpeed;
            }
            //move down
            if (sideHazardMovement < 0)
            {
                sideHazard.transform.position += -Vector3.up * Time.deltaTime * moveSpeed;
            }

            

        }

        //cooldown timer
        sideHazardCooldownTimer -= Time.deltaTime;

        //cooldown finished
        if(sideHazardCooldownTimer < 0)
        {
            sideHazardReady = true;
        }

        //lerping timer
        sideHazardLerpTimer += Time.deltaTime;

        //if you're in the machine and you hit jump the eel goes/starts
        if (sideHazardMachineEnabled == true && Input.GetButtonDown("Jump") && sideHazardReady == true)
        {
            sideHazardReady = false;
            sideHazardShouldLerp = true;
            sideHazardMovingForward = true;
            sideHazardLerpTimer = 0;
            sideHazardCooldownTimer = 5.0f;
        }

        //eel moves right first
        if (sideHazardShouldLerp == true && sideHazardMovingForward == true)
        {
            sideHazard.transform.Translate(Vector3.right * sideHazardLerpSpeed * Time.deltaTime);
        }
        //eel moves back left after
        else if (sideHazardShouldLerp == true && sideHazardMovingForward == false)
        {
            sideHazard.transform.Translate(Vector3.left * sideHazardLerpSpeed * Time.deltaTime);
        }

        //if the timer exceeds half a second but not a second, and its moving right/forward, then stop it 
        if (sideHazardLerpTimer > .5f && sideHazardLerpTimer <= 1.0f && sideHazardShouldLerp == true && sideHazardMovingForward == true)
        {
            sideHazardShouldLerp = false;
            //sideHazardMovingForward = false;
        }
        //it stops for half a second then moves back to left
        if (sideHazardLerpTimer > 1.0f && sideHazardMovingForward == true)
        {
            sideHazardMovingForward = false;
            sideHazardShouldLerp = true;
        }
        //after cycle completes it moves back to starting position and gets ready for next time
        if (sideHazardLerpTimer >= 1.5f && sideHazardShouldLerp == true)
        {
            sideHazardShouldLerp = false;
            sideHazardMachineEnabled = false;
            sideHazard.transform.position = sideHazardStartPos;
            SupportPlayer.movementEnabled = true;
        }
    }


    IEnumerator SetInMachineTrueDelay()
    {
        yield return new WaitForSeconds(.1f);
        sideHazardMachineEnabled = true;
        SupportPlayer.movementEnabled = false;
        sideHazard.transform.position = new Vector2(sideHazard.transform.position.x + 0.25f, sideHazard.transform.position.y);
    }




    private void OnTriggerStay2D(Collider2D other)
    {
        if(sideHazardCooldownTimer < 0)
        {
            if (other.gameObject.tag == "Player" && Input.GetButtonDown("Jump") && sideHazardMachineEnabled == false && sideHazardReady == true)
            {
                StartCoroutine(SetInMachineTrueDelay());
            }
        }
        
    }

}
