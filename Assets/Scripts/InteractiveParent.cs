using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveParent : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
    public TunnelControl tunnel;

    public float[] nextPosX = new float[4]; // �������� �Ѿ ������ �ű� ��ġ
    public bool trigger = false;

    // �θ� ���� ���� �ʱ�ȭ. �ڽĿ��� �ٽ� �� �� ������ ��.
    protected virtual void Start()
    {
        //nextPosX = { 0, 0, 0, 0 };
        trigger = false;
    }

    // �θ��� Update������ �� ���� ����. �ڽĿ��� ������ ���� �ݵ�� �ڽĿ��� ���� ��.
    protected abstract void Update();

    // ��ȣ�ۿ� �� ����
    public void Interaction()
    {
        Debug.Log("��ȣ�ۿ� ����: " + this.name);
        trigger = true;
    }

    // �浹 ������ �ڽ��� �ڱ��ڽ��� �����ؾ� �ϹǷ� �ڽĿ� �ܴ�.
}
