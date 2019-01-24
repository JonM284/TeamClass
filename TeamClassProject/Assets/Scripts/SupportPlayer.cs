using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportPlayer : MonoBehaviour
{

    public float moveSpeed;
    float movement;
    public GameObject platformMid1;
    bool isPlatformActive;
    float platformTimer;
    public BoxCollider2D machine1Collider;

    // Start is called before the first frame update
    void Start()
    {
        isPlatformActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        //input for horizontal input
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

        //Platform has 5 second cooldown

        //activate/deactivate platformMid1
        if(isPlatformActive == true)
        {
            platformMid1.SetActive(true);
            machine1Collider.enabled = false;
        }
        else if(isPlatformActive == false)
        {
            platformMid1.SetActive(false);
            machine1Collider.enabled = true;
        }
        //platform appears for 5 seconds then goes off
        platformTimer += Time.deltaTime;
        if(platformTimer > 5 && isPlatformActive == true)
        {
            isPlatformActive = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Stand touching machine and press f to activate platform
        if(other.gameObject.tag == "Machine1" && Input.GetButtonDown("ActivateMachine"))
        {
            //activate platform
            isPlatformActive = true;
            platformTimer = 0;
           
        }
    }

}
