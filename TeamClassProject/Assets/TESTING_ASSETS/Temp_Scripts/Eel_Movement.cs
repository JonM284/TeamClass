using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel_Movement : MonoBehaviour
{

    public GameObject myEel;
    public Transform myEndPos;
    private Vector3 myStartPos, pause_Position;
    public bool Eel_Active = false;
    public bool has_Hit_Platform = false, hasStarted = false, is_Facing_Right;
    public float going_Out_Speed, return_Speed, pause_timer;
    
    public float Head_Poke;
    private float pause_timer_Max;


    // Start is called before the first frame update
    void Start()
    {
        myStartPos = myEel.transform.position;
        pause_timer_Max = pause_timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Eel_Active && pause_timer > 0)
        {
            pause_timer -= Time.deltaTime;
            Set_Up_Eel();
        }


        if (Eel_Active && pause_timer <= 0)
        {
            Activate_Eel();
        }

        if (!Eel_Active)
        {
            Deactivate_Eel();
        }


    }

    private void LateUpdate()
    {
        if (is_Facing_Right)
        {
            pause_Position = new Vector3(myStartPos.x + Head_Poke, transform.position.y, transform.position.z);
        }
        else
        {
            pause_Position = new Vector3(myStartPos.x - Head_Poke, transform.position.y, transform.position.z);
        }
    }

    void Set_Up_Eel()
    {
        myEel.transform.position = Vector3.Lerp(myEel.transform.position, pause_Position, Time.deltaTime * going_Out_Speed);
    }


    public void Activate_Eel()
    {
        

        if (Vector3.Distance(myEel.transform.position, myEndPos.position) > 1 && !has_Hit_Platform && Eel_Active) {
            myEel.transform.position = Vector3.Lerp(myEel.transform.position, new Vector3(myEndPos.position.x, transform.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        }

        if (Vector3.Distance(myEel.transform.position, myEndPos.position) <= 1 || has_Hit_Platform)
        {
            Eel_Active = false;
            pause_timer = pause_timer_Max;
            Debug.Log("Now Equal, should go back");
        }


    }

    public void Actual_Activate_Eel()
    {
        Eel_Active = true;
        has_Hit_Platform = false;
        pause_timer = pause_timer_Max;
    }

    public void Deactivate_Eel()
    {
        
        myEel.transform.position = Vector3.Lerp(myEel.transform.position, new Vector3(myStartPos.x, transform.position.y, transform.position.z), Time.deltaTime * return_Speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            Debug.Log("Hit Platform");
            has_Hit_Platform = true;
            Eel_Active = false;
        }
        if (other.gameObject.tag == "Player")
        {
            float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(15f, angle, 0, 0.2f, 20f, 1.5f, true, 0.1f, 0.3f, 0.2f);
        }
    }
}
