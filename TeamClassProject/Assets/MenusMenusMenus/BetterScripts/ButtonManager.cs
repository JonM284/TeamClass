using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Image playButton;
    public Image fightersButton;
    public Image optionsButton;
    public Image extrasButton;
    public Image quitButton;

    // Know which game object should be selected
    public Image selected;
    // Know where our selection is
    public int xPos, yPos;

    // Start is called before the first frame update
    void Start()
    {
        // Pick the first selected
        xPos = 1;
        yPos = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(xPos == 1 && yPos == 1){
            selected = playButton;
        }

        if (xPos == 1 && yPos == 2)
        {
            selected = fightersButton;
        }

        if (xPos == 1 && yPos == 3)
        {
            selected = optionsButton;
        }

        if (xPos == 1 && yPos == 4)
        {
            selected = extrasButton;
        }

        if (xPos == 1 && yPos == 5)
        {
            selected = quitButton;
        }
    }

    public void playBtn()
    {
        SceneManager.LoadScene("MixedTogetherScene");
    }

    public void fighterBtn()
    {
        SceneManager.LoadScene("Fighters");
    }

    public void optionsBtn()
    {
        //SceneManager.LoadScene("");
    }

    public void extrasBtn()
    {
        //SceneManager.LoadScene("");
    }

    public void quitBtn()
    {
        Application.Quit();
    }

    public void menuBtn()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
