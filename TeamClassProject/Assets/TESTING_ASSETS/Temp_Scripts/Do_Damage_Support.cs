using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Do_Damage_Support : MonoBehaviour
{

    public float attack_Damage;
    public bool is_Spike;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && is_Spike)
        {
            float angle = Mathf.Atan2(collision.transform.position.y, collision.transform.position.x);
            if (transform.position.x < collision.transform.position.x)
            {
                collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 45, 100 ,0.2f, 60f, 0.5f, false, 0.1f, 0.3f, 0.2f);
            }else if (transform.position.x > collision.transform.position.x)
            {
                collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 45, 100 ,0.2f, 60f, 0.5f, true, 0.1f, 0.3f, 0.2f);
            }
           
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !is_Spike)
        {
            collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 0, 0,0,0,0, true, 0,0,0);
        }
    }
}
