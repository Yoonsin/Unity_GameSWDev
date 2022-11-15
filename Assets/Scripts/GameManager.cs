using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;

    //private bool isRunning = true;
    //public bool gameOver = false;

    public int playerHP = 4;
    public int playerAttack = 4;

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
