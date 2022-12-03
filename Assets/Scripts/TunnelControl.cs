using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TunnelControl : MonoBehaviour
{
    public TunnelLightControl TunnelLight;

    private bool state;
    public bool Tun = false;

    Light2D light; // 2D light 추가

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
        Tun = true;
        light.color = Color.black; // 글로벌 라이트 검은색
        Invoke("TurnOff", 5);

        
    }

    void TurnOff()
    {
        Debug.Log("터널 퇴장");
        Tun = false;
        light.color = Color.white; // 글로벌 라이트 흰색
        Invoke("TurnOn", 10);
    }
}
