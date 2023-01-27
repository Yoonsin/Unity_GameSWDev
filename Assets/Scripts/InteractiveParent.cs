using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveParent : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
    public TunnelControl tunnel;

    public float[] nextPosX = new float[4]; // 스테이지 넘어갈 때마다 옮길 위치
    public bool trigger = false;

    // 부모가 가진 변수 초기화. 자식에서 다시 한 번 실행할 것.
    protected virtual void Start()
    {
        //nextPosX = { 0, 0, 0, 0 };
        trigger = false;
    }

    // 부모의 Update에서는 할 것이 없다. 자식에서 수행할 일은 반드시 자식에서 써줄 것.
    protected abstract void Update();

    // 상호작용 시 수행
    public void Interaction()
    {
        Debug.Log("상호작용 성공: " + this.name);
        trigger = true;
    }

    // 충돌 판정은 자식이 자기자신을 전달해야 하므로 자식에 단다.
}
