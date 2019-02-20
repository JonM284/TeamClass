using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public int buttonID;
    public static int lastButtonOn;
    public GameObject selector;
    public GameObject flash;

    public GameObject playVideo;
    public GameObject fightersVideo;
    public GameObject optionsVideo;
    public GameObject extrasVideo;
    public GameObject quitVideo;

    // Start is called before the first frame update
    void Start()
    {
        lastButtonOn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastButtonOn == 1)
        {
            playVideo.SetActive(true);
        } else
        {
            playVideo.SetActive(false);
        }

        if (lastButtonOn == 2)
        {
            fightersVideo.SetActive(true);
        }
        else
        {
            fightersVideo.SetActive(false);
        }

        if (lastButtonOn == 3)
        {
            optionsVideo.SetActive(true);
        }
        else
        {
            optionsVideo.SetActive(false);
        }

        if (lastButtonOn == 4)
        {
            extrasVideo.SetActive(true);
        }
        else
        {
            extrasVideo.SetActive(false);
        }

        if (lastButtonOn == 5)
        {
            quitVideo.SetActive(true);
        }
        else
        {
            quitVideo.SetActive(false);
        }

        //Debug.Log(lastButtonOn);
    }

    void OnMouseEnter()
    {
        selector.SetActive(true);

        if(lastButtonOn != buttonID){
            flash.SetActive(true);
        }

        lastButtonOn = buttonID;
    }

    void OnMouseExit()
    {
        selector.SetActive(false);
        flash.SetActive(false);
    }
}
