using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Cannon_Behaviour : MonoBehaviour
{
    public bool Is_Facing_Right;


    public void Do_Flash()
    {
        StartCoroutine(flash_Control());
    }

    IEnumerator flash_Control()
    {

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponentInParent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponentInParent<SpriteRenderer>().color = Color.white;
        }

    }
}
