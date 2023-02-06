using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    //플랫폼 개수 
    public int platformNum = 0;

    //플랫폼 종류
    public GameObject platform;

    //플랫폼 인스턴스 리스트
    public List<GameObject> platformList;

    //플랫폼 간 간격
    public const float platInterval = 51.3f;

    //플랫폼 벽 간격
    public const float WallInterval = 0.8f;

    //플랫폼 벽
    public GameObject wall;
    GameObject finalWall;
    

    // Start is called before the first frame update
    void Awake()
    {
        //플레이어 위치 파악.
        Vector2 playerPos = GameObject.Find("Player").transform.position;
        Vector2 platformPos = playerPos + new Vector2(21, 2.3f);
        float platformX = 0;

        //플레이어 오른쪽 방향으로 플랫폼 n개 생성.
        platformList = new List<GameObject>();
        for(int i = 0; i < platformNum; i++)
        {
            platformList.Add(Instantiate(platform));
            platformList[i].transform.position = platformPos + new Vector2(platformX,0);
            platformX += platInterval;

            Debug.LogFormat("플랫폼 {0} 의 X 범위 : {1} ~ {2} ", i + 1, platformList[i].transform.position.x - platInterval / 2 + WallInterval, (platformList[i].transform.position.x - platInterval / 2 + WallInterval) - WallInterval * 2 + platInterval) ;
        }

        //프리팹 첫칸은 스테이지 블록 삭제.
        platformList[0].transform.Find("Stage").gameObject.SetActive(false);

        //프리팹 마지막칸은 오른쪽 벽을 별도로 추가해줘야 함. (겹침문제로 프리팹 안에 양쪽 벽 다있는게 아니라 왼쪽 벽 밖에 없음)
        finalWall = Instantiate(wall, new Vector2(platformX, 0), Quaternion.identity);
        finalWall.transform.parent = platformList[platformNum - 1].transform;
        finalWall.transform.localScale = new Vector3(1, 1, 1);
        finalWall.transform.localPosition = new Vector2(5.5f, 0);

        


    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < platformNum; i++)
        {
           //플레이어 위치에 따라 몇번째 스테이지인지 파악.
        }

       
    }
}
