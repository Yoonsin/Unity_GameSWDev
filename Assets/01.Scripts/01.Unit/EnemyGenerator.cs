using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public PlatformGenerator platGenerator;
    

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
        for (int stage = 0; stage < platGenerator.platformList.Count; stage++)
        {

            //스테이지 n의 양쪽 끝 X좌표
            float stageLeftX = gameManager.stageX[stage];
            float stageRightX = stageLeftX + PlatformGenerator.platInterval - PlatformGenerator.WallInterval * 2;


            if (stage == platGenerator.platformList.Count - 1)
            {
                //보스 스테이지

                float bossX = Random.Range(stageLeftX, stageRightX);
                GameObject boss = Instantiate(bossPrefab);
                boss.transform.position = new Vector2(bossX, -0.7f);
            }
            else
            {
                //나머지 스테이지

                for (int spawned = 0; spawned < gameManager.enemyNum; spawned++)
                {

                    float enemyX = Random.Range(stageLeftX, stageRightX);
                    GameObject enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = new Vector2(enemyX, -0.7f);

                }
            }
        }
    }
}
