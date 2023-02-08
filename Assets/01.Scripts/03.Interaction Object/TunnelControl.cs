using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TunnelControl : MonoBehaviour
{
    public InteractiveParent InterOb;
    
    public bool Tun = false;

    Light2D light;

    void Start()
    {
        Tun = false;
        light = GetComponentInChildren<Light2D>();
        InterOb = GameObject.Find("Switchs").GetComponent<SwitchInteraction>();
        Invoke("TurnOn", 5);
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
        InterOb.trigger = false; //�ڵ����� ����ġ ����
        light.color = Color.white;

        Invoke("TurnOn", 5);
        
    }
}
