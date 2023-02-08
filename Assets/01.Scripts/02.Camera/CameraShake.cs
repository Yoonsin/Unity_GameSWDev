using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.2f; //���� ����
    public float shakeDuration = 0.15f; //���� �ð�

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ShakeCamera() //�ܺ�(Ư�� �÷��̾�) ������Ʈ���� ���� ������ ī�޶� ���� �ڷ�ƾ ���� �Լ�, ���� ����� �ð��� CameraShake ��ũ��Ʈ���� ���� ����
    {
        StartCoroutine(ShakeCameraCoroutine(shakeAmount, shakeDuration));
        //Debug.Log("ī�޶� ���� �ڵ� ����");
    }

    private IEnumerator ShakeCameraCoroutine(float amount, float duration) //ī�޶� �����ǥ ���� �ڷ�ƾ, PlayerMove.cs���� �̵���
    {
        Vector3 cameraOriginPos = transform.localPosition; //�ʱ� �����ǥ ����
        float timer = 0; //���� �ð� ��� ���� Ÿ�̸�
        while (timer <= duration)
        {
            transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * amount + cameraOriginPos; //���� ��ġ�� ����

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = cameraOriginPos; //���� �����ǥ�� �ٽ� �缳��
    }
}
