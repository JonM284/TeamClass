using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel_Collision : MonoBehaviour
{
    public GameObject my_Parent;
    [Header("Variables for GetHit()")]
    [Tooltip("Attack force if eels touch player")]
    public Vector2 attack_Angle;
    public float attack_force = 100f;
    public float hitStun = 0.2f;
    public float screen_Shake_Duration = 0.2f;
    public float screen_Shake_Magnitude = 0.1f;
    public float screen_Shake_Time = 0.2f;


    private void Start()
    {
        my_Parent = this.transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform") {
            my_Parent.GetComponent<Eel_Movement>().has_Hit_Platform = true;
            my_Parent.GetComponent<Eel_Movement>().Eel_Active = true;
            
        }

        if (other.gameObject.tag == "Player")
        {
            my_Parent.GetComponent<Eel_Movement>().has_Hit_Platform = true;
            my_Parent.GetComponent<Eel_Movement>().Eel_Active = true;
            float angle = Mathf.Atan2(other.transform.position.y, other.transform.position.x);
            bool player_Facing_Right = other.GetComponent<BasePlayer>().FacingRight();
            
            other.gameObject.GetComponent<BasePlayer>().GetHit(75f, attack_Angle, attack_force, hitStun,
                player_Facing_Right, screen_Shake_Duration, screen_Shake_Magnitude, screen_Shake_Time);
        }

        
    }
}
