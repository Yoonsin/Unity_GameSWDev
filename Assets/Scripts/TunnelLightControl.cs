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
            GameObject.Find("Light_Parent").transform.Find("Light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
            states = false;
            Debug.Log("���� on");
        }
        else if (tunnel.Tun == false && states == false)
        {
            lighting.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = true;
            Debug.Log("���� off");
        }
    }
}
