using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinHandler : MonoBehaviour 
{
    GameObject team1;
    GameObject team2;


    private bool winActive = false;




    // Start is called before the first frame update
    void Start()
    {
        team1 = GameObject.Find("Team1");
        team2 = GameObject.Find("Team2");
    }

    // Update is called once per frame
    void Update()
    {
        //team 1 loses
        if(winActive == false && (team1.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <=0 || team1.GetComponent<SwitchHandler>().teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        {
            winActive = true;
            SceneManager.LoadScene("WinRed");
        }

        //team 2 loses
        if (winActive == false && (team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 || team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        {
            winActive = true;
            SceneManager.LoadScene("WinBlue");
        }

        /*
        if (winActive)
        {
            if(Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene("JonScene");
            }
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("JonScene");
        }
        */
    }
}
