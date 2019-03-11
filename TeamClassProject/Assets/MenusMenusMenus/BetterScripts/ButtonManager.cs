using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{

    public EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(es.currentSelectedGameObject == null){
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }
    }

    public void playBtn()
    {
        if (MenuSliderScript.sliderInt == 1)
        {
            SceneManager.LoadScene("MixedTogetherScene");
        }
    }

    public void fighterBtn()
    {
        if (MenuSliderScript.sliderInt == 1)
        {
            SceneManager.LoadScene("Fighters");
        }
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
        if (MenuSliderScript.sliderInt == 1)
        {
            Application.Quit();
        }
    }

    public void menuBtn()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
