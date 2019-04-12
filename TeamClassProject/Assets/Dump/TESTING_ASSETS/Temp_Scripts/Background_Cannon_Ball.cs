using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Cannon_Ball : MonoBehaviour
{

    public ParticleSystem explosion_Effect;
    public float wait_Time;

    private void OnEnable()
    {
        this.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Turn_On());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(250f, angle, 0, 0.2f, 20f, 1.5f, true, 0.1f, 0.3f, 0.2f);
        }
    }

    IEnumerator Turn_On()
    {

        yield return new WaitForSeconds(wait_Time);
        this.GetComponent<Collider2D>().enabled = true;
        explosion_Effect.Play();
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Collider2D>().enabled = false;
        explosion_Effect.Stop();
        StartCoroutine(waitToShutOff());
    }


    IEnumerator waitToShutOff()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
