using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHandler : MonoBehaviour
{

    public int teamNumber;

    public GameObject teammate1_fighter;
    public GameObject teammate1_support;

    public GameObject teammate2_fighter;
    public GameObject teammate2_support;

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

    // Start is called before the first frame update
    void Start()
    {
        teammate1_num = teammate1_fighter.GetComponent<BasicPlayerScript>().playerNum;
        teammate2_num = teammate2_fighter.GetComponent<BasicPlayerScript>().playerNum;

        teammate1_fighter.SetActive(true);
        teammate1_support.SetActive(false);

        teammate2_fighter.SetActive(false);
        teammate2_support.SetActive(true);

        teammate1_fighter.GetComponent<BasicPlayerScript>().teamNum = teamNumber;
        teammate1_support.GetComponent<AlternateSP>().teamNum = teamNumber;
        teammate2_fighter.GetComponent<BasicPlayerScript>().teamNum = teamNumber;
        teammate2_support.GetComponent<AlternateSP>().teamNum = teamNumber;


        origBarColor = specialMeter[0].color;

        for(int i = 0; i < 3; i++)
        {
            specialMeter[i].fillAmount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UltBar();

        regenHealthPool += Time.deltaTime * 20;

        teammate1_timer -= Time.deltaTime;
        teammate2_timer -= Time.deltaTime;

        if (teammate1_fighter.activeSelf == true)
        {
            teammate1_fighterPos = teammate1_fighter.transform.position;
        }
        if (teammate1_support.activeSelf == true)
        {
            teammate1_supportPos = teammate1_support.transform.position;
        }
        if (teammate2_fighter.activeSelf == true)
        {
            teammate2_fighterPos = teammate2_fighter.transform.position;
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
                }


                if (teammate2_fighter.activeSelf)
                {
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
                    specialMeter[i].color = Color.yellow;
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
        currentUltNum += damage * .5f;

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
