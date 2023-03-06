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
    public float cameraSetZ = -10; //��� ������Ʈ�κ��� �󸶳� x,y,z ��ǥ��ŭ ī�޶� ������ ���� ���� �ʱ� ����

    public float cameraSpeed = 6; //ī�޶� �����̴� �ӵ�

    Vector3 cameraPosition;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition.y = player.transform.position.y + cameraSetY; //ĳ���� ���� �ٸ� ��浵 ���� �ö󰡴� ���� ����, �ʱ⿡�� y��ǥ �������ְ� ���� X �ʿ�, ����� �ӽ÷� �ۼ�
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
        if(player.GetComponent<PlayerMove>().isShaked==false) //ī�޶� ��鸱 ���� ī�޶� �÷��̾� ���󰡴°� ��� ���߱� 
        {
            pos = player.transform.position; //�÷��̾� ��ġ �޾ƿ���
            pos.x += 1;
            pos.y = 0;
            pos.z = -10;
            this.transform.position = pos; //ī�޶� ��ġ�� �÷��̾� ��ġ �־ �÷��̾ ���� ī�޶� �̵������ֱ�
        }
        */

    }
}
