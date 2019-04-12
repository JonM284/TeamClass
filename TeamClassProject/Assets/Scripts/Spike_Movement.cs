using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Movement : MonoBehaviour
{

    public GameObject mySpike;
    public Transform myEndPos;
    public Vector3 myStartPos;
    public bool Spike_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;
    public int damageToDealToPlayer;

    // Start is called before the first frame update
    void Start()
    { 
        //set start position, disable sprite and collider of spikes, set damage
        myStartPos = mySpike.transform.position;
        mySpike.GetComponent<SpriteRenderer>().enabled = false;
        mySpike.GetComponent<BoxCollider2D>().enabled = false;
        damageToDealToPlayer = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to send spike, send spike and do Activate_Spike();
        if (Spike_Active)
        {
            Activate_Spike();
        }

    }

    //the function done to move the spike upwards
    public void Activate_Spike()
    {
        mySpike.transform.position = Vector3.Lerp(mySpike.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
    }

    //the function that happens when the spike hits the invisible collider above the map to disable it
    public void Deactivate_Spike()
    {
        Spike_Active = false;
        mySpike.transform.position = myStartPos;
        mySpike.GetComponent<SpriteRenderer>().enabled = false;
        mySpike.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //disables the spike when it reaches above map
        if(other.gameObject.name == "Spike_EndPos")
        {
            Deactivate_Spike();
        }
        //deals damage to player if spike hits player
        if (other.gameObject.tag == "Player")
        {
            print("hit player");
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(damageToDealToPlayer, 0, 0, 0, 0, 0, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(),0,0,0);
            
        }
    }

}
