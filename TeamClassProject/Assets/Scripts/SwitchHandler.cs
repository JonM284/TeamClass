using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandler : MonoBehaviour
{
    public GameObject teammate1_fighter;
    public GameObject teammate1_support;

    public GameObject teammate2_fighter;
    public GameObject teammate2_support;

    float teammate1_timer = 0;
    float teammate2_timer = 0;

    int teammate1_num;
    int teammate2_num;

    // Start is called before the first frame update
    void Start()
    {
        teammate1_num = teammate1_fighter.GetComponent<BasicPlayerScript>().playerNum;
        teammate2_num = teammate2_fighter.GetComponent<BasicPlayerScript>().playerNum;

        teammate1_fighter.SetActive(true);
        teammate1_support.SetActive(false);

        teammate2_fighter.SetActive(false);
        teammate2_support.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        teammate1_timer -= Time.deltaTime;
        teammate2_timer -= Time.deltaTime;

        if(teammate1_timer > 0 && teammate2_timer > 0)
        {
            teammate2_timer = 0;
            teammate1_timer = 0;

            if (teammate1_fighter.activeSelf)
            {
                teammate1_fighter.SetActive(false);
                teammate1_support.SetActive(true);
            }
            else
            {
                teammate1_fighter.SetActive(true);
                teammate1_support.SetActive(false);
            }


            if (teammate2_fighter.activeSelf)
            {
                teammate2_fighter.SetActive(false);
                teammate2_support.SetActive(true);
            }
            else
            {
                teammate2_fighter.SetActive(true);
                teammate2_support.SetActive(false);
            }
        }
    }

    public void BeginSwap(int playerNum)
    {
        if(playerNum == teammate1_num)
        {
            teammate1_timer = .5f;
        }
        else
        {
            if (playerNum == teammate2_num)
            {
                teammate2_timer = .5f;
            }
        }
    }
}
