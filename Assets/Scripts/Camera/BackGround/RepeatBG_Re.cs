using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=HPhJngP695Y 참고
//https://www.youtube.com/watch?v=KUQAULcpYZU 근데 카메라 움직이게 설정하면 배경어떡함?
//어떡하긴 카메라 움직임을 여기 배경에 다 더해주면 되는거 아님?
//카메라와 배경 분리해서 해결! -2023.1.9-

public class RepeatBG_Re : MonoBehaviour
{

    [SerializeField][Range(0.01f, 0.5f)] float BGspeed = 0.5f; //원경 움직이는 속도(한 0.3f정도가 적당한듯)
 
    Vector3 startPos; //화면 스크롤 할 때 처음의 뷰포트 좌표
    Vector3 ansPos; //화면 스크롤시 계속해서 오른쪽으로 업데이트 시켜줄 뷰포트 좌표
    Vector3 defaultPos; //원래대로 되돌려 주기 위한 기본 뷰포트 좌표

    float posValue; //화면 한칸 거리의 x값 (뷰포트 좌표 기준)
    float newPos; //화면 한칸 거리만큼 닿기위해 x에 조금씩 더해줄 값 

    bool repeatflag = false; //한번 화면 한칸 스크롤 할 때 다시 처음 뷰포트 좌표를 받아주면 초기화 되므로 방지하기 위한 bool 변수
    

    // Start is called before the first frame update
    void Start()
    {
        float BGspeed = 0.5f;
        repeatflag = false;
        defaultPos = Camera.main.WorldToViewportPoint(this.transform.position);
        // https://www.youtube.com/watch?v=icYDYFFBVCc 후에 원경 카메라에 꽉차게 매핑해주는 코드 start에 추가하기
    }

    // Update is called once per frame
    void Update()
    {
        
        //먼저 카메라를 통해서 원경 오브젝트의 뷰포트를 구하고,
        //그 뷰포트 x를  -x 값까지 옮겨 주는 과정을 월드 좌표로 반영해 적용하면 됨.

        if(repeatflag == false) //한칸 나아갈 때까지는 움직이는거 한번만 하고 기다리기(startPos 새값을 넣어주면 안됨)
        {
            startPos = Camera.main.WorldToViewportPoint(this.transform.position); //현재 원경 뷰포트 좌표 구하기
            //startPos.x = 0f; //현재 뷰포트가 중앙에 있으니까 맨 왼쪽으로 시작 기준 잡기(이 값은 배경 크기에 따라 수정될 수 있어요..)
            posValue = startPos.x + 1f; //현재 원경에서 화면 한칸 오른쪽 띈 위치값
        }

        //46번째랑 54번째 줄 -만 빼면 왼쪽에서 오른쪽으로 이동함
        newPos = -(Mathf.Repeat((Time.time/BGspeed)  + startPos.x, posValue)); //기존 x 값에서 오른쪽으로 화면 한칸 이동한 x값까지 반복
        ansPos = startPos; ///시작 좌표 넣기
        ansPos.x = newPos;
        //ansPos.x += newPos; //시작 좌표에 + 계속 왼쪽으로 이동하는 x값 넣기
        this.transform.position = Camera.main.ViewportToWorldPoint(ansPos); //움직인 좌표는 뷰포트 좌표니까 이거 다시 월드좌표로 움직여서 넣기
        //Debug.Log(ansPos.x);
        repeatflag = true; //화면 한칸 넣는거 기다리기

        if (-(ansPos.x) > posValue) //만약 화면 한칸 분량 이상 나아간다면
        {
            this.transform.position = Camera.main.ViewportToWorldPoint(defaultPos); // 다시 원래 뷰포트 좌표 넣어주기
            repeatflag = false; //다시 한칸 나아갈 수 있도록 하기
        }

        //아래는 삽질들^^
        //ansPos = startPos; //다시 적용해줄 원경 뷰포트 좌표 넣기
        /*posValue = startPos.x;

        newPos = -(Mathf.Repeat(Time.time * BGspeed, posValue));

        //ansPos = startPos + Vector3.left * newPos;*/

    }


}
