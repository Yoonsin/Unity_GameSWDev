using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemyPrefab;

    bool isnotSpawned = false; // 스폰이 안 됐는가?

    // Start is called before the first frame update
    void Start()
    {
        isnotSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isnotSpawned)
        {
            isnotSpawned = false;
            enemySpawn();
        }
    }

    // 적 스폰
    void enemySpawn()
    {
        for (int stage = 0; stage < 2; stage++)
        {
            for (int spawned = 0; spawned < gameManager.enemyNum[stage]; spawned++)
            {
                //Debug.Log("stage " + stage + " / spawned " + spawned);
                //Debug.Log("min " + (gameManager.wallX[stage] + 5.0f));
                //Debug.Log("max " + (gameManager.wallX[stage + 1] - 3.0f));
                float x = Random.Range(gameManager.wallX[stage] + 5.0f, gameManager.wallX[stage + 1] - 3.0f);
                //Debug.Log("x: " + x);
                Instantiate(enemyPrefab).transform.position = new Vector2(x, -0.5f);
            }
        }
    }
}
