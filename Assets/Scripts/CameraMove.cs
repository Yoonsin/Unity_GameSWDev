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
        player = GameObject.Find("Player"); //플레이어 게임 오브젝트 찾아서 반환
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        
        pos = player.transform.position; //플레이어 위치 받아오기
        pos.x += 5;
        pos.y = 0;
        pos.z = -10;
        this.transform.position = pos; //카메라 위치에 플레이어 위치 넣어서 플레이어에 따라 카메라 이동시켜주기
        
    }
}
