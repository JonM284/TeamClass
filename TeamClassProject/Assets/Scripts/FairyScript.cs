using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyScript : MonoBehaviour
{
    public Vector3 startPos;
    public MachineBehaviour2 machineBehaviourScript_OnFairyMachine;
    public bool fairyHitPlayer;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        fairyHitPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            fairyHitPlayer = true;

            //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(-100f, 0, 0, 0, 0, 0, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0,0 ,0);
            other.gameObject.GetComponent<BasicPlayerScript>().currentHealth += 100f;
            transform.position = startPos;

        }
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            fairyHitPlayer = true;

            //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(-100f, 0, 0, 0, 0, 0, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0,0 ,0);
            other.gameObject.GetComponent<BasicPlayerScript>().currentHealth += 100f;
            transform.position = startPos;

        }
    }

}
