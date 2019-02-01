using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire : MonoBehaviour
{

    public float maxHealth;
    public int speed;
    public float weight;

    PlatformPlayer player;

    private void Awake()
    {
        if(this.GetComponent<PlatformPlayer>() != null)
        {
            player = this.GetComponent<PlatformPlayer>();
            player.maxHealth = maxHealth;
            player.speed = speed;
            player.weight = weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
