using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel_Movement : MonoBehaviour
{

    public GameObject myEel;
    public Transform myEndPos;
    private Vector3 myStartPos, pause_Position;
    public bool Eel_Active = false;
    public bool has_Hit_Platform = false, hasStarted = false, is_Facing_Right, is_In_Pause = false;
    public float going_Out_Speed, return_Speed, Poke_Dist, Poke_timer;
    private float poke_Timer_Max;


    // Start is called before the first frame update
    void Start()
    {
        myStartPos = myEel.transform.position;
        poke_Timer_Max = Poke_timer;
        if (is_Facing_Right)
        {
            pause_Position.x = myEel.transform.position.x + Poke_Dist;
        }
        else
        {
            pause_Position.x = myEel.transform.position.x - Poke_Dist;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is_In_Pause && Poke_timer > 0)
        {
            Poke_timer -= Time.deltaTime;
            Pause_Eel();
        }

        if (Poke_timer <= 0 && is_In_Pause)
        {
            is_In_Pause = false;
            Eel_Active = true;
        }

        if (Eel_Active && !is_In_Pause)
        {
            Activate_Eel();
        }

        if (!Eel_Active && !is_In_Pause)
        {
            Deactivate_Eel();
        }


    }

    private void LateUpdate()
    {
        pause_Position.y = transform.position.y;
        pause_Position.z = transform.position.z;
    }

    void Pause_Eel()
    {
        myEel.transform.position = Vector3.Lerp(myEel.transform.position, pause_Position, Time.deltaTime * going_Out_Speed);
    }

    public void Actually_Activate()
    {
        Poke_timer = poke_Timer_Max;
        has_Hit_Platform = false;
        is_In_Pause = true;
        Eel_Active = false;
    }



    public void Activate_Eel()
    {

        if (Vector3.Distance(myEel.transform.position, myEndPos.position) > 1 && !has_Hit_Platform && Eel_Active) {
            myEel.transform.position = Vector3.Lerp(myEel.transform.position, new Vector3(myEndPos.position.x, transform.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        }

        if (Vector3.Distance(myEel.transform.position, myEndPos.position) <= 1 || has_Hit_Platform)
        {
            Eel_Active = false;
            Debug.Log("Now Equal, should go back");
        }


    }

    public void Do_Flash()
    {
        StartCoroutine(flash_Control());
    }

    public IEnumerator flash_Control()
    {

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.1f);
                GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(0.1f);
                GetComponent<SpriteRenderer>().color = Color.white;
            }
       
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
