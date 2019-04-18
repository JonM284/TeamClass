using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;

public class WinScreen : MonoBehaviour
{
    public EventSystem es;
    public GameObject menuButton;

    //the following is in order to use rewired
    //[Tooltip("Reference for using rewired")]
    //private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        es.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }

        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];
        }
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
