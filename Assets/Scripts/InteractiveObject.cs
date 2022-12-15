using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
    public TunnelControl tunnel;
    
    public bool trigger = false;
    public bool Btrigger = false;

    void Start()
    {
        
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
        if (this.name == "Switchs")
        {
            if (trigger == true)
            {
                GameObject.Find("Switchs").transform.Find("switch_0").gameObject.SetActive(false);
                GameObject.Find("Switchs").transform.Find("switch_1").gameObject.SetActive(true);
            }
            else if (trigger == false)
            {
                GameObject.Find("Switchs").transform.Find("switch_0").gameObject.SetActive(true);
                GameObject.Find("Switchs").transform.Find("switch_1").gameObject.SetActive(false);
            }
        }
        if (this.name == "BombUnit" && gameManager.currentStageEnemy == 0)
        {
            // 폭탄 해체하고 문 열고 문 넘어가기. 문 닫는 건 플레이어의 Update에서 함.
            if (Btrigger == true)
            {
                // 폭탄 해체
                this.gameObject.SetActive(false);
                Btrigger = false;
                player.interactiveObject = null;

                // 문 열기
                //Debug.Log("삭제할 벽: " + GameObject.Find("Wall").transform.GetChild(gamemanager.currentStage).name);
                if (GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).name != "FinalWall")   // 마지막 벽은 부수면 안 됨
                {
                    GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).gameObject.SetActive(false);
                    gameManager.isOpened = true;
                }
            }
            Btrigger = false;
        } else if (this.name == "BombUnit")
        {
            Btrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = this;
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
