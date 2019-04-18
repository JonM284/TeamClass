using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_BouncePad : MonoBehaviour
{

    public GameObject myMushroomBounce;
    //public Transform myEndPos;
    public Vector3 myStartPos;
    public bool MushroomBounce_Active = false;
    private bool hasStarted = false;
    //public float going_Out_Speed, return_Speed;
    //public int damageToDealToPlayer;
    //public SpriteRenderer[] appleSprites;
    public SpriteRenderer mushroomBounceSprite;
    public BoxCollider2D[] mushroomBounceColliders;
    public GameObject mushroomBouncePrefab;

    public float removeMushroomBounceTimer, removeMushroomBounceLength;
    public bool MushroomBounceAreThere;

    [SerializeField] private float bounceForce;

    private Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        removeMushroomBounceTimer = 0f;
        removeMushroomBounceLength = 4f;
        MushroomBounceAreThere = false;
        myStartPos = myMushroomBounce.transform.position;
        //mushroomBounceSprite = myMushroomBounce.GetComponent<SpriteRenderer>();
        mushroomBounceSprite.enabled = false;
        mushroomBounceColliders = myMushroomBounce.GetComponents<BoxCollider2D>();
        mushroomBouncePrefab.SetActive(false);
        foreach (BoxCollider2D mushroomBounceCollider in mushroomBounceColliders)
        {
            mushroomBounceCollider.enabled = false;
        }
        //myColor = new Color(255, 255, 255, 1);            //this is for bc we dont have sprite yet
        myColor = new Color(255, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses BasicAttack to activate bounce pad
        if (MushroomBounce_Active)
        {
            Activate_MushroomBounce();
        }

        //if the bounce pad is active currently, count timer down til its less than 0, then....
        if (removeMushroomBounceTimer > 0 && MushroomBounceAreThere == true)
        {
            removeMushroomBounceTimer -= Time.deltaTime;
            if(removeMushroomBounceTimer < 1)
            {
                myColor.a -= .02f;
                mushroomBounceSprite.color = myColor;
            }
        }
        //...disable bounce pad and reset its position for next time machine is used
        if (removeMushroomBounceTimer <= 0 && MushroomBounceAreThere == true)
        {
            Deactivate_MushroomBounce();
            myColor.a = 1f;
            mushroomBounceSprite.color = myColor;
        }


    }

    //the function done to activate bounce pad
    public void Activate_MushroomBounce()
    {

    }

    //the function to deactivate the bounce pad / reset
    public void Deactivate_MushroomBounce()
    {
        MushroomBounce_Active = false;
        MushroomBounceAreThere = false;
        //mushroomBounceSprite.enabled = false;
        foreach (BoxCollider2D mushroomBounceCollider in mushroomBounceColliders)
        {
            mushroomBounceCollider.enabled = false;
        }
        mushroomBouncePrefab.SetActive(false);
        myMushroomBounce.transform.position = myStartPos;
    }

    //when hit a player send them upwards
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Bounce");
            //bounce the player up HERE
            other.gameObject.GetComponent<BasicPlayerScript>().velocity.y = other.gameObject.GetComponent<BasicPlayerScript>().jumpVel * bounceForce;
        }
    }

}

