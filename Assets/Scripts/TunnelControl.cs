using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TunnelControl : MonoBehaviour
{
    public InteractiveObject InterOb;
    
    public bool Tun = false;

    Light2D light;

    void Start()
    {
        InterOb = GameObject.Find("Spike").GetComponent<InteractiveObject>();
        light = GetComponentInChildren<Light2D>();
        Invoke("TurnOn", 3);
    }

    
    void Update()
    {
        
    }

    void TurnOn()
    {
        Debug.Log("터널 입장");
        Tun = true;
        light.color = Color.black;
        Invoke("TurnOff", 3);
    }

    void TurnOff()
    {
        Debug.Log("터널 퇴장");
        Tun = false;
        InterOb.trigger = false;
        light.color = Color.white;
        Invoke("TurnOn", 5);
    }
}
