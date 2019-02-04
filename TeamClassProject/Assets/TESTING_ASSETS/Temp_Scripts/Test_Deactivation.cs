using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Deactivation : MonoBehaviour
{


    private void OnEnable()
    {
        StartCoroutine(waitToShutOff());
    }

    IEnumerator waitToShutOff()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

}
