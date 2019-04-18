using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Movement : MonoBehaviour
{

    public GameObject myTree;
    public Transform myEndPos;
    public Vector3 myStartPos;
    public bool Tree_Active = false;
    private bool hasStarted = false;
    public float going_Out_Speed, return_Speed;

    public float removeTreeTimer, removeTreeLength;
    public bool treeIsThere;
    private Color myColor;
    private RigidbodyConstraints2D originalConstraints;
    private Quaternion treeRotation;

    private void Awake()
    {
        originalConstraints = myTree.GetComponent<Rigidbody2D>().constraints;
        treeRotation = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        //timer to remove tree after being sent out, the length it takes to remove it, set start position, disable sprite and collider of spikes,
        //set damage, set bool treeIsThere (meaning tree is on map) to off
        removeTreeTimer = 0;
        removeTreeLength = 5f;
        myStartPos = myTree.transform.position;
        myTree.GetComponent<SpriteRenderer>().enabled = false;
        //myTree.GetComponent<BoxCollider2D>().enabled = false;
        myTree.GetComponent<PolygonCollider2D>().enabled = false;
        treeIsThere = false;
        myColor = new Color(255, 255, 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = treeRotation;

        //if the player presses BasicAttack to send tree, send tree and do Activate_Tree();
        if (Tree_Active)
        {
            Activate_Tree();
        }

        
        //if the tree is on map currently, count timer down til its less than 0, then....
        if(removeTreeTimer > 0 && treeIsThere == true)
        {
            removeTreeTimer -= Time.deltaTime;
            if(removeTreeTimer < 1)
            {
                myColor.a -= .02f;
                myTree.GetComponent<SpriteRenderer>().color = myColor;
            }
        }
        //...disable tree and reset its position for next time machine is used
        if(removeTreeTimer <= 0 && treeIsThere == true)
        {
            treeIsThere = false;
            myTree.GetComponent<SpriteRenderer>().enabled = false;
            //myTree.GetComponent<BoxCollider2D>().enabled = false;
            myTree.GetComponent<PolygonCollider2D>().enabled = false;
            myTree.transform.position = myStartPos;
            myColor.a = 1f;
            myTree.GetComponent<SpriteRenderer>().color = myColor;
        }



    }
    //the function done to move the tree upwards
    public void Activate_Tree()
    {
        myTree.transform.position = Vector3.Lerp(myTree.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
        myTree.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    //the function that happens when the tree hits the invisible collider in the map to stop it moving and start the removeTree Timer
    public void Deactivate_Tree()
    {
        Tree_Active = false;
        treeIsThere = true;
        removeTreeTimer = removeTreeLength;
        myTree.GetComponent<Rigidbody2D>().constraints = originalConstraints;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //when tree hits the trigger deactive the tree ^^^
        if (other.gameObject.name == "Tree_EndPos")
        {
            Deactivate_Tree();
        }
    }

}

