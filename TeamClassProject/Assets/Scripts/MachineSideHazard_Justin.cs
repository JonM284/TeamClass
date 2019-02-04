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

    private bool shouldLerp;
    float lerpTimer;

    // Start is called before the first frame update
    void Start()
    {
        sideHazardStartPos = sideHazard.transform.position;
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

        lerpTimer += Time.deltaTime;

        if (sideHazardMachineEnabled == true && Input.GetButtonDown("Jump"))
        {
            shouldLerp = true;
            lerpTimer = 0;
        }

        if (shouldLerp)
        {
            sideHazard.transform.position = Vector3.Lerp(sideHazardStartPos, new Vector2(sideHazardStartPos.x + 15.75f, sideHazardStartPos.y), 1f);
        }

        if(lerpTimer > 1)
        {
            shouldLerp = false;
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
