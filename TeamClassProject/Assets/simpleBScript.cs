using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using Rewired.ControllerExtensions;

public class simpleBScript : MonoBehaviour
{
    public EventSystem es;
    public GameObject playButton;

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

        /*Player myPlayer = ReInput.players.Players[0];


        if (myPlayer.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("NolanScene");
        }
        */

    }

    public void Go_To_Main_Menu()
    {
        SceneManager.LoadScene("NolanScene");
    }


}
