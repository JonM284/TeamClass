using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectScript : MonoBehaviour
{
    public EventSystem ES;

    public Button Stage1;
    public Button Stage2;
    public Button Stage3;
    public Button Stage4;
    public Button Random;
    public Button Back;

    public int selected;

    // Start is called before the first frame update
    void Start()
    {
        ES = GetComponent<EventSystem>();
        selected = 1;
    }

    // Update is called once per frame
    void Update()
    {
       //if (ES.currentSelectedGameObject == Stage1){

       //}
    }
}
