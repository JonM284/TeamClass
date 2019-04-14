using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Spores : MonoBehaviour
{
    public GameObject myMushroomSpores;
    //public Transform myEndPos;
    public Vector3 myStartPos;
    public bool MushroomSpores_Active = false;
    private bool hasStarted = false;
    //public float going_Out_Speed, return_Speed;
    //public int damageToDealToPlayer;
    //public SpriteRenderer[] appleSprites;
    public SpriteRenderer mushroomSporesSprite;
    public BoxCollider2D[] mushroomSporesColliders;

    public float removeMushroomSporesTimer, removeMushroomSporesLength;
    public bool MushroomSporesAreThere;
    public ParticleSystem sporesParticles;

    private Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        removeMushroomSporesTimer = 0f;
        removeMushroomSporesLength = 4f;
        MushroomSporesAreThere = false;
        myStartPos = myMushroomSpores.transform.position;
        mushroomSporesSprite = myMushroomSpores.GetComponent<SpriteRenderer>();
        mushroomSporesSprite.enabled = false;
        mushroomSporesColliders = myMushroomSpores.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D mushroomSporesCollider in mushroomSporesColliders)
        {
            mushroomSporesCollider.enabled = false;
        }
        //damageToDealToPlayer = 100;
        //myColor = new Color(255, 255, 255, 1);            //this is for bc we dont have sprite yet
        myColor = new Color(196, 0, 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to activate spores shroom
        if (MushroomSpores_Active)
        {
            Activate_MushroomSpores();

        }


        //if the spores shroom is on map currently, count timer down til its less than 0, then....
        if (removeMushroomSporesTimer > 0 && MushroomSporesAreThere == true)
        {
            removeMushroomSporesTimer -= Time.deltaTime;
            if (removeMushroomSporesTimer > 1f)
            {
                sporesParticles.Play();
            }
            if(removeMushroomSporesTimer < 1)
            {
                myColor.a -= 0.02f;
                mushroomSporesSprite.color = myColor;
            }
        }
        if(removeMushroomSporesTimer <= 1f && sporesParticles.isPlaying == true)
        {
            sporesParticles.Stop();
        }
        //...disable spores shroom and reset its position for next time machine is used
        if (removeMushroomSporesTimer <= 0 && MushroomSporesAreThere == true)
        {
            Deactivate_MushroomSpores();
            myColor.a = 1f;
            mushroomSporesSprite.color = myColor;
        }
    }

    //the function activates shroom spores
    public void Activate_MushroomSpores()
    {

    }

    //the function that happens to reset spores shroom
    public void Deactivate_MushroomSpores()
    {
        MushroomSpores_Active = false;
        MushroomSporesAreThere = false;
        mushroomSporesSprite.enabled = false;
        foreach (BoxCollider2D mushroomSporesCollider in mushroomSporesColliders)
        {
            mushroomSporesCollider.enabled = false;
        }
        myMushroomSpores.transform.position = myStartPos;
    }

    //when player is touching spores it damages player constantly
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("damage by spores");
            //Deal Damage to the player HERE
            other.gameObject.GetComponent<BasicPlayerScript>().GetHit(1f, 0, 0, 0, 0, 0, other.gameObject.GetComponent<BasicPlayerScript>().FacingRight(), 0, 0, 0);
        }
    }
}
