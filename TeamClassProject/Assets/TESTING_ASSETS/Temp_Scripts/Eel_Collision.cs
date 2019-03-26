using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel_Collision : MonoBehaviour
{
    public GameObject my_Parent;

    private void Start()
    {
        my_Parent = this.transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform") {
            my_Parent.GetComponent<Eel_Movement>().has_Hit_Platform = true;
            my_Parent.GetComponent<Eel_Movement>().Eel_Active = true;
        }

        if (other.gameObject.tag == "Player")
        {
            my_Parent.GetComponent<Eel_Movement>().has_Hit_Platform = true;
            my_Parent.GetComponent<Eel_Movement>().Eel_Active = true;
            float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(75f, angle, 0, 0.2f, 20f, 1.5f, true, 0.1f, 0.3f, 0.2f);
        }

        
    }
}
