using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePlatform_Vacuum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayerScript>().gravityUp = 49f;
            other.gameObject.GetComponent<BasicPlayerScript>().gravityDown = 34f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayerScript>().gravityUp = 70f;
            other.gameObject.GetComponent<BasicPlayerScript>().gravityDown = 48f;
        }
    }

}
