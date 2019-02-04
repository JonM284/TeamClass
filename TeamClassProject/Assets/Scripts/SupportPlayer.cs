using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportPlayer : MonoBehaviour
{

    public float moveSpeed;
    float movement;
    public static bool movementEnabled;

    bool isPlatformMidActive;
    public GameObject platformMid1;
    float platformMidTimer;
    public BoxCollider2D machine1Collider;

    public bool isPlatformLeftOpen;
    public GameObject platformLeft1;
    public GameObject platformLeft2a;
    public GameObject platformLeft2b;
    float platformLeftTimer;
    public BoxCollider2D machine2aCollider;

    public bool isPlatformRightOpen;
    public GameObject platformRight1;
    public GameObject platformRight2a;
    public GameObject platformRight2b;
    float platformRightTimer;
    public BoxCollider2D machine2bCollider;
   

    // Start is called before the first frame update
    void Start()
    {

        movementEnabled = true;

        isPlatformMidActive = false;
        platformMid1.SetActive(false);
        machine1Collider.enabled = true;

        isPlatformLeftOpen = false;
        machine2aCollider.enabled = true;
        platformLeft1.SetActive(true);
        platformLeft2a.SetActive(false);
        platformLeft2b.SetActive(false);

        isPlatformRightOpen = false;
        machine2bCollider.enabled = true;
        platformRight1.SetActive(true);
        platformRight2a.SetActive(false);
        platformRight2b.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //input for horizontal input
        if(movementEnabled == true)
        {
            movement = Input.GetAxisRaw("HorizontalSupport") * moveSpeed;

            //move right
            if (movement > 0)
            {
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
                //transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            //move left
            if (movement < 0)
            {
                transform.position += -Vector3.right * Time.deltaTime * moveSpeed;
                //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
        

        //Platform Mid
        //platform mid appears for 5 seconds then goes off
        platformMidTimer += Time.deltaTime;
        if(platformMidTimer > 5 && isPlatformMidActive == true)
        {
            isPlatformMidActive = false;
            platformMid1.SetActive(false);
            machine1Collider.enabled = true;
        }


        //Platform Left
        //platform left opens for 5 seconds then closes
        platformLeftTimer += Time.deltaTime;
        if(platformLeftTimer > 5 && isPlatformLeftOpen == true)
        {
            isPlatformLeftOpen = false;
            machine2aCollider.enabled = true;
            platformLeft1.SetActive(true);
            platformLeft2a.SetActive(false);
            platformLeft2b.SetActive(false);
        }

        //Platform Right
        //platform right opens for 5 seconds then closes
        platformRightTimer += Time.deltaTime;
        if (platformRightTimer > 5 && isPlatformRightOpen == true)
        {
            isPlatformRightOpen = false;
            machine2bCollider.enabled = true;
            platformRight1.SetActive(true);
            platformRight2a.SetActive(false);
            platformRight2b.SetActive(false);
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Stand touching machine and press e to activate platform
        if(other.gameObject.name == "Machine1" && Input.GetButtonDown("ActivateMachine"))
        {
            isPlatformMidActive = true;
            platformMidTimer = 0;
            machine1Collider.enabled = false;
            platformMid1.SetActive(true);
        }
        if(other.gameObject.name == "Machine2a" && Input.GetButtonDown("ActivateMachine"))
        {
            isPlatformLeftOpen = true;
            platformLeftTimer = 0;
            machine2aCollider.enabled = false;
            platformLeft1.SetActive(false);
            platformLeft2a.SetActive(true);
            platformLeft2b.SetActive(true);
        }
        if (other.gameObject.name == "Machine2b" && Input.GetButtonDown("ActivateMachine"))
        {
            isPlatformRightOpen = true;
            platformRightTimer = 0;
            machine2bCollider.enabled = false;
            platformRight1.SetActive(false);
            platformRight2a.SetActive(true);
            platformRight2b.SetActive(true);
        }
    }

}
