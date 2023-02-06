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
    

    bool isnotSpawned = false; // ������ �� �ƴ°�?

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

    // �� ����
    void enemySpawn()
    {
        for (int stage = 0; stage < platGenerator.platformList.Count; stage++)
        {

            //�������� n�� ���� �� X��ǥ
            float stageLeftX = gameManager.stageX[stage];
            float stageRightX = stageLeftX + PlatformGenerator.platInterval - PlatformGenerator.WallInterval * 2;


            if (stage == platGenerator.platformList.Count - 1)
            {
                //���� ��������

                float bossX = Random.Range(stageLeftX, stageRightX);
                GameObject boss = Instantiate(bossPrefab);
                boss.transform.position = new Vector2(bossX, -0.7f);
            }
            else
            {
                //������ ��������

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
