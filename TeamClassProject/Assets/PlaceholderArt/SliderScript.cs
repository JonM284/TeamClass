using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    public GameObject menuSlider;

    // Start is called before the first frame update
    void Start()
    {
        menuSlider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            menuSlider.SetActive(true);
        }
    }
}
