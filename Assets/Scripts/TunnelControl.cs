using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TunnelControl : MonoBehaviour
{
    public bool Tun = false;

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
        Debug.Log("�ͳ� ����");
        Tun = true;
        light.color = Color.black;
        Invoke("TurnOff", 5);
    }

    void TurnOff()
    {
        Debug.Log("�ͳ� ����");
        Tun = false;
        light.color = Color.white;
        Invoke("TurnOn", 10);
    }
}
