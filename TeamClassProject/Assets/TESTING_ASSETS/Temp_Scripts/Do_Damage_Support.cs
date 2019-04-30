using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Do_Damage_Support : MonoBehaviour
{

    public float attack_Damage;
    public bool is_Spike;
    [Header("Variables for GetHit()")]
    public Vector2 attack_Angle;
    [Tooltip("Attack force if eels touch player")]
    public float attack_force = 100f;
    [Tooltip("How long player will be stunned for")]
    public float hitStun = 0.2f;

    public float screen_Shake_Duration = 0.2f;
    public float screen_Shake_Magnitude = 0.1f;
    public float screen_Shake_Time = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && is_Spike)
        {
            /*float angle = Mathf.Atan2(collision.transform.position.y, collision.transform.position.x);
            if (transform.position.x < collision.transform.position.x)
            {
                collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 45, 100 ,0.2f, 60f, 0.5f, false, 0.1f, 0.3f, 0.2f);
                
            }else if (transform.position.x > collision.transform.position.x)
            {
                //collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 45, 100 ,0.2f, 60f, 0.5f, true, 0.1f, 0.3f, 0.2f);
                Vector2 attack_Angle = collision.gameObject.transform.position - transform.position;
                bool player_Facing_Right = GetComponent<BasePlayer>().FacingRight();
                collision.gameObject.GetComponent<BasePlayer>().GetHit(attack_Damage, attack_Angle, attack_force, hitStun,
                player_Facing_Right, screen_Shake_Duration, screen_Shake_Magnitude, screen_Shake_Time);
            }*/
            bool player_Facing_Right = collision.GetComponent<BasePlayer>().FacingRight();
            collision.gameObject.GetComponent<BasePlayer>().GetHit(attack_Damage, attack_Angle, attack_force, hitStun,
            player_Facing_Right, screen_Shake_Duration, screen_Shake_Magnitude, screen_Shake_Time);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !is_Spike)
        {
            //collision.GetComponent<BasicPlayerScript>().GetHit(attack_Damage, 0, 0,0,0,0, true, 0,0,0);
            collision.GetComponent<BasePlayer>().currentHealth -= attack_Damage;
        }
    }
}
