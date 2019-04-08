using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitHolder : MonoBehaviour
{
    public Animator team_anim;

    // Start is called before the first frame update
    void Start()
    {
        team_anim = GetComponent<Animator>();
        team_anim.SetInteger("animState", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStageSelect.hasSelected == true)
        {
            team_anim.SetInteger("animState", 1);
        }

        if (PlayerStageSelect.hasSelected == false)
        {
            team_anim.SetInteger("animState", 2);
        }
    }
}
