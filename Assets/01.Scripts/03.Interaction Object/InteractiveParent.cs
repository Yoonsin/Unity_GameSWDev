using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class InteractiveParent : MonoBehaviour
{
    protected GameManager gameManager;
    protected PlayerMove player;
    protected TunnelControl tunnel;
    

    public float[] nextPosX = new float[4]; // 스테이지 넘어갈 때마다 옮길 위치
    public bool trigger;

    // 부모가 가진 변수 초기화. 자식에서 다시 한 번 실행할 것.
    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        trigger = false;

    }

    // 부모의 Update에서는 할 것이 없다. 자식에서 수행할 일은 반드시 자식에서 써줄 것.
    protected abstract void Update();

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + this.name);
        
        var type = this.GetType(); //상호작용 물체 파악
        if(type == typeof(BombInteraction))
        {
            //폭탄은 적 다 죽임 -> 폭탄 해체 트리거 -> 문열림 시퀀스여야 함
            //이유: 적이 남아있는데 트리거 바로 true 되고 적 죽이면 문이 저절로 열리는 모양새라 어색함..
            if(gameManager.currentStageEnemy == 0)
            {
                trigger = true;
            }
        }
        else
        {
            trigger = true;
        }

    }

    // 충돌 판정은 자식이 자기자신을 전달해야 하므로 자식에 단다.
}
