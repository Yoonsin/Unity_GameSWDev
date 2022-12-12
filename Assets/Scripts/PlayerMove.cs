using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public TunnelControl tunnel;
    public InteractiveObject InterOb;
    public TunnelLightControl tunnelL;

    Rigidbody2D rigid;
    //CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private float speed = 7.0f;     // 이동 속도
    public float jumpPower;         // 점프력

    private bool isDamaged = false;       // 피격 상태 판별
    private float damagedTimer = 1;       // 피격 상태 지속 시간

    private Transform playerLight;      // 플레이어 조명

    // 피격 이펙트
    public Image damagedBg;
    private float damagedBgAlpha = 0;
    public GameObject Camera;
    Vector3 cameraOriginPos;
    public bool isShaked = false; // 피격시 카메라 흔들림 여부 => cameraMove 스크립트에서 사용

    public float attackTimer = -1;      // 공격 타이머 (공격 횟수 초기화에 사용)
    public bool isAttacking = false;
    public bool isInterrupting = false; // 반격을 시도했는지 확인

    public InteractiveObject interactiveObject = null;  // 상호작용 물체

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // 부모자식 간의 충돌 무시
        playerLight = GameObject.Find("Player").transform.Find("Player_light");
        InterOb = GameObject.Find("Spike").GetComponent<InteractiveObject>();
        tunnelL = GameObject.Find("Light_Parent").GetComponent<TunnelLightControl>();
        tunnel = GameObject.Find("Tunnel").GetComponent<TunnelControl>();
    }

    // 매 프레임 호출. 주로 단발적 이벤트.
    private void Update()
    {
        // 피격 상태 지속 시간
        if (isDamaged)
        {
            damagedTimer -= Time.deltaTime;     // deltaTime: 1/FPS
            if (damagedTimer <= 0)
            {
                damagedBgAlpha = 0;
                gameManager.playerHP = 4;       // 플레이어 HP 초기화
                isDamaged = false;
                damagedTimer = 1;
            }
        }

        // 공격 딜레이 및 공격 횟수 초기화 *
        if (attackTimer >= 0)
        {
            //Debug.Log("attackTimer " + attackTimer);
            attackTimer -= Time.deltaTime;
        } else if (attackTimer > -1)    // 타이머 끝났고 초기값보다 클 때
        {

            Debug.Log("playerAttack reset");
            gameManager.playerAttack = 4;
            attackTimer = -1;
        }

        // 점프
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && !anim.GetBool("isJumping"))
        {
            isAttacking = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // 방향 전환
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);   // Left flip
        } else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);   // Left flip
        }

        // 걷기 애니메이션
        if (rigid.velocity.normalized.x == 0)
            anim.SetBool("isWalking", false);
        else
        {
            anim.SetBool("isWalking", true);
            isAttacking = false;
        }

        // 공격 *
        if (Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && gameManager.playerAttack > 0)
        {
            isAttacking = true;
            //Debug.Log("isAttacking " + isAttacking);
            anim.SetTrigger("Attack");
        }
        // 반격
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Z Pressed");
            isInterrupting = true;
            //Debug.Log("반격 시도");
            anim.SetTrigger("Attack");
        }

        // 상호작용
        if (Input.GetKeyDown(KeyCode.LeftAlt) && interactiveObject)
        {
            interactiveObject.Interaction();
        }

        // 플레이어 조명(터널에서 스위치 못 올렸을 때만 켜지기)
        if (tunnel.Tun == true && tunnelL.states == true && InterOb.trigger == false)
        {
            GameObject.Find("Player").transform.Find("Player_light").gameObject.SetActive(true);// 조명 오브젝트 활성화
        }
        else if(tunnelL.states == true)
        {
            playerLight.gameObject.SetActive(false);// 조명 오브젝트 비활성화
        }
    }

    // 주로 지속적 이벤트.
    private void FixedUpdate()
    {
        // 이동 left, a: -1 / right, d: 1 / 안 움직이거나 양쪽 다 누를 때: 0
        float key = Input.GetAxisRaw("Horizontal");
        // x축 이동은 x * speed로, y축 이동은 기존의 속력 값(현재는 중력)
        if (gameObject.layer != 9)  // 피격시 반동을 위해서 제어할 수 없게 함
            rigid.velocity = new Vector2(key * speed, rigid.velocity.y);

        // Landing Platform, 레이 캐스트
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)  // 플레이어 크기의 절반
                    anim.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            damagedTimer = 1;
            isDamaged = true;
            OnDamaged(collision.transform.position);
        }
        if (collision.gameObject.tag == "Enemy_Attack")
        {
            damagedTimer = 1;
            isDamaged = true;
            OnDamaged(collision.transform.position);
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        // 체력 감소
        gameManager.HealthDown();
        if (gameManager.playerHP < 1)
            return;

        // 레이어 변경
        gameObject.layer = 9;   // PlayerDamaged 레이어 (플레이어 무적 레이어)

        // 배경 이펙트
        StartCoroutine(Shake(0.2f, 0.15f));
        damagedBgAlpha = 0.2f * (5 - gameManager.playerHP);
        damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
        StartCoroutine("reduceDamagedBg");

        // 플레이어 투명도 조절
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 반동
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);    // OffDamaged() 호출 시간 지연. 무적 해제. 수정 필요.
    }

    private void OffDamaged()
    {
        // 플레이어 이펙트
        spriteRenderer.color = new Color(1, 1, 1, 1);   // 투명도 조절
        gameObject.layer = 8;   // 레이어 변경: Player 
    }

    public void OnDie()
    {
        // 죽음 이펙트
        gameObject.layer = 9;   // PlayerDamaged 레이어
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("Respawn", 2);   // 2초 후 리스폰
    }
    
    private void Respawn()
    {
        // 플레이어 원위치
        spriteRenderer.flipX = false;
        gameObject.transform.position = new Vector3(-5, -3, -1);
        gameManager.playerHP = 4;
        gameObject.layer = 8;   // Player 레이어
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator reduceDamagedBg()
    {
        while (damagedBgAlpha >= 0)
        {
            damagedBgAlpha -= (0.2f * (5 - gameManager.playerHP)) / 80;
            yield return new WaitForSeconds(0.01f); // 0.01초마다 실행
            damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
        }
    }

    private IEnumerator Shake(float amount, float duration)
    {
        cameraOriginPos = Camera.transform.localPosition;
        float timer = 0;
        while (timer <= duration)
        {
            isShaked = true; //true면 cameraMove 에서 카메라가 플레이어 따라가는 코드 중지시키기. (cameraMove의 카메라 위치 변경 코드가 여기 코드를 덮고있어서 흔들림이 적용안돼기 때문)
            Camera.transform.localPosition = (Vector3)Random.insideUnitCircle * amount + cameraOriginPos;

            timer += Time.deltaTime;
            yield return null;
        }
        isShaked = false;
        Camera.transform.localPosition = cameraOriginPos;
    }
}
