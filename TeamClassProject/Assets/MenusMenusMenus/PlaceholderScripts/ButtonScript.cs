using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public int buttonID;
    public GameObject selector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        selector.SetActive(true);
    }

    void OnMouseExit()
    {
        selector.SetActive(false);
    }
}
