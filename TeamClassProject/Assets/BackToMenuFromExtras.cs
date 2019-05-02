using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using Rewired.ControllerExtensions;

public class backToMenuFromExtras : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];


            if (myPlayer.GetButtonDown("HeavyAttack"))
            {
                SceneManager.LoadScene("NolanScene");
            }
        }
    }
}
