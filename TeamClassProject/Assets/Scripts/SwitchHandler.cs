using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchHandler : MonoBehaviour
{

    public int teamNumber;

    [Header("Player 1 or 3 Character Dump")]
    public GameObject P1x3_ClaireFighter;
    public GameObject P1x3_ClaireSupport;
    public GameObject P1x3_GilbertFighter;
    public GameObject P1x3_GilbertSupport;
    [Header("Player 2 or 4 Character Dump")]
    public GameObject P2x4_ClaireFighter;
    public GameObject P2x4_ClaireSupport;
    public GameObject P2x4_GilbertFighter;
    public GameObject P2x4_GilbertSupport;

    [HideInInspector]
    public GameObject teammate1_fighter;
    [HideInInspector]
    public GameObject teammate1_support;
    [HideInInspector]
    public GameObject teammate2_fighter;
    [HideInInspector]
    public GameObject teammate2_support;

    [Header("Visual Effects")]
    public GameObject switchSmoke;

    [Header("UI")]

    public Image[] specialMeter = new Image[3];

    Vector3 teammate1_fighterPos;
    Vector3 teammate1_supportPos;
    Vector3 teammate2_fighterPos;
    Vector3 teammate2_supportPos;

    public float teammate1_timer = 0;
    public float teammate2_timer = 0;

    int teammate1_num;
    int teammate2_num;

    float regenHealthPool = 0;

    [HideInInspector]
    public float currentUltNum = 0;
    float maxUltNum = 300;

    Color origBarColor;

    public Image clairePortrait;
    public Image gilbertPortrait;

    [Header("Audio")]
    public AudioClip[]swapSounds;
    public AudioSource swapSoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        P1x3_ClaireFighter.SetActive(false);
        P1x3_ClaireSupport.SetActive(false);
        P1x3_GilbertFighter.SetActive(false);
        P1x3_GilbertSupport.SetActive(false);
        P2x4_ClaireFighter.SetActive(false);
        P2x4_ClaireSupport.SetActive(false);
        P2x4_GilbertFighter.SetActive(false);
        P2x4_GilbertSupport.SetActive(false);

        //setting AudioSource
        swapSoundPlayer = gameObject.GetComponent<AudioSource>();


        if (teamNumber == 1)
        {
            //player 1
            if (PlayerPrefs.GetInt("Player1Character") == 1)
            {
                teammate1_fighter = P1x3_ClaireFighter;
                teammate1_support = P1x3_ClaireSupport;
            }
            if (PlayerPrefs.GetInt("Player1Character") == 2)
            {
                teammate1_fighter = P1x3_GilbertFighter;
                teammate1_support = P1x3_GilbertSupport;
            }

            //player 2
            if (PlayerPrefs.GetInt("Player2Character") == 1)
            {
                teammate2_fighter = P2x4_ClaireFighter;
                teammate2_support = P2x4_ClaireSupport;
            }
            if (PlayerPrefs.GetInt("Player2Character") == 2)
            {
                teammate2_fighter = P2x4_GilbertFighter;
                teammate2_support = P2x4_GilbertSupport;
            }
        }

        if (teamNumber == 2)
        {
            //player 3
            if (PlayerPrefs.GetInt("Player3Character") == 1)
            {
                teammate1_fighter = P1x3_ClaireFighter;
                teammate1_support = P1x3_ClaireSupport;
            }
            if (PlayerPrefs.GetInt("Player3Character") == 2)
            {
                teammate1_fighter = P1x3_GilbertFighter;
                teammate1_support = P1x3_GilbertSupport;
            }

            //player 4
            if (PlayerPrefs.GetInt("Player4Character") == 1)
            {
                teammate2_fighter = P2x4_ClaireFighter;
                teammate2_support = P2x4_ClaireSupport;
            }
            if (PlayerPrefs.GetInt("Player4Character") == 2)
            {
                teammate2_fighter = P2x4_GilbertFighter;
                teammate2_support = P2x4_GilbertSupport;
            }
        }



        teammate1_num = teammate1_fighter.GetComponent<BasicPlayerScript>().playerNum;
        teammate2_num = teammate2_fighter.GetComponent<BasicPlayerScript>().playerNum;

        teammate1_fighter.SetActive(true);
        teammate1_support.SetActive(false);

        teammate2_fighter.SetActive(false);
        teammate2_support.SetActive(true);

        teammate1_fighter.GetComponent<BasicPlayerScript>().teamNum = teamNumber;
        teammate2_fighter.GetComponent<BasicPlayerScript>().teamNum = teamNumber;


        if (SceneManager.GetActiveScene().name.Equals("JonScene"))
        {
            Debug.Log("jon");
            teammate1_support.GetComponent<AlternateSP>().teamNum = teamNumber;
            teammate2_support.GetComponent<AlternateSP>().teamNum = teamNumber;
        }

        if (SceneManager.GetActiveScene().name.Equals("JustinScene"))
        {
            Debug.Log("justin");
            teammate1_support.GetComponent<AlternateSP2>().teamNum = teamNumber;
            teammate2_support.GetComponent<AlternateSP2>().teamNum = teamNumber;
        }



        origBarColor = specialMeter[0].color;

        for(int i = 0; i < 3; i++)
        {
            specialMeter[i].fillAmount = 0;
        }

        /*
        if (teammate1_fighter.activeSelf)
        {
            if (teammate1_fighter.GetComponent<BasicPlayerScript>().claire)
            {
                clairePortrait.enabled = true;
                gilbertPortrait.enabled = false;
            }
            else
            if (teammate1_fighter.GetComponent<BasicPlayerScript>().gillbert)
            {
                clairePortrait.enabled = false;
                gilbertPortrait.enabled = true;
            }
        }
        else
        {
            if (teammate2_fighter.GetComponent<BasicPlayerScript>().claire)
            {
                clairePortrait.enabled = true;
                gilbertPortrait.enabled = false;
            }
            else
            if (teammate2_fighter.GetComponent<BasicPlayerScript>().gillbert)
            {
                clairePortrait.enabled = false;
                gilbertPortrait.enabled = true;
            }
        }
        */
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (teammate1_fighter.activeSelf)
        {
            if (teammate1_fighter.GetComponent<BasicPlayerScript>().claire)
            {
                clairePortrait.enabled = true;
                gilbertPortrait.enabled = false;
            }
            else
            if (teammate1_fighter.GetComponent<BasicPlayerScript>().gillbert)
            {
                clairePortrait.enabled = false;
                gilbertPortrait.enabled = true;
            }
        }
        else
        {
            if (teammate2_fighter.GetComponent<BasicPlayerScript>().claire)
            {
                clairePortrait.enabled = true;
                gilbertPortrait.enabled = false;
            }
            else
            if (teammate2_fighter.GetComponent<BasicPlayerScript>().gillbert)
            {
                clairePortrait.enabled = false;
                gilbertPortrait.enabled = true;
            }
        }



        UltBar();

        regenHealthPool += Time.deltaTime * 20;

        teammate1_timer -= Time.deltaTime;
        teammate2_timer -= Time.deltaTime;

        if (teammate1_fighter.activeSelf == true)
        {
            teammate1_fighterPos = teammate1_fighter.transform.position;

            swapSoundPlayer.clip = swapSounds[0];
            swapSoundPlayer.Play();
        }
        if (teammate1_support.activeSelf == true)
        {
            teammate1_supportPos = teammate1_support.transform.position;
        }
        if (teammate2_fighter.activeSelf == true)
        {
            teammate2_fighterPos = teammate2_fighter.transform.position;

            swapSoundPlayer.clip = swapSounds[0];
            swapSoundPlayer.Play();

        }
        if (teammate2_fighter.activeSelf == true)
        {
            teammate2_supportPos = teammate2_support.transform.position;
        }

        if (teammate1_timer > 0 && teammate2_timer > 0)
        {
            if (specialMeter[0].fillAmount >= .2f)
            {
                currentUltNum -= 100;

                teammate2_timer = 0;
                teammate1_timer = 0;

                if (teammate1_fighter.activeSelf)
                {
                    Instantiate(switchSmoke, teammate1_fighterPos, Quaternion.identity);
                    Instantiate(switchSmoke, teammate2_supportPos, Quaternion.identity);
                    teammate1_fighter.SetActive(false);
                    teammate1_support.SetActive(true);
                    teammate1_support.transform.position = new Vector3(teammate2_supportPos.x, -3.85f, teammate2_supportPos.z);
                }
                else
                {
                    teammate1_fighter.SetActive(true);
                    if (teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth + regenHealthPool >= teammate1_fighter.GetComponent<BasicPlayerScript>().regenHeath)
                    {
                        teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth = teammate1_fighter.GetComponent<BasicPlayerScript>().regenHeath;
                    }
                    else
                    {
                        teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth += regenHealthPool;
                        teammate1_fighter.GetComponent<BasicPlayerScript>().regenHeath = teammate1_fighter.GetComponent<BasicPlayerScript>().currentHealth;
                    }
                    teammate1_fighter.transform.position = new Vector3(teammate2_fighterPos.x, teammate2_fighterPos.y + .3f, teammate2_fighterPos.z);
                    teammate1_support.SetActive(false);

                    //portrait swap
                    if (teammate1_fighter.GetComponent<BasicPlayerScript>().claire)
                    {
                        clairePortrait.enabled = true;
                        gilbertPortrait.enabled = false;
                    }
                    else if (teammate1_fighter.GetComponent<BasicPlayerScript>().gillbert)
                    {
                        clairePortrait.enabled = false;
                        gilbertPortrait.enabled = true;
                    }
                }


                if (teammate2_fighter.activeSelf)
                {
                    Instantiate(switchSmoke, teammate1_supportPos, Quaternion.identity);
                    Instantiate(switchSmoke, teammate2_fighterPos, Quaternion.identity);
                    teammate2_fighter.SetActive(false);
                    teammate2_support.SetActive(true);
                    teammate2_support.transform.position = new Vector3(teammate1_supportPos.x, -3.85f, teammate1_supportPos.z);
                }
                else
                {
                    teammate2_fighter.SetActive(true);
                    if (teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth + regenHealthPool >= teammate2_fighter.GetComponent<BasicPlayerScript>().regenHeath)
                    {
                        teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth = teammate2_fighter.GetComponent<BasicPlayerScript>().regenHeath;
                    }
                    else
                    {
                        teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth += regenHealthPool;
                        teammate2_fighter.GetComponent<BasicPlayerScript>().regenHeath = teammate2_fighter.GetComponent<BasicPlayerScript>().currentHealth;
                    }
                    teammate2_fighter.transform.position = new Vector3(teammate1_fighterPos.x, teammate1_fighterPos.y + .3f, teammate1_fighterPos.z);
                    teammate2_support.SetActive(false);

                    //portrait swap
                    if (teammate2_fighter.GetComponent<BasicPlayerScript>().claire)
                    {
                        clairePortrait.enabled = true;
                        gilbertPortrait.enabled = false;
                    }
                    else if (teammate2_fighter.GetComponent<BasicPlayerScript>().gillbert)
                    {
                        clairePortrait.enabled = false;
                        gilbertPortrait.enabled = true;
                    }
                }
            }
        }
    }

    public void UltBar()
    {
        //ult bars logic
        float temp = currentUltNum;
        for(int i = 0; i < 3; i++)
        {
            if (currentUltNum >= 0) {
                specialMeter[i].fillAmount = temp / 100 * .2f;
                temp -= 100;

                if(specialMeter[i].fillAmount >= .2f)
                {
                    specialMeter[i].color = new Color(1, 248.0f/255.0f, 151.0f/255.0f);
                }
                else
                {
                    specialMeter[i].color = origBarColor;
                }
            }
            else
            {
                break;
            }
            
        }
    }

    public void UpdateUltBar(float damage)
    {
        currentUltNum += damage * .39f;

        if(currentUltNum > maxUltNum)
        {
            currentUltNum = maxUltNum;
        }
    }

    public void BeginSwap(int playerNum)
    {
        if(playerNum == teammate1_num)
        {
            teammate1_timer = .5f;
        }
        else
        {
            if (playerNum == teammate2_num)
            {
                teammate2_timer = .5f;
            }
        }
    }
}
