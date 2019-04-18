using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Follower : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cam.WorldToScreenPoint(new Vector3(target.position.x + offset.x,
                target.position.y + offset.y, target.position.z + offset.z));
    }
}
