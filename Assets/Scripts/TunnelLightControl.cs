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
            GameObject.Find("Light_parent").transform.Find("Light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
            states = true;
            Debug.Log("���� on");
        }
        else if(tunnel.Tun == false && states == true)
        {
            lighting.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = false;
            Debug.Log("���� off");
            
        }
    }
}
