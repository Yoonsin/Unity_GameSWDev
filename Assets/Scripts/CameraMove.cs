using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector3 pos;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(player.GetComponent<PlayerMove>().isShaked==false) //ī�޶� ��鸱 ���� ī�޶� �÷��̾� ���󰡴°� ��� ���߱� 
        {
            pos = player.transform.position; //�÷��̾� ��ġ �޾ƿ���
            pos.x += 1;
            pos.y = 0;
            pos.z = -10;
            this.transform.position = pos; //ī�޶� ��ġ�� �÷��̾� ��ġ �־ �÷��̾ ���� ī�޶� �̵������ֱ�
        }
        
        
    }
}
