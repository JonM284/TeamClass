using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireUlt : MonoBehaviour
{

    public float duration;
    public float damage;

    public GameObject thisPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = thisPlayer.transform.position;
    }

    IEnumerator DestroyUlt()
    {
        yield return new WaitForSeconds(duration);
        thisPlayer.GetComponent<Claire>().ultActive = false;
        thisPlayer.GetComponent<Claire>().hasStorm = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject != thisPlayer)
        {
            collision.gameObject.GetComponent<BasicPlayerScript>().currentHealth -= damage;
            Debug.Log("I'm hitting something");
        }
    }

}
