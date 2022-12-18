using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;

    public int playerHP = 4;
    public int playerAttack = 4;
    private float startUiAlpha = 0.7f;
    public GameObject startUiObj;
    private Image startUi;

    public float[] wallX = new float[5];// ���������� �� X��ǥ �� �հ� ������ ���� ����
    public float[] childX = new float[4]; //���������� ������ X��ǥ 
    public int[] enemyNum = new int[3] { 2, 2, 2 };  // ���������� ���� ��
    public int currentStageEnemy = 0;   // ���� ���� �� ��
    public int currentStage = 0;        // ���� ��������
    public bool isOpened = false;       // ���������� ���� ���ȴ°�?

    private void Awake()
    {
        startUi = startUiObj.GetComponent<Image>(); //ui �̹��� �޾ƿ���
    }

    private void Start()
    {
        playerHP = 4;
        playerAttack = 4;
        startUiAlpha = 0.7f;
        startUi.color = new Color(0, 0, 0, startUiAlpha);
        Time.timeScale = 0; //��ư ������ ������ �ð� ����
        currentStage = 1;
        currentStageEnemy = enemyNum[currentStage - 1];
        isOpened = false;
        // �� ��ǥ �ʱ�ȭ
        for (int i = 0; i < 5; i++)
        {
            wallX[i] = GameObject.Find("Wall").transform.GetChild(i).position.x;
            Debug.Log("wall " + wallX[i]);
        }
    }

    public void startButtonDown()
    {
        /* Color color = startUi.color;
         color.a = 0f;
         startUi.color = color;*/

        //Time.timeScale = 1; //��ư ������ �ð� Ȱ��ȭ
        startUiObj.SetActive(false); //��ŸƮ ui ���� ��Ȱ��ȭ ��Ű��
        TextManager.onTM = true; //�ؽ�Ʈ �Ŵ��� ��ŸƮ

    }

    // �÷��̾� ü�� ����
    public void HealthDown()
    {
        playerHP--;

        if (playerHP == 0) //0�� �� �� �ѹ��� �װ���( HP < 0 �ϸ� ���� �ڵ尡 ������ �����) 
        {
            Debug.Log(playerHP);
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
