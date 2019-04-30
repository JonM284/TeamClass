using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backToMenuFromFighters : MonoBehaviour
{
    public static bool pressedBack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pressedBack == true)
        {
            GoToMenu();
        }
    }

    public static void GoToMenu()
    {
        pressedBack = false;
        SceneManager.LoadScene("NolanScene");
    }
}
