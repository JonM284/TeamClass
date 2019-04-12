using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Platform_Behaviour : MonoBehaviour
{
    public Animator anim;
    public GameObject fire_Object;
    private bool spike_Out = false;
    public float spike_Time, fire_Time;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fire_Object.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Handle_Animation();
    }

    void Handle_Animation()
    {
        if (spike_Out)
        {
            anim.SetInteger("AnimNum", 1);
        }else
        {
            anim.SetInteger("AnimNum", 0);
        }
    }

    public void Do_Spike()
    {
        spike_Out = true;
        StartCoroutine(Retract_Spike());
    }

    public void Do_Fire()
    {
        fire_Object.SetActive(true);
        StartCoroutine(Stop_Fire());
    }

    IEnumerator Stop_Fire()
    {
        yield return new WaitForSeconds(fire_Time);
        fire_Object.SetActive(false);
    }

    IEnumerator Retract_Spike()
    {
        yield return new WaitForSeconds(spike_Time);
        spike_Out = false;
    }
}
