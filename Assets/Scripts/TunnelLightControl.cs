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
            lighting.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = false;
            //Debug.Log("���� off");
        }
        else if (tunnel.Tun == true && InterOb.trigger == false)
        {
            GameObject.Find("Light_Parent").transform.Find("Light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
            states = true;
            //Debug.Log("���� on");
        }
        else if (tunnel.Tun == false && states == true)
        {
            lighting.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = false;
            //Debug.Log("���� off");
        }
    }
}