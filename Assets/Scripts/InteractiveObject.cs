using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public PlayerMove player;
    public TunnelControl tunnel;
    
    public bool trigger = false;


    void Start()
    {
        
    }

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + gameObject.name);
        trigger = true;
    }

    public void Update()
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
