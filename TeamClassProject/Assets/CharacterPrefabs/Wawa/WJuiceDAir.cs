using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJuiceDAir : MonoBehaviour
{
    public GameObject floorjuice;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            try
            {
                Debug.Log("hi1");
                GameObject melonDownJ = Instantiate(floorjuice, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            catch
            {

            }
        }
    }
}
