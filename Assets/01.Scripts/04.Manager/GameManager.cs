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
    //public float[] wallX;// 스테이지별 벽 X좌표 맨 앞과 마지막 벽도 포함
    public List<float> stageX; //스테이지 X좌표 
    public int enemyNum = 4; //스테이지 동적 생성으로 인해 갯수를 정적으로 정할 수 없음. 고정된 수여야함 
    public int currentStageEnemy = 0;   // 현재 남은 적 수
    public int currentStage = 0;        // 현재 스테이지
    public bool isOpened = false;       // 스테이지의 문이 열렸는가?
   

    private void Awake()
    {
        startUi = startUiObj.GetComponent<Image>(); //startUi 이미지 받아오기
        endUi = endUiObj.GetComponent<Image>(); //endUi 이미지 받아오기
        endUi.color = new Color(0, 0, 0, startUiAlpha); //endUI는 미리 이미지 설정
        endUiObj.SetActive(false); //게임 시작할 때는 endUi 비활성화

        for (int stage = 0; stage < platGenerator.platformList.Count; stage++)
        {
            stageX.Add(platGenerator.platformList[stage].transform.position.x - PlatformGenerator.platInterval / 2 + PlatformGenerator.WallInterval);
            wallX.Add(platGenerator.platformList[stage].transform.Find("Wall").gameObject);
            if(stage == platGenerator.platformList.Count - 1)
            {
                //마지막 스테이지에는 벽하나 더 있어서 그것도 붙이기
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
        Time.timeScale = 0; //버튼 누르기 전까진 시간 멈춤
        currentStage = 1;
        currentStageEnemy = enemyNum;
        isOpened = false;
   
        
       
    }

    private void Update()
    {
        //마지막 스테이지면 끝내기 
        if(currentStage == platGenerator.platformList.Count + 1)
        {
            
            endUiObj.SetActive(true);
            Time.timeScale = 0;
        }

        //스테이지 지나면 현재 스테이지랑 현재 스테이지 적 숫자 업데이트
        if (currentStage<= platGenerator.platformList.Count && player.transform.position.x > (stageX[currentStage - 1] + PlatformGenerator.platInterval - PlatformGenerator.WallInterval * 2))
        {

            currentStage += 1;
            currentStageEnemy = enemyNum;
            isOpened = true;
        }
        

    }

    public void startButtonDown()
    {

        startUiObj.SetActive(false); //스타트 ui 전부 비활성화 시키기
        TextManager.onTM = true; //텍스트 매니저 스타트
    }

    public void reStartButtonDown()
    {    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //씬 리로드
    }

    // 플레이어 체력 관리
    public void HealthDown()
    {
        playerHP--;

        if (playerHP == 0) //0일 때 딱 한번만 죽게함( HP < 0 하면 안의 코드가 여러번 재생됨) 
        {
            Debug.Log(playerHP);
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
