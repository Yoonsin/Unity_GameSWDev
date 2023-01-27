using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
    public TunnelControl tunnel;

    public float[] bombX = new float[4]; //BombUnit의 X 좌표 (스테이지 넘어갈 때마다 위치 옮겨줄 예정)
    public float[] switchX = new float[4]; //switch의 X 좌표 (스테이지 넘어갈 때마다 위치 옮겨줄 예정)
    public bool trigger = false;
    public bool Btrigger = false;

    void Start()
    { 
        //bombX = { 12.1 , 70.6, };
    }

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + this.name);
        trigger = true;
        Btrigger = true;
    }

    private void Update()
    {
        if (this.name == "Switches")
        {
            if (trigger == true)
            {
                GameObject.Find("Switches").transform.Find("switch_0").gameObject.SetActive(false);
                GameObject.Find("Switches").transform.Find("switch_1").gameObject.SetActive(true);
            }
            else if (trigger == false)
            {
                GameObject.Find("Switches").transform.Find("switch_0").gameObject.SetActive(true);
                GameObject.Find("Switches").transform.Find("switch_1").gameObject.SetActive(false);
            }
        }
        if (this.name == "BombUnit" && gameManager.currentStageEnemy == 0)
        {
            // 폭탄 해체하고 문 열고 문 넘어가기. 문 닫는 건 플레이어의 Update에서 함.
            if (Btrigger == true)
            {
                Btrigger = false;
                player.interactiveObject = null;
                // 문 열기
                Debug.Log("삭제할 벽: " + GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).name);
                GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).gameObject.SetActive(false);
                gameManager.isOpened = true;
              
                // 폭탄 해체 
                if (gameManager.currentStage == 4) //보스 스테이지에서 폭탄 해체하면
                {
                    player.isFinalBomb = true ; //폭탄 해체 했다!
                    this.gameObject.SetActive(false); //폭탄 이동 하지말고 안보이게 하기
                    
                }
                else
                {
                    this.transform.position = new Vector3(bombX[gameManager.currentStage], this.transform.position.y, this.transform.position.z);
                    GameObject.Find("Switches").transform.position = new Vector3(switchX[gameManager.currentStage], GameObject.Find("Switches").transform.position.y, GameObject.Find("Switches").transform.position.z);
                }
                
                //this.transform.GetChild(gameManager.currentStage - 1).gameObject.SetActive(false);
            }
            Btrigger = false;
        }
        else if (this.name == "BombUnit")
        {
            Btrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //player.interactiveObject = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = null;
        }
    }
}