using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class TunnelLightControl : MonoBehaviour
{
    public TunnelControl tunnel;
    public InteractiveParent InterOb;

    public bool states = false;

    private Transform lighting;

    void Start()
    {
        states = false;
        lighting = GameObject.Find("Light_Parent").transform.Find("Light");
        lighting.gameObject.SetActive(false);
        InterOb = GameObject.Find("Switches").GetComponent<SwitchInteraction>();
}


    void Update()
    {
        if (tunnel.Tun == false && InterOb.trigger == true)
        {
            lighting.gameObject.SetActive(false);// 조명 오브젝트 비활성화
            states = false;
            //Debug.Log("조명 off");
        }
        else if (tunnel.Tun == true && InterOb.trigger == false)
        {
            GameObject.Find("Light_Parent").transform.Find("Light").gameObject.SetActive(true);// 조명 오브젝트 활성화
            states = true;
            //Debug.Log("조명 on");
        }
        else if (tunnel.Tun == false && states == true)
        {
            lighting.gameObject.SetActive(false);// 조명 오브젝트 비활성화
            states = false;
            //Debug.Log("조명 off");
        }
    }
}