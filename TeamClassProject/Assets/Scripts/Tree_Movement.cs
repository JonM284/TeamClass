﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        //timer to remove tree after being sent out, the length it takes to remove it, set start position, disable sprite and collider of spikes,
        //set damage, set bool treeIsThere (meaning tree is on map) to off
        removeTreeTimer = 0;
        removeTreeLength = 5f;
        myStartPos = myTree.transform.position;
        myTree.GetComponent<SpriteRenderer>().enabled = false;
        myTree.GetComponent<BoxCollider2D>().enabled = false;
        treeIsThere = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to send tree, send tree and do Activate_Tree();
        if (Tree_Active)
        {
            Activate_Tree();
        }

        //if the tree is on map currently, count timer down til its less than 0, then....
        if(removeTreeTimer > 0 && treeIsThere == true)
        {
            removeTreeTimer -= Time.deltaTime;
        }
        //...disable tree and reset its position for next time machine is used
        if(removeTreeTimer <= 0 && treeIsThere == true)
        {
            treeIsThere = false;
            myTree.GetComponent<SpriteRenderer>().enabled = false;
            myTree.GetComponent<BoxCollider2D>().enabled = false;
            myTree.transform.position = myStartPos;
        }

    }
    //the function done to move the tree upwards
    public void Activate_Tree()
    {
        myTree.transform.position = Vector3.Lerp(myTree.transform.position, new Vector3(transform.position.x, myEndPos.position.y, transform.position.z), Time.deltaTime * going_Out_Speed);
    }

    //the function that happens when the tree hits the invisible collider in the map to stop it moving and start the removeTree Timer
    public void Deactivate_Tree()
    {
        Tree_Active = false;
        treeIsThere = true;
        removeTreeTimer = removeTreeLength;
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
