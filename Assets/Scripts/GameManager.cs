using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;

    //private bool isRunning = true;
    //public bool gameOver = false;

    public int playerHP = 4;
    public int playerAttack = 4;
    private float startUiAlpha = 0.7f;
    public GameObject startUiObj;
    private Image startUi;


    private void Awake()
    {
        startUi = startUiObj.GetComponent<Image>(); //ui 이미지 받아오기
        startUi.color = new Color(0, 0, 0, startUiAlpha);
        Time.timeScale = 0; //버튼 누르기 전까진 시간 멈춤 
    }

    public void startButtonDown()
    {
        /* Color color = startUi.color;
         color.a = 0f;
         startUi.color = color;*/

        Time.timeScale = 1; //버튼 누르면 시간 활성화
        startUiObj.SetActive(false); //스타트 ui 전부 비활성화 시키기

    }

    // 플레이어 체력 관리
    public void HealthDown()
    {
        playerHP--;

        if (playerHP < 1)
        {
            player.OnDie();     // 플레이어 죽음 이펙트

            // 결과 UI
            Debug.Log("사망");

            // Retry 버튼 UI: 사망 2초 후 리스폰으로 대체. 추후 수정.
        }
    }

    // 연속 공격 횟수 관리
    public void AttackCntDown()
    {
        playerAttack--;
    }
}
