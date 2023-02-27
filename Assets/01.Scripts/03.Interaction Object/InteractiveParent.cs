using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class InteractiveParent : MonoBehaviour
{
    protected GameManager gameManager;
    protected PlayerMove player;
    protected TunnelControl tunnel;
    

    public float[] nextPosX = new float[4]; // �������� �Ѿ ������ �ű� ��ġ
    public bool trigger;

    // �θ� ���� ���� �ʱ�ȭ. �ڽĿ��� �ٽ� �� �� ������ ��.
    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        trigger = false;

    }

    // �θ��� Update������ �� ���� ����. �ڽĿ��� ������ ���� �ݵ�� �ڽĿ��� ���� ��.
    protected abstract void Update();

    // ��ȣ�ۿ� �� ����
    public void Interaction()
    {
        Debug.Log("��ȣ�ۿ� ����: " + this.name);
        
        var type = this.GetType(); //��ȣ�ۿ� ��ü �ľ�
        if(type == typeof(BombInteraction))
        {
            //��ź�� �� �� ���� -> ��ź ��ü Ʈ���� -> ������ ���������� ��
            //����: ���� �����ִµ� Ʈ���� �ٷ� true �ǰ� �� ���̸� ���� ������ ������ ������ �����..
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

    // �浹 ������ �ڽ��� �ڱ��ڽ��� �����ؾ� �ϹǷ� �ڽĿ� �ܴ�.
}
