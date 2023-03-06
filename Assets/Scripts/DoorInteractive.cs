using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractive : MonoBehaviour
{
    public GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject.Find("Main_Camera_Point").transform.Find("backGround").gameObject.SetActive(true);
            GameObject.Find("Player").transform.Find("Player_light").gameObject.SetActive(true);

            gameManager.sceneNum = 1;
            Debug.Log("YS씬으로 넘긴다? " + gameManager.sceneNum);
            SceneManager.LoadScene("YS");
        }
    }
}
