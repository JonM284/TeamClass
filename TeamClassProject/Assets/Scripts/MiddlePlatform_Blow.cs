﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePlatform_Blow : MonoBehaviour
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
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayerScript>().jumpVel = 2;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BasicPlayerScript>().jumpVel = 13;
        }
    }

}
