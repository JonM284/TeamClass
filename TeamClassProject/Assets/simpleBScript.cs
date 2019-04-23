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
    private int countDown;
    public EventSystem es;
    public GameObject playButton;

    // Start is called before the first frame update
    void Start()
    {
        countDown = 250;
        es.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        countDown--;

        if (es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }
    }

    public void Go_To_Main_Menu()
    {
        if (countDown < 0)
        {
            SceneManager.LoadScene("NolanScene");
        }
    }


}
