using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;

    public PlatformGenerator platGenerator;

    public TextManager textManager;

    public int playerHP = 4;
    public int playerAttack = 4;
    private float startUiAlpha = 0.7f;
    public GameObject startUiObj;
    public GameObject endUiObj;
    private Image startUi;
    private Image endUi;

    public List<GameObject> wallX;
    //public float[] wallX;// ���������� �� X��ǥ �� �հ� ������ ���� ����
    public List<float> stageX; //�������� X��ǥ 
    public int enemyNum = 4; //�������� ���� �������� ���� ������ �������� ���� �� ����. ������ �������� 
    public int currentStageEnemy = 0;   // ���� ���� �� ��
    public int currentStage = 0;        // ���� ��������
    public bool isOpened = false;       // ���������� ���� ���ȴ°�?
   

    private void Awake()
    {
        startUi = startUiObj.GetComponent<Image>(); //startUi �̹��� �޾ƿ���
        endUi = endUiObj.GetComponent<Image>(); //endUi �̹��� �޾ƿ���
        endUi.color = new Color(0, 0, 0, startUiAlpha); //endUI�� �̸� �̹��� ����
        endUiObj.SetActive(false); //���� ������ ���� endUi ��Ȱ��ȭ

        for (int stage = 0; stage < platGenerator.platformList.Count; stage++)
        {
            stageX.Add(platGenerator.platformList[stage].transform.position.x - PlatformGenerator.platInterval / 2 + PlatformGenerator.WallInterval);
            wallX.Add(platGenerator.platformList[stage].transform.Find("Wall").gameObject);
            if(stage == platGenerator.platformList.Count - 1)
            {
                //������ ������������ ���ϳ� �� �־ �װ͵� ���̱�
                wallX.Add(platGenerator.platformList[stage].transform.Find("Wall(Clone)").gameObject);
            }
        }
    }

    private void Start()
    {
        playerHP = 4;
        playerAttack = 4;
        startUiAlpha = 0.7f;
        startUi.color = new Color(0, 0, 0, startUiAlpha);
        Time.timeScale = 0; //��ư ������ ������ �ð� ����
        currentStage = 1;
        currentStageEnemy = enemyNum;
        isOpened = false;
   
        
       
    }

    private void Update()
    {
        //������ ���������� ������ 
        if(currentStage == platGenerator.platformList.Count + 1)
        {
            
            endUiObj.SetActive(true);
            Time.timeScale = 0;
        }

        //�������� ������ ���� ���������� ���� �������� �� ���� ������Ʈ
        if (currentStage<= platGenerator.platformList.Count && player.transform.position.x > (stageX[currentStage - 1] + PlatformGenerator.platInterval - PlatformGenerator.WallInterval * 2))
        {

            currentStage += 1;
            currentStageEnemy = enemyNum;
            isOpened = true;
        }
        

    }

    public void startButtonDown()
    {

        startUiObj.SetActive(false); //��ŸƮ ui ���� ��Ȱ��ȭ ��Ű��
        TextManager.onTM = true; //�ؽ�Ʈ �Ŵ��� ��ŸƮ
    }

    public void reStartButtonDown()
    {    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //�� ���ε�
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
