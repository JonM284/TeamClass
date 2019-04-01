using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChip : MonoBehaviour
{
    public int PlayerID;
    public int position;

    public static int currentID;

    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;
    public GameObject random;
    public bool canMove;

    public float timer;
    public float changePlayerTimer;

    /* Bools will tell us if someone voted and then we disable their input
    true - did vote, false - did not vote */
    public bool P1_Voted, P2_Voted, P3_Voted, P4_Voted = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        currentID = 1;

        canMove = true;

        changePlayerTimer = 25f;
    }

    // Update is called once per frame
    void Update()
    {
        timer++;

        Debug.Log(currentID);

        if(timer >= 2.5f)
        {
            timer = 2.5f;
        }

        if (position <= 0)
        {
            position = 5;
        }

        if (position >= 6)
        {
            position = 1;
        }

        if (Input.GetKey(KeyCode.RightArrow) && timer >= 2.5f && canMove == true && currentID == PlayerID)
        {
            position += 1;
            timer = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && timer >= 2.5f && canMove == true && currentID == PlayerID)
        {
            position -= 1;
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return) && timer >= 2.5f && canMove == true && currentID == PlayerID)
        {
            timer = 0;
            currentID += 1;
        }

        if (Input.GetKeyDown(KeyCode.Return) && timer >= 2.5f && canMove == true && currentID == 4)
        {
            timer = 0;
            canMove = false;
        }

        if(currentID >= 4)
        {
            currentID = 4;
        }

        if (position == 1)
        {
            stage1.SetActive(true);
        }
        else
        {
            stage1.SetActive(false);
        }

        if (position == 2)
        {
            stage2.SetActive(true);
        }
        else
        {
            stage2.SetActive(false);
        }

        if (position == 3)
        {
            stage3.SetActive(true);
        }
        else
        {
            stage3.SetActive(false);
        }

        if (position == 4)
        {
            stage4.SetActive(true);
        }
        else
        {
            stage4.SetActive(false);
        }

        if (position == 5)
        {
            random.SetActive(true);
        }
        else
        {
            random.SetActive(false);
        }
    }
}
