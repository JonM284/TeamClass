using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnomercy : MonoBehaviour
{
    [Header("Gnomercy Values")]
    public float maxHealth;
    public int speed;
    public float weight;

    BasicPlayer player;

    private void Awake()
    {
        if(this.GetComponent<BasicPlayer>() != null)
        {
            player = this.GetComponent<BasicPlayer>();
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
