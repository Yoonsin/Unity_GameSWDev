using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=HPhJngP695Y ����
//https://www.youtube.com/watch?v=KUQAULcpYZU �ٵ� ī�޶� �����̰� �����ϸ� �����?
//��ϱ� ī�޶� �������� ���� ��濡 �� �����ָ� �Ǵ°� �ƴ�?
//ī�޶�� ��� �и��ؼ� �ذ�! -2023.1.9-

public class RepeatBG_Re : MonoBehaviour
{

    [SerializeField][Range(0.01f, 0.5f)] float BGspeed = 0.5f; //���� �����̴� �ӵ�(�� 0.3f������ �����ѵ�)
 
    Vector3 startPos; //ȭ�� ��ũ�� �� �� ó���� ����Ʈ ��ǥ
    Vector3 ansPos; //ȭ�� ��ũ�ѽ� ����ؼ� ���������� ������Ʈ ������ ����Ʈ ��ǥ
    Vector3 defaultPos; //������� �ǵ��� �ֱ� ���� �⺻ ����Ʈ ��ǥ

    float posValue; //ȭ�� ��ĭ �Ÿ��� x�� (����Ʈ ��ǥ ����)
    float newPos; //ȭ�� ��ĭ �Ÿ���ŭ ������� x�� ���ݾ� ������ �� 

    bool repeatflag = false; //�ѹ� ȭ�� ��ĭ ��ũ�� �� �� �ٽ� ó�� ����Ʈ ��ǥ�� �޾��ָ� �ʱ�ȭ �ǹǷ� �����ϱ� ���� bool ����
    

    // Start is called before the first frame update
    void Start()
    {
        float BGspeed = 0.5f;
        repeatflag = false;
        defaultPos = Camera.main.WorldToViewportPoint(this.transform.position);
        // https://www.youtube.com/watch?v=icYDYFFBVCc �Ŀ� ���� ī�޶� ������ �������ִ� �ڵ� start�� �߰��ϱ�
    }

    // Update is called once per frame
    void Update()
    {
        
        //���� ī�޶� ���ؼ� ���� ������Ʈ�� ����Ʈ�� ���ϰ�,
        //�� ����Ʈ x��  -x ������ �Ű� �ִ� ������ ���� ��ǥ�� �ݿ��� �����ϸ� ��.

        if(repeatflag == false) //��ĭ ���ư� �������� �����̴°� �ѹ��� �ϰ� ��ٸ���(startPos ������ �־��ָ� �ȵ�)
        {
            startPos = Camera.main.WorldToViewportPoint(this.transform.position); //���� ���� ����Ʈ ��ǥ ���ϱ�
            //startPos.x = 0f; //���� ����Ʈ�� �߾ӿ� �����ϱ� �� �������� ���� ���� ���(�� ���� ��� ũ�⿡ ���� ������ �� �־��..)
            posValue = startPos.x + 1f; //���� ���濡�� ȭ�� ��ĭ ������ �� ��ġ��
        }

        //46��°�� 54��° �� -�� ���� ���ʿ��� ���������� �̵���
        newPos = -(Mathf.Repeat((Time.time/BGspeed)  + startPos.x, posValue)); //���� x ������ ���������� ȭ�� ��ĭ �̵��� x������ �ݺ�
        ansPos = startPos; ///���� ��ǥ �ֱ�
        ansPos.x = newPos;
        //ansPos.x += newPos; //���� ��ǥ�� + ��� �������� �̵��ϴ� x�� �ֱ�
        this.transform.position = Camera.main.ViewportToWorldPoint(ansPos); //������ ��ǥ�� ����Ʈ ��ǥ�ϱ� �̰� �ٽ� ������ǥ�� �������� �ֱ�
        //Debug.Log(ansPos.x);
        repeatflag = true; //ȭ�� ��ĭ �ִ°� ��ٸ���

        if (-(ansPos.x) > posValue) //���� ȭ�� ��ĭ �з� �̻� ���ư��ٸ�
        {
            this.transform.position = Camera.main.ViewportToWorldPoint(defaultPos); // �ٽ� ���� ����Ʈ ��ǥ �־��ֱ�
            repeatflag = false; //�ٽ� ��ĭ ���ư� �� �ֵ��� �ϱ�
        }

        //�Ʒ��� ������^^
        //ansPos = startPos; //�ٽ� �������� ���� ����Ʈ ��ǥ �ֱ�
        /*posValue = startPos.x;

        newPos = -(Mathf.Repeat(Time.time * BGspeed, posValue));

        //ansPos = startPos + Vector3.left * newPos;*/

    }


}
