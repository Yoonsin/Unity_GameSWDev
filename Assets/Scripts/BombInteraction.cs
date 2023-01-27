using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInteraction : InteractiveParent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // 폭탄 해체하고 문 열고 문 넘어가기. 문 닫는 건 플레이어의 Update에서 함.
        if (gameManager.currentStageEnemy == 0 && trigger == true)
        {
            trigger = false;
            player.interactiveObject = null;
            // 문 열기
            Debug.Log("삭제할 벽: " + GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).name);
            GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).gameObject.SetActive(false);
            gameManager.isOpened = true;

            // 폭탄 해체 
            if (gameManager.currentStage == 4) //보스 스테이지에서 폭탄 해체하면
            {
                player.isFinalBomb = true; //폭탄 해체 했다!
                this.gameObject.SetActive(false); //폭탄 이동 하지말고 안보이게 하기

            }
            
            {
                this.transform.position = new Vector3(nextPosX[gameManager.currentStage], this.transform.position.y, this.transform.position.z);
                GameObject.Find("Switches").transform.position = new Vector3(GameObject.Find("Switches").GetComponent<SwitchInteraction>().nextPosX[gameManager.currentStage], GameObject.Find("Switches").transform.position.y, GameObject.Find("Switches").transform.position.z);
            }

            //this.transform.GetChild(gameManager.currentStage - 1).gameObject.SetActive(false);
        }
        else
        {
            trigger = false;
        }
    }

    // 플레이어가 상호작용 물체의 충돌 범위에 들어왔을 때 플레이어에게 알림
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = this;
            transform.Find("InteractiveMark").gameObject.SetActive(true);
        }
    }

    // 플레이어가 상호작용 물체의 충돌 범위에서 벗어났을 때 플레이어에게 알림
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = null;
            transform.Find("InteractiveMark").gameObject.SetActive(false);
        }
    }
}
