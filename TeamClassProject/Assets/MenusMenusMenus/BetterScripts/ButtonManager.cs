using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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
        //SceneManager.LoadScene("");
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

    public void backBtn()
    {
        //SceneManager.LoadScene("");
    }
}
