using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauserScript : MonoBehaviour
{

     public Canvas pPanel;
     public Canvas canv;

     void Start()
     {
        pPanel.enabled = false;
     }


     void Update()
     {

         print("1GoGo");
         if (Input.GetKeyDown(KeyCode.Escape))
             {
                 if (pPanel.enabled)
                 {
                     Continuer();
                 }
                 else
                 {
                     Pauser();
                 }
             }
     }
    public void Pauser()
     {
         Time.timeScale = 0;
        pPanel.enabled = true;
        print("Pause!");
        canv.enabled = false;
     }
     public void Continuer()
     {
         Time.timeScale = 1;
        pPanel.enabled = false;
        print("Continue!");
        canv.enabled = true;
    }

}

