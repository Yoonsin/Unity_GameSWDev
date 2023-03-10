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
        Tun = false;
        light = GetComponentInChildren<Light2D>();
<<<<<<< Updated upstream:Assets/Scripts/TunnelControl.cs
=======
        //InterOb = GameObject.Find("Switchs").GetComponent<SwitchInteraction>();
>>>>>>> Stashed changes:Assets/01.Scripts/03.Interaction Object/TunnelControl.cs
        Invoke("TurnOn", 5);
    }

    
    void Update()
    {
        
    }

    void TurnOn()
    {
        Debug.Log("터널 입장");
        Tun = true;
        light.color = Color.black;
        Invoke("TurnOff", 5);
    }

    void TurnOff()
    {
        Debug.Log("터널 퇴장");
        Tun = false;
<<<<<<< Updated upstream:Assets/Scripts/TunnelControl.cs
        InterOb.trigger = false;
=======
        //InterOb.trigger = false; //자동으로 스위치 꺼짐
>>>>>>> Stashed changes:Assets/01.Scripts/03.Interaction Object/TunnelControl.cs
        light.color = Color.white;
        Invoke("TurnOn", 5);
        
    }
}
