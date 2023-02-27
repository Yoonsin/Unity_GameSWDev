using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.2f; //진동 세기
    public float shakeDuration = 0.15f; //진동 시간

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ShakeCamera() //외부(특히 플레이어) 오브젝트에서 접근 가능한 카메라 진동 코루틴 실행 함수, 진동 세기와 시간은 CameraShake 스크립트에서 변경 가능
    {
        StartCoroutine(ShakeCameraCoroutine(shakeAmount, shakeDuration));
        //Debug.Log("카메라 진동 코드 실행");
    }

    private IEnumerator ShakeCameraCoroutine(float amount, float duration) //카메라 상대좌표 흔드는 코루틴, PlayerMove.cs에서 이동됨
    {
        Vector3 cameraOriginPos = transform.localPosition; //초기 상대좌표 저장
        float timer = 0; //흔드는 시간 경과 측정 타이머
        while (timer <= duration)
        {
            transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * amount + cameraOriginPos; //랜덤 위치로 흔들기

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = cameraOriginPos; //원래 상대좌표로 다시 재설정
    }
}
