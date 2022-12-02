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
        Debug.Log("�ͳ� ����");
        light.color = Color.black;
        Invoke("TurnOff", 5);

        
    }

    void TurnOff()
    {
        Debug.Log("�ͳ� ����");
        light.color = Color.white;
        Invoke("TurnOn", 10);
    }
}
