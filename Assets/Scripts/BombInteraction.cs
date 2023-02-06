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
       
        //Debug.LogFormat("{0}, {1}", gameManager.currentStageEnemy, bombtrigger);
        // ��ź ��ü�ϰ� �� ���� �� �Ѿ��. �� �ݴ� �� �÷��̾��� Update���� ��.
        if (trigger == true)
        {
            trigger = false; 
            player.interactiveObject = null;
            
            // �� ����
            Debug.Log("������ ��: " + gameManager.wallX[gameManager.currentStage]);
            gameManager.wallX[gameManager.currentStage].SetActive(false);
            //gameManager.isOpened = true;

            // ��ź ��ü 
            if (gameManager.currentStage == 4) //���� ������������ ��ź ��ü�ϸ�
                player.isFinalBomb = true; //��ź ��ü �ߴ�!
            
            this.gameObject.SetActive(false); //��ź �̵� �������� �Ⱥ��̰� �ϱ�

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
