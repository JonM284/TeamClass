using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireUlt : MonoBehaviour
{

    public float duration;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyUlt()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }

}
