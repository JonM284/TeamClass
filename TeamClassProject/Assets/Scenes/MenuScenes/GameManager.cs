using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton Design Pattern
    public static GameManager instance = null;

    /* Ints will check and tell us which scene is currently active
    1 - Main Menu, 2 - Stage Select, 3 - Fighter Select, 4 - Main Fight Scene */
    public static int gameState = 1;

    /* Ints will handle which stage was voted for
    1 - Claire's, 2 - Gilbert's, 3 - Gno's, 4 - Wawa's, 5 - Random */
    public static int Stage_1, Stage_2, Stage_3, Stage_4, Stage_5 = 0;

    /* Ints will handle which stage is selected
    1 - Claire's, 2 - Gilbert's, 3 - Gno's, 4 - Wawa's, 5 - Random */
    public static int stageToLoad = 0;

    /* Ints will handle which character someone is
    1 - Claire, 2 - Gilbert, 3 - Gno, 4 - Wawa */  
    public static int P1_Character, P2_Character, P3_Character, P4_Character = 0;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
        // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Player1Character", 0);
        PlayerPrefs.SetInt("Player2Character", 0);
        PlayerPrefs.SetInt("Player3Character", 0);
        PlayerPrefs.SetInt("Player4Character", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Stage_1 + Stage_2 + Stage_3 + Stage_4 + Stage_5 >= 4)
        {
            //load level with most votes here once everyone voted
        }
    }
}
