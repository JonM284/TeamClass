using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJuiceDAir : MonoBehaviour
{
    public GameObject floorjuice;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            
                Debug.Log("hi1");
                GameObject melonDownJ = Instantiate(floorjuice, transform.position, Quaternion.identity);
                Destroy(gameObject);

        }
    }
}
