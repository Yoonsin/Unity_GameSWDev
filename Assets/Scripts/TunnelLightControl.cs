using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TunnelLightControl : MonoBehaviour
{
    public bool states = false;
    public TunnelControl tunnel;
    private Transform lighting;

    void Start()
    {
        //LightTurnOff();
        lighting = GameObject.Find("Light_parent").transform.Find("Light");
    }


    void Update()
    {
        if(tunnel.Tun == true && states == false)
        {
            GameObject.Find("Light_parent").transform.Find("Light").gameObject.SetActive(true);// 조명 오브젝트 활성화
            states = true;
            Debug.Log("조명 on");
        }
        else if(tunnel.Tun == false && states == true)
        {
            lighting.gameObject.SetActive(false);// 조명 오브젝트 비활성화
            states = false;
            Debug.Log("조명 off");
            
        }
    }
}
