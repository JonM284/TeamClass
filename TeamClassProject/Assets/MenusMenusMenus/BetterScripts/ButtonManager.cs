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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
