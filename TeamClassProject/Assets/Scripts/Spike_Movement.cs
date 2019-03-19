using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Movement : MonoBehaviour
{

    public GameObject mySpike;
    public Transform myEndPos;
    private Vector3 myStartPos;
    public bool Spike_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;

    // Start is called before the first frame update
    void Start()
    {
        myStartPos = mySpike.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spike_Active)
        {
            Activate_Spike();
        }

        /*if (!Spike_Active)
        {
            Deactivate_Spike();
        }*/
    }

    public void Activate_Spike()
    {


        //if (Vector3.Distance(mySpike.transform.position, myEndPos.position) > 1)
        //{
            mySpike.transform.position = Vector3.Lerp(mySpike.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        //}

        /*if (Vector3.Distance(mySpike.transform.position, myEndPos.position) <= 1)
        {
            Spike_Active = false;
            Debug.Log("Now Equal, should go back");
            mySpike.transform.position = myStartPos;
        }*/


    }

    public void Deactivate_Spike()
    {
        Spike_Active = false;
        mySpike.transform.position = myStartPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Spike_EndPos")
        {
            Deactivate_Spike();
        }
    }

}
