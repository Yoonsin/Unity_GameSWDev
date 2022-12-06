using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public PlayerMove player;
    
    public bool trigger;


    void Start()
    {
        
    }

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + gameObject.name);
        trigger = true;
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
