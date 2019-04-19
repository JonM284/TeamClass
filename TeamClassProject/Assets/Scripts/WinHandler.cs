using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinHandler : MonoBehaviour 
{
    GameObject team1;
    GameObject team2;

    bool t1_p1_dead = false;
    bool t1_p2_dead = false;

    bool t2_p1_dead = false;
    bool t2_p2_dead = false;

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
        // if(winActive == false && (team1.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <=0 && team1.GetComponent<SwitchHandler>().teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        if(t1_p1_dead && t1_p2_dead)
        {
            winActive = true;
            SceneManager.LoadScene("WinRed");
        }

        if (t2_p1_dead && t2_p2_dead)
        {
            winActive = true;
            SceneManager.LoadScene("WinBlue");
        }




        //team 2 loses
        // if (winActive == false && (team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 && team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        if (t2_p1_dead && t2_p2_dead)
        {
            winActive = true;
           // SceneManager.LoadScene("WinBlue");
        }

        if (team1.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 && t1_p1_dead == false)
        {
            t1_p1_dead = true;
            team1.GetComponent<SwitchHandler>().teammate1_fighter.SetActive(false);
            team1.GetComponent<SwitchHandler>().teammate2_support.SetActive(false);
            team1.GetComponent<SwitchHandler>().teammate2_fighter.SetActive(true);
        }
        if (team1.GetComponent<SwitchHandler>().teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 && t1_p2_dead == false)
        {
            t1_p2_dead = true;
            team1.GetComponent<SwitchHandler>().teammate2_fighter.SetActive(false);
            team1.GetComponent<SwitchHandler>().teammate1_support.SetActive(false);
            team1.GetComponent<SwitchHandler>().teammate1_fighter.SetActive(true);
        }


        if (team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 && t2_p1_dead == false)
        {
            t2_p1_dead = true;
            team2.GetComponent<SwitchHandler>().teammate1_fighter.SetActive(false);
            team2.GetComponent<SwitchHandler>().teammate2_support.SetActive(false);
            team2.GetComponent<SwitchHandler>().teammate2_fighter.SetActive(true);
        }
        if (team2.GetComponent<SwitchHandler>().teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 && t2_p2_dead == false)
        {
            t2_p2_dead = true;
            team2.GetComponent<SwitchHandler>().teammate1_fighter.SetActive(true);
            team2.GetComponent<SwitchHandler>().teammate1_support.SetActive(false);
            team2.GetComponent<SwitchHandler>().teammate2_fighter.SetActive(false);
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
