using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel_Movement : MonoBehaviour
{

    public GameObject myEel;
    public Transform myEndPos;
    private Vector3 myStartPos;
    public bool Eel_Active = false;
    private bool has_Hit_Platform = false, hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        myStartPos = myEel.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Eel_Active)
        {
            Activate_Eel();
        }

        if (!Eel_Active)
        {
            Deactivate_Eel();
        }
    }


    public void Activate_Eel()
    {
        

        if (Vector3.Distance(myEel.transform.position, myEndPos.position) > 1 && !has_Hit_Platform) {
            myEel.transform.position = Vector3.Lerp(myEel.transform.position, new Vector3(myEndPos.position.x, transform.position.y, transform.position.z), Time.deltaTime * 3);
        }else if(Vector3.Distance(myEel.transform.position, myEndPos.position) <= 1 || has_Hit_Platform)
        {
            Eel_Active = false;
            Debug.Log("Now Equal, should go back");
        }


    }

    public void Deactivate_Eel()
    {
        
        myEel.transform.position = Vector3.Lerp(myEel.transform.position, new Vector3(myStartPos.x, transform.position.y, transform.position.z), Time.deltaTime * 5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            Debug.Log("Hit Platform");
            has_Hit_Platform = true;
            Eel_Active = false;
        }
    }
}
