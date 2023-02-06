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
