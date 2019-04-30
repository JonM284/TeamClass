using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    bool team1Won;

    [Header("Teammate 1")]
    public Image claireLeft;
    public Image gilbertLeft;
    [Header("Teammate 2")]
    public Image claireRight;
    public Image gilbertRight;
    
    public GameObject fish_background, gnome_Background;

    // Start is called before the first frame update
    void Start()
    {
        claireLeft.enabled = false;
        gilbertLeft.enabled = false;
        claireRight.enabled = false;
        gilbertRight.enabled = false;
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (team1Won)
        {
            //player 1
            if (PlayerPrefs.GetInt("Player1Character") == 1)
            {
                claireLeft.enabled = true;
            }else if(PlayerPrefs.GetInt("Player1Character") == 2)
            {
                gilbertLeft.enabled = true;
            }

            //player 2
            if (PlayerPrefs.GetInt("Player2Character") == 1)
            {
                claireRight.enabled = true;
            }
            else if (PlayerPrefs.GetInt("Player2Character") == 2)
            {
                gilbertRight.enabled = true;
            }
        }
        else
        {
            //player 3
            if (PlayerPrefs.GetInt("Player3Character") == 1)
            {
                claireLeft.enabled = true;
            }
            else if (PlayerPrefs.GetInt("Player3Character") == 2)
            {
                gilbertLeft.enabled = true;
            }

            //player 4
            if (PlayerPrefs.GetInt("Player4Character") == 1)
            {
                claireRight.enabled = true;
            }
            else if (PlayerPrefs.GetInt("Player4Character") == 2)
            {
                gilbertRight.enabled = true;
            }
        }

        if (PlayerPrefs.GetInt("Current_Stage") == 0)
        {
            fish_background.SetActive(true);
            gnome_Background.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Current_Stage") == 1)
        {
            gnome_Background.SetActive(true);
            fish_background.SetActive(false);
        }
    }
}
