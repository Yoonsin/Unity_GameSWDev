using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class TunnelLightControl : MonoBehaviour
{
   
    public InteractiveParent InterOb;

    //암살가능 상태(터널 O & 스위치 O)
    public bool states = false;

    //왜 public으로 해서 인스펙터 창에 넣어줬을 땐 인식이 제대로 안됐을까..플랫폼 제너레이터컴포넌트만 awake에서 기차맵을 생성해서 그런가?
    //플랫폼 제너레이터 외의 다른 컴포넌트는 start에서 초기화 하니까, 기차맵 안의 컴포넌트가 인식이 안되는거지
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
        //스위치 O 가 터널 조명 OFF 상태를, X가 ON 상태를 의미합니다.
        
        if (tunnel.Tun == false)
        {
            //터널 X & 스위치 O or X
            lighting.SetActive(false); // 조명 오브젝트 비활성화
            states = false;
           
        }
        else if (tunnel.Tun == true && InterOb.trigger == false)
        {
            //터널 O & 스위치 X
            lighting.SetActive(true); // 조명 오브젝트 활성화
            states = false;
       
        }
        else if (tunnel.Tun == true && InterOb.trigger == true)
        {
            //터널 O & 스위치 O
            lighting.SetActive(false);// 조명 오브젝트 비활성화
            states = true;
            //Debug.Log("조명 off");
        }
    }
}