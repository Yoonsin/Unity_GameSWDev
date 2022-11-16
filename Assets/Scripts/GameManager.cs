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
        startUi = startUiObj.GetComponent<Image>(); //ui �̹��� �޾ƿ���
        startUi.color = new Color(0, 0, 0, startUiAlpha);
        Time.timeScale = 0; //��ư ������ ������ �ð� ���� 
    }

    public void startButtonDown()
    {
        /* Color color = startUi.color;
         color.a = 0f;
         startUi.color = color;*/

        Time.timeScale = 1; //��ư ������ �ð� Ȱ��ȭ
        startUiObj.SetActive(false); //��ŸƮ ui ���� ��Ȱ��ȭ ��Ű��

    }

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
