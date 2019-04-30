using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Cannon_Ball : MonoBehaviour
{

    public ParticleSystem explosion_Effect;
    public float wait_Time;

    [Tooltip("Attack force if eels touch player")]
    public Vector2 attack_Angle;
    public float attack_force = 100f;
    [Tooltip("How long player will be stunned for")]
    public float hitStun = 0.2f;
    public float attack_Damage = 250f;
    public float screen_Shake_Duration = 0.2f;
    public float screen_Shake_Magnitude = 0.1f;
    public float screen_Shake_Time = 0.2f;

    private void OnEnable()
    {
        this.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Turn_On());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
           //other.gameObject.GetComponent<BasicPlayerScript>().GetHit(250f, angle, 100, 0.2f, 75f, 0.5f, true, 0.1f, 0.3f, 0.2f);

            bool player_Facing_Right = other.GetComponent<BasePlayer>().FacingRight();
            other.gameObject.GetComponent<BasePlayer>().GetHit(attack_Damage, attack_Angle, attack_force, hitStun,
            player_Facing_Right, screen_Shake_Duration, screen_Shake_Magnitude, screen_Shake_Time);
        }
    }

    IEnumerator Turn_On()
    {

        yield return new WaitForSeconds(wait_Time);
        this.GetComponent<Collider2D>().enabled = true;
        explosion_Effect.Play();
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1.5f); 
        explosion_Effect.Stop();
        StartCoroutine(waitToShutOff());
    }


    IEnumerator waitToShutOff()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
