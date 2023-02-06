using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class TunnelLightControl : MonoBehaviour
{
   
    public InteractiveParent InterOb;

    //�ϻ찡�� ����(�ͳ� O & ����ġ O)
    public bool states = false;

    //�� public���� �ؼ� �ν����� â�� �־����� �� �ν��� ����� �ȵ�����..�÷��� ���ʷ�����������Ʈ�� awake���� �������� �����ؼ� �׷���?
    //�÷��� ���ʷ����� ���� �ٸ� ������Ʈ�� start���� �ʱ�ȭ �ϴϱ�, ������ ���� ������Ʈ�� �ν��� �ȵǴ°���
    private TunnelControl tunnel; 
    private GameObject lighting;

    void Start()
    {
        states = false;
        tunnel = GameObject.Find("Tunnel").GetComponent<TunnelControl>();
        lighting = transform.Find("Light").gameObject;
        lighting.SetActive(false);

}


    void Update()
    {
        //����ġ O �� �ͳ� ���� OFF ���¸�, X�� ON ���¸� �ǹ��մϴ�.
        
        if (tunnel.Tun == false)
        {
            //�ͳ� X & ����ġ O or X
            lighting.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ
            states = false;
           
        }
        else if (tunnel.Tun == true && InterOb.trigger == false)
        {
            //�ͳ� O & ����ġ X
            lighting.SetActive(true); // ���� ������Ʈ Ȱ��ȭ
            states = false;
       
        }
        else if (tunnel.Tun == true && InterOb.trigger == true)
        {
            //�ͳ� O & ����ġ O
            lighting.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
            states = true;
            //Debug.Log("���� off");
        }
    }
}