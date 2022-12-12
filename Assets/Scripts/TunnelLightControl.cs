using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class TunnelLightControl : MonoBehaviour
{
    public TunnelControl tunnel;
    public InteractiveObject InterOb;

    public bool states = false;

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
            lighting.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = false;
            Debug.Log("���� off");
        }
        else if (tunnel.Tun == false && states == false)
        {
            GameObject.Find("Light_Parent").transform.Find("Light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
            states = true;
            Debug.Log("���� on");
        }
    }
}