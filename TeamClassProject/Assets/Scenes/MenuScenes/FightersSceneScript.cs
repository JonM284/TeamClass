using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;

public class FightersSceneScript : MonoBehaviour
{

    public EventSystem es;
    public GameObject backButton;
    public GameObject claireButton;
    public GameObject gilbertButton;
    public GameObject gnoButton;
    public GameObject wawaButton;
    public int hPosition, vPosition;

    public GameObject claireText, gilbertText, gnoText, wawaText;

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    private Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public float timer;
    public float timerMax;

    void Awake()
    {
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        timer = 0f;
        timerMax = 15f;

        hPosition = 1;
        vPosition = 1;

        es.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        timer++;

        if (timer >= timerMax)
        {
            timer = timerMax;
        }

        if (es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }

        if (myPlayer.GetAxis("Vertical") >= 0.1f && timer >= timerMax && vPosition == 2)
        {
            timer = 0;
            vPosition = 1;
        }

        if (myPlayer.GetAxis("Vertical") <= -0.1f && timer >= timerMax && vPosition == 1)
        {
            timer = 0;
            vPosition = 2;
        }

        if (myPlayer.GetAxis("Horizontal") <= -0.1f && timer >= timerMax && vPosition == 1)
        {
            timer = 0;
            hPosition--;
        }

        if (myPlayer.GetAxis("Horizontal") >= 0.1f && timer >= timerMax && vPosition == 1)
        {
            timer = 0;
            hPosition++;
        }

        if (hPosition <= 0)
        {
            hPosition = 4;
        }

        if (hPosition >= 5)
        {
            hPosition = 1;
        }

        if (hPosition == 1 && vPosition == 1)
        {
            es.SetSelectedGameObject(claireButton);
            claireText.SetActive(true);
        }
        else
        {
            claireText.SetActive(false);
        }

        if (hPosition == 2 && vPosition == 1)
        {
            es.SetSelectedGameObject(gilbertButton);
            gilbertText.SetActive(true);
        }
        else
        {
            gilbertText.SetActive(false);
        }

        if (hPosition == 3 && vPosition == 1)
        {
            es.SetSelectedGameObject(gnoButton);
            gnoText.SetActive(true);
        }
        else
        {
            gnoText.SetActive(false);
        }

        if (hPosition == 4 && vPosition == 1)
        {
            es.SetSelectedGameObject(wawaButton);
            wawaText.SetActive(true);
        }
        else
        {
            wawaText.SetActive(false);
        }

        if (vPosition == 2)
        {
            es.SetSelectedGameObject(backButton);
        }

        if (myPlayer.GetButtonDown("HeavyAttack") && timer >= timerMax)
        {
            timer = 0;
            SceneManager.LoadScene("NolanScene");
        }
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
