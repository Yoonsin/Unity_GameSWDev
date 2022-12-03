using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TunnelControl : MonoBehaviour
{
    public TunnelLightControl TunnelLight;

    private bool state;
    public bool Tun = false;

    Light2D light; // 2D light �߰�

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
        light.color = Color.black; // �۷ι� ����Ʈ ������
        Invoke("TurnOff", 5);

        
    }

    void TurnOff()
    {
        Debug.Log("�ͳ� ����");
        Tun = false;
        light.color = Color.white; // �۷ι� ����Ʈ ���
        Invoke("TurnOn", 10);
    }
}
