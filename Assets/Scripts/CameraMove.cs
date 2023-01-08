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
        if(player.GetComponent<PlayerMove>().isShaked==false) //카메라가 흔들릴 때는 카메라가 플레이어 따라가는거 잠시 멈추기 
        {
            pos = player.transform.position; //플레이어 위치 받아오기
            pos.x += 1;
            pos.y = 0;
            pos.z = -10;
            this.transform.position = pos; //카메라 위치에 플레이어 위치 넣어서 플레이어에 따라 카메라 이동시켜주기
        }
        
        
    }
}
