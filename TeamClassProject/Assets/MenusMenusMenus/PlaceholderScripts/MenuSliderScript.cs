using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSliderScript : MonoBehaviour
{

    private Animator anim;
    public int sliderInt;
    public AudioSource menuUp;
    public AudioSource menuDown;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sliderInt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && sliderInt != 1)
        {
            anim.SetInteger("SliderValue", 1);
            menuUp.Play();
            sliderInt = 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && sliderInt != 2)
        {
            anim.SetInteger("SliderValue", 2);
            menuDown.Play();
            sliderInt = 2;
        }
    }
}
