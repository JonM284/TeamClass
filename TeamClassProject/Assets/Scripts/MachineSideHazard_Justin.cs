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
    float sideHazardLerpSpeed;
    float sideHazardLerpTimer;

    // Start is called before the first frame update
    void Start()
    {
        sideHazardStartPos = sideHazard.transform.position;
        sideHazardLerpSpeed = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if(sideHazardMachineEnabled == true)
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

        sideHazardLerpTimer += Time.deltaTime;

        if (sideHazardMachineEnabled == true && Input.GetButtonDown("Jump"))
        {
            sideHazardShouldLerp = true;
            sideHazardLerpTimer = 0;
        }

        if (sideHazardShouldLerp == true)
        {
            sideHazard.transform.Translate(Vector3.right * sideHazardLerpSpeed * Time.deltaTime);
        }

        if(sideHazardLerpTimer > .5f)
        {
            sideHazardShouldLerp = false;
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
        if(other.gameObject.tag == "Player" && Input.GetButtonDown("Jump") && sideHazardMachineEnabled == false)
        {
            StartCoroutine(SetInMachineTrueDelay());
        }
    }

}
