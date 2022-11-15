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

    // �÷��̾� ü�� ����
    public void HealthDown()
    {
        playerHP--;

        if (playerHP < 1)
        {
            player.OnDie();     // �÷��̾� ���� ����Ʈ

            // ��� UI
            Debug.Log("���");

            // Retry ��ư UI: ��� 2�� �� ���������� ��ü. ���� ����.
        }
    }

    // ���� ���� Ƚ�� ����
    public void AttackCntDown()
    {
        playerAttack--;
    }
}
