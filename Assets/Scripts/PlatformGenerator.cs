using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    //�÷��� ���� 
    public int platformNum = 0;

    //�÷��� ����
    public GameObject platform;

    //�÷��� �ν��Ͻ� ����Ʈ
    public List<GameObject> platformList;

    //�÷��� �� ����
    public const float platInterval = 51.3f;

    //�÷��� �� ����
    public const float WallInterval = 0.8f;

    //�÷��� ��
    public GameObject wall;
    GameObject finalWall;
    

    // Start is called before the first frame update
    void Awake()
    {
        //�÷��̾� ��ġ �ľ�.
        Vector2 playerPos = GameObject.Find("Player").transform.position;
        Vector2 platformPos = playerPos + new Vector2(21, 2.3f);
        float platformX = 0;

        //�÷��̾� ������ �������� �÷��� n�� ����.
        platformList = new List<GameObject>();
        for(int i = 0; i < platformNum; i++)
        {
            platformList.Add(Instantiate(platform));
            platformList[i].transform.position = platformPos + new Vector2(platformX,0);
            platformX += platInterval;

            Debug.LogFormat("�÷��� {0} �� X ���� : {1} ~ {2} ", i + 1, platformList[i].transform.position.x - platInterval / 2 + WallInterval, (platformList[i].transform.position.x - platInterval / 2 + WallInterval) - WallInterval * 2 + platInterval) ;
        }

        //������ ùĭ�� �������� ��� ����.
        platformList[0].transform.Find("Stage").gameObject.SetActive(false);

        //������ ������ĭ�� ������ ���� ������ �߰������ ��. (��ħ������ ������ �ȿ� ���� �� ���ִ°� �ƴ϶� ���� �� �ۿ� ����)
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
           //�÷��̾� ��ġ�� ���� ���° ������������ �ľ�.
        }

       
    }
}
