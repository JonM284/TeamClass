using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Movement : MonoBehaviour
{

    public GameObject myTree;
    public Transform myEndPos;
    private Vector3 myStartPos;
    public bool Tree_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;

    public float removeTreeTimer, removeTreeLength;

    // Start is called before the first frame update
    void Start()
    {
        removeTreeTimer = 0;
        removeTreeLength = 5f;
        myStartPos = myTree.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Tree_Active)
        {
            Activate_Tree();
        }

        if(removeTreeTimer > 0)
        {
            removeTreeTimer -= Time.deltaTime;
        }


        /*if (!Spike_Active)
        {
            Deactivate_Spike();
        }*/
    }

    public void Activate_Tree()
    {


        //if (Vector3.Distance(mySpike.transform.position, myEndPos.position) > 1)
        //{
        myTree.transform.position = Vector3.Lerp(myTree.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        //}

        /*if (Vector3.Distance(mySpike.transform.position, myEndPos.position) <= 1)
        {
            Spike_Active = false;
            Debug.Log("Now Equal, should go back");
            mySpike.transform.position = myStartPos;
        }*/


    }

    public void Deactivate_Tree()
    {
        Tree_Active = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Tree_EndPos")
        {
            Deactivate_Tree();
        }
    }

}

