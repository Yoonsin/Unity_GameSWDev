using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSwitch : MonoBehaviour
{
    public PlayerMove player;

    BoxCollider2D detect;
    private Transform switchs;

    void Start()
    {
        detect = GetComponent<BoxCollider2D>();
        switchs = GameObject.Find("Switch").transform.Find("Spike");
    }

    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Switch").transform.Find("Spike").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            switchs.gameObject.SetActive(false);
        }
    }
}
