using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelControl : MonoBehaviour
{
    private bool state;

    void Start()
    {
        state = true;
        Invoke("TurnOn", 3);
    }

    
    void Update()
    {
        
    }

    void TurnOn()
    {
        gameObject.SetActive(false);
        state = false;
        Debug.Log("�ͳ� ����");
        Invoke("TurnOff", 5);
    }

    void TurnOff()
    {
        gameObject.SetActive(true);
        state = true;
        Debug.Log("�ͳ� ����");
        Invoke("TurnOn", 10);
    }
}
