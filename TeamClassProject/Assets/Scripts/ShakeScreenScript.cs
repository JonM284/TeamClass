using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreenScript : MonoBehaviour
{

    [Header("Camera")]
    private float shakeDuration = 0;
    private float shakeMagnitude = 0;
    private float shakeSlowDown = 0;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
    }

    public void SetVariables(float duration, float magnitude, float slowDown)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeSlowDown = slowDown;

    }

    private void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * shakeSlowDown;
        }
        else
        {
            shakeDuration = 0;
            transform.localPosition = initialPos;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
