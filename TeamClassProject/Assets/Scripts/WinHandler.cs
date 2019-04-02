using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinHandler : MonoBehaviour 
{
    GameObject team1;
    GameObject team2;


    public Image winScreen;
    public Text winText;

    public Text team1Text;
    public Text team2Text;

    private bool winActive = false;




    // Start is called before the first frame update
    void Start()
    {
        winScreen.enabled = false;
        team1 = GameObject.Find("Team1");
        team2 = GameObject.Find("Team2");
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(winActive == false && (team1.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <=0 || team1.GetComponent<SwitchHandler>().teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        {
            winScreen.enabled = true;
            winText.text = "TEAM 2 WINS!";
            winActive = true;
            team1Text.enabled = false;
            team2Text.enabled = false;
        }

        if (winActive == false && (team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0 || team2.GetComponent<SwitchHandler>().teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth <= 0))
        {
            winScreen.enabled = true;
            winText.text = "TEAM 1 WINS!";
            winActive = true;
            team1Text.enabled = false;
            team2Text.enabled = false;
        }

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
    }
}
