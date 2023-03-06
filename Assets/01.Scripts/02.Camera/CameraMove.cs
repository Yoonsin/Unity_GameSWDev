using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    Vector3 pos;
    public GameObject player;


    public float cameraSetX = 0;
    public float cameraSetY = 3;
    public float cameraSetZ = -10; //대상 오브젝트로부터 얼마나 x,y,z 좌표만큼 카메라가 떨어져 있을 건지 초기 설정

    public float cameraSpeed = 6; //카메라 움직이는 속도

    Vector3 cameraPosition;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition.y = player.transform.position.y + cameraSetY; //캐릭터 점프 뛰면 배경도 딸려 올라가는 문제 생김, 초기에만 y좌표 지정해주고 변경 X 필요, 현재는 임시로 작성
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraPosition.x = player.transform.position.x + cameraSetX;

        cameraPosition.z = player.transform.position.z + cameraSetZ;

        transform.position = Vector3.Lerp(transform.position, cameraPosition, cameraSpeed * Time.smoothDeltaTime);
    }

    void LateUpdate()
    {
        /*
        if(player.GetComponent<PlayerMove>().isShaked==false) //카메라가 흔들릴 때는 카메라가 플레이어 따라가는거 잠시 멈추기 
        {
            pos = player.transform.position; //플레이어 위치 받아오기
            pos.x += 1;
            pos.y = 0;
            pos.z = -10;
            this.transform.position = pos; //카메라 위치에 플레이어 위치 넣어서 플레이어에 따라 카메라 이동시켜주기
        }
        */

    }
}
