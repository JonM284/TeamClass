using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MachineBehaviour : MonoBehaviour
{

    public bool is_In_Use = false;
    public GameObject Controlled_Hazzard;
    public float speed;

    private float horizontalInput, verticalInput;
    
    public enum MachineID {SideCannon, BackgroundCannon, SideHazard, MovingPlatform, SpecialPlatform};
    public MachineID mach;
    private Vector2 vel;


    private Player myPlayer;

    private ObjectSpawner objectPool;

    

    // Start is called before the first frame update
    void Start()
    {
        objectPool = ObjectSpawner.Instance;
        if (mach == MachineID.BackgroundCannon)
        {
            Controlled_Hazzard.GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazzard.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is_In_Use) {
            if (mach == MachineID.SideCannon)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                SideCannonMovement();
            } else if (mach == MachineID.BackgroundCannon)
            {
                verticalInput = myPlayer.GetAxisRaw("Vertical");
                horizontalInput = myPlayer.GetAxisRaw("Horizontal");
                BackgroundCannonMovement();
            }
            /*else if (mach == MachineID.SideHazard)
            {

            }else if (mach == MachineID.MovingPlatform)
            {

            }else if (mach == MachineID.SpecialPlatform)
            {

            }*/
        }
    }

    void SideCannonMovement()
    {
        vel.y = verticalInput * speed;

        Controlled_Hazzard.GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazzard.GetComponent<Rigidbody2D>().position 
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }

    void BackgroundCannonMovement()
    {
        vel.x = horizontalInput * speed;
        vel.y = verticalInput * speed;

        if (myPlayer.GetButtonDown("Jump"))
        {
            objectPool.SpawnFromPool("Tester", Controlled_Hazzard.transform.position, Quaternion.identity);
            End_Control();
        }

        Controlled_Hazzard.GetComponent<Rigidbody2D>().MovePosition(Controlled_Hazzard.GetComponent<Rigidbody2D>().position
            + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }

    public void Commence_Control(int playerNum, int teamID)
    {
        is_In_Use = true;
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        
        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazzard.SetActive(true);
            switch (teamID)
            {
                case 2:
                    Controlled_Hazzard.GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 1:
                    Controlled_Hazzard.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Controlled_Hazzard.GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }
        }
        Debug.Log("Player:"+playerNum+ " has activated cannon:"+mach);
    }

    public void End_Control()
    {
        is_In_Use = false;
        myPlayer = ReInput.players.GetPlayer(-1);
        
        if (mach == MachineID.BackgroundCannon) {
            Controlled_Hazzard.GetComponent<SpriteRenderer>().color = Color.white;
            Controlled_Hazzard.SetActive(false);
        }
        
        Debug.Log("Player has deactivated machine: "+transform.name);
    }
    
}
