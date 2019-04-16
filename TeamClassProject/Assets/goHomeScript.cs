using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class goHomeScript : MonoBehaviour
{
    public void resGame()
    {
        SceneManager.LoadScene("NolanScene");
    }
}
