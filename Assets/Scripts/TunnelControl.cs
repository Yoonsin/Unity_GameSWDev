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
        Debug.Log("터널 입장");
        Invoke("TurnOff", 5);
    }

    void TurnOff()
    {
        gameObject.SetActive(true);
        state = true;
        Debug.Log("터널 퇴장");
        Invoke("TurnOn", 10);
    }
}
