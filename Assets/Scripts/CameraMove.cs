using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector3 pos;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); //�÷��̾� ���� ������Ʈ ã�Ƽ� ��ȯ
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        
        pos = player.transform.position; //�÷��̾� ��ġ �޾ƿ���
        pos.x += 5;
        pos.y = 0;
        pos.z = -10;
        this.transform.position = pos; //ī�޶� ��ġ�� �÷��̾� ��ġ �־ �÷��̾ ���� ī�޶� �̵������ֱ�
        
    }
}
