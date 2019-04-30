using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.SceneManagement;

public class PortraitFloat : MonoBehaviour
{
    float originalY;
    public int selection;
    public Image clairePic;
    public Image gilbertPic;
    public TextMeshProUGUI claireText;
    public TextMeshProUGUI gilbertText;
    public TextMeshProUGUI claireText2;
    public TextMeshProUGUI gilbertText2;

    public float floatStrength;

    public float timer;
    public float timerMax;

    void Start()
    {
        this.originalY = this.transform.position.y;
        selection = 1;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
        originalY + ((float)Mathf.Sin(Time.time * 2) * floatStrength),
        transform.position.z);

        timer++;

        if (timer >= timerMax)
        {
            timer = timerMax;
        }

        //Iterating through Players (excluding the System Player)
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player myPlayer = ReInput.players.Players[i];

            if (myPlayer.GetAxis("Horizontal") <= -0.1f && timer >= timerMax)
            {
                timer = 0;
                selection--;
            }

            if (myPlayer.GetAxis("Horizontal") >= 0.1f && timer >= timerMax)
            {
                timer = 0;
                selection++;
            }

            if (selection <= -1)
            {
                selection = 1;
            }

            if (selection >= 2)
            {
                selection = 0;
            }

            if (selection == 1)
            {
                clairePic.enabled = true;
                claireText.enabled = true;
                claireText2.enabled = true;
                gilbertPic.enabled = false;
                gilbertText.enabled = false;
                gilbertText2.enabled = false;
            }
            else
            {
                gilbertPic.enabled = true;
                gilbertText.enabled = true;
                gilbertText2.enabled = true;
                clairePic.enabled = false;
                claireText.enabled = false;
                claireText2.enabled = false;
            }

            if (myPlayer.GetButtonDown("HeavyAttack"))
            {
                //Go back to menu
                SceneManager.LoadScene("NolanScene");
            }

        }
    }
}
