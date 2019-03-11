using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightersScreenScript : MonoBehaviour
{
    public EventSystem ES;

    public Button Claire;
    public Button Gilbert;
    public Button Gnomercy;
    public Button Wawa;

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
        if(ES.currentSelectedGameObject == Claire){
            
        }
    }
}
