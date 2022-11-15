using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public PlayerMove player;

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = this;
            transform.Find("Interactive Mark").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.interactiveObject = null;
            transform.Find("Interactive Mark").gameObject.SetActive(false);
        }
    }
}
