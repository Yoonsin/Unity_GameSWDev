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
        // ��ź ��ü�ϰ� �� ���� �� �Ѿ��. �� �ݴ� �� �÷��̾��� Update���� ��.
        if (gameManager.currentStageEnemy == 0 && trigger == true)
        {
            trigger = false;
            player.interactiveObject = null;
            // �� ����
            Debug.Log("������ ��: " + GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).name);
            GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).gameObject.SetActive(false);
            gameManager.isOpened = true;

            // ��ź ��ü 
            if (gameManager.currentStage == 4) //���� ������������ ��ź ��ü�ϸ�
            {
                player.isFinalBomb = true; //��ź ��ü �ߴ�!
                this.gameObject.SetActive(false); //��ź �̵� �������� �Ⱥ��̰� �ϱ�

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

    // �÷��̾ ��ȣ�ۿ� ��ü�� �浹 ������ ������ �� �÷��̾�� �˸�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = this;
            transform.Find("InteractiveMark").gameObject.SetActive(true);
        }
    }

    // �÷��̾ ��ȣ�ۿ� ��ü�� �浹 �������� ����� �� �÷��̾�� �˸�
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = null;
            transform.Find("InteractiveMark").gameObject.SetActive(false);
        }
    }
}
