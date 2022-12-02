using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TunnelControl : MonoBehaviour
{
    private bool state;

    Light2D light;

    void Start()
    {
        light = GetComponentInChildren<Light2D>();
        Invoke("TurnOn", 3);
    }

    
    void Update()
    {
        
    }

    void TurnOn()
    {
        Debug.Log("터널 입장");
        light.color = Color.black;
        Invoke("TurnOff", 5);

        
    }

    void TurnOff()
    {
        Debug.Log("터널 퇴장");
        light.color = Color.white;
        Invoke("TurnOn", 10);
    }
}
