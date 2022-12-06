using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLightControl : MonoBehaviour
{
    public TunnelControl tunnel;
    public InteractiveObject InterOb;

    private bool states = false;

    private Transform lighting;

    void Start()
    {
        InterOb = GameObject.Find("Spike").GetComponent<InteractiveObject>();
        lighting = GameObject.Find("Light_Parent").transform.Find("Light");
    }


    void Update()
    {
        if (tunnel.Tun == true && states == true && InterOb.trigger == true)
        {
            GameObject.Find("Light_Parent").transform.Find("Light").gameObject.SetActive(true);// 조명 오브젝트 활성화
            states = false;
            Debug.Log("조명 on");
        }
        else if (tunnel.Tun == false && states == false)
        {
            lighting.gameObject.SetActive(false);// 조명 오브젝트 비활성화
            states = true;
            Debug.Log("조명 off");
        }
    }
}
