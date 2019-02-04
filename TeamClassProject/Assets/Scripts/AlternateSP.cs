using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class AlternateSP : MonoBehaviour
{

    //public variables
    public float speed;
    public int playerNum, teamID;


    //private variables
    private enum Status { Free, AtMachine };
    private Rigidbody2D rb;
    private Vector2 vel;
    private float horizontalInput;
    private Player myPlayer;
    [SerializeField]
    private bool is_In_Area = false;
    Status status;
    private GameObject myMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myMachine = null;
        status = Status.Free;
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (status == Status.Free) {
            horizontalInput = myPlayer.GetAxisRaw("Horizontal");
        }else
        {
            horizontalInput = 0;
        }

        if (is_In_Area && myPlayer.GetButtonDown("Jump"))
        {
            if (status == Status.Free)
            {
                status = Status.AtMachine;
                if (!myMachine.GetComponent<MachineBehaviour>().is_In_Use) {
                    myMachine.GetComponent<MachineBehaviour>().Commence_Control(playerNum, teamID);
                }
                Debug.Log(status);
            }else if (status == Status.AtMachine)
            {
                status = Status.Free;
                if (myMachine.GetComponent<MachineBehaviour>().is_In_Use)
                {
                    myMachine.GetComponent<MachineBehaviour>().End_Control();
                    Debug.Log("Has detached from machine with Jump");
                }
                
                Debug.Log(status);
            }
        }else if (is_In_Area && myPlayer.GetButtonDown("HeavyAttack"))
        {
            if (myMachine.GetComponent<MachineBehaviour>().is_In_Use)
            {
                myMachine.GetComponent<MachineBehaviour>().End_Control();
                myMachine = null;
                Debug.Log("Has detached from machine with Heavy Attack");
            }
            if (status == Status.AtMachine)
            {
                status = Status.Free;
                Debug.Log(status);
            }

        }

        



    }

    private void FixedUpdate()
    {
        if (horizontalInput < -0.1f || horizontalInput > 0.1f)
        {
            Movement();
        }
    }

    public void SetFree()
    {
        status = Status.Free;
    }

    void Movement()
    {
        vel.x = horizontalInput * speed;
        vel.y = 0;

        rb.MovePosition(rb.position + Vector2.ClampMagnitude(vel, speed) * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Machine")
        {
            is_In_Area = true;
            myMachine = other.gameObject;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Machine")
        {           
            myMachine = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Machine")
        {
            is_In_Area = false;
            myMachine = null;
        }
    }


    ///rewired functions
    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(myPlayer);
    }

    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4
            switch (playerNum)
            {
                case 4:
                    ds4.SetLightColor(Color.yellow);
                    break;
                case 3:
                    ds4.SetLightColor(Color.green);
                    break;
                case 2:
                    ds4.SetLightColor(Color.blue);
                    break;
                case 1:
                    ds4.SetLightColor(Color.red);
                    break;
                default:
                    ds4.SetLightColor(Color.white);
                    Debug.LogError("Player Num is 0, please change to a number >0");
                    break;
            }


        }
    }
}
