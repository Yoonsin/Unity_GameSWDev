using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLightControl : MonoBehaviour
{
    private bool states;

    void Start()
    {
        states = false;
        Invoke("LightTurnOn", 3);
    }


    void Update()
    {

    }

    void LightTurnOn()
    {
        gameObject.SetActive(true);
        states = true;
        Debug.Log("조명 on");
        Invoke("LightTurnOff", 5);
    }

    void LightTurnOff()
    {
        gameObject.SetActive(false);
        states = false;
        Debug.Log("조명 off");
        Invoke("LightTurnOn", 10);
    }
}
