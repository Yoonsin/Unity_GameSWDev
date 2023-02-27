using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteraction : InteractiveParent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (trigger == true)
        {
            transform.Find("switch_0").gameObject.SetActive(false);
            transform.Find("switch_1").gameObject.SetActive(true);
           
        }
        else if (trigger == false)
        {
            transform.Find("switch_0").gameObject.SetActive(true);
            transform.Find("switch_1").gameObject.SetActive(false);
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
