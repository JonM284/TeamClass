using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireUlt : MonoBehaviour
{

    public int damage;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyUlt());
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
            collision.GetComponent<BasicPlayerScript>().currentHealth -= damage;
        }
    }

}
