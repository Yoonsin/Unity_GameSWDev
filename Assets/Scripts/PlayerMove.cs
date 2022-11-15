using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rigid;
    //CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private float speed = 7.0f;     // 이동 속도
    public float jumpPower;         // 점프력

    private bool isDamaged = false;       // 피격 상태 판별
    private float damagedTimer = 1;       // 피격 상태 지속 시간

    // 피격 이펙트
    public Image damagedBg;
    private float damagedBgAlpha = 0;
    public GameObject Camera;
    Vector3 cameraOriginPos;

    private float attackTimer = 2;      // 강공 후 딜레이
    private float preAttack = 2;        // 이전 공격 일정시간 후 공격횟수 초기화

    public InteractiveObject interactiveObject = null;  // 상호작용 물체

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // 부모자식 간의 충돌 무시
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
        if (gameManager.playerAttack < 1)   // 강공 판별
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                gameManager.playerAttack = 4;
                attackTimer = 2;
            }
        } else if (Time.deltaTime - preAttack >= 2) // 공격 지연 시 공격 횟수 초기화
        {
            gameManager.playerAttack = 4;
            attackTimer = 2;
        }

        // 점프
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && !anim.GetBool("isJumping"))
        {
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
            anim.SetBool("isWalking", true);

        // 공격 *
        if (Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && gameManager.playerAttack > 0)
        {
            gameManager.AttackCntDown();
            preAttack = Time.deltaTime;
            anim.SetTrigger("Attack");
            Debug.Log("player move " + gameManager.playerAttack);
        }

        // 상호작용
        if (Input.GetKeyDown(KeyCode.LeftAlt) && interactiveObject)
        {
            interactiveObject.Interaction();
        }
    }

    // 주로 지속적 이벤트.
    private void FixedUpdate()
    {
        // 이동 left, a: -1 / right, d: 1 / 안 움직이거나 양쪽 다 누를 때: 0
        float key = Input.GetAxisRaw("Horizontal");
        // x축 이동은 x * speed로, y축 이동은 기존의 속력 값(현재는 중력)
        if (gameObject.layer != 8)  // 피격시 반동을 위해서 제어할 수 없게 함
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
    }

    private void OnDamaged(Vector2 targetPos)
    {
        // 체력 감소
        gameManager.HealthDown();
        if (gameManager.playerHP < 1)
            return;

        // 레이어 변경
        gameObject.layer = 8;   // PlayerDamaged 레이어 (플레이어 무적 레이어)

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
        gameObject.layer = 7;   // 레이어 변경: Player 
    }

    public void OnDie()
    {
        // 죽음 이펙트
        gameObject.layer = 8;   // PlayerDamaged 레이어
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("Respawn", 2);   // 2초 후 리스폰
    }
    
    private void Respawn()
    {
        // 플레이어 원위치
        spriteRenderer.flipX = false;
        gameObject.transform.position = new Vector3(-5, -3, -1);
        gameObject.layer = 7;   // Player 레이어
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
            Camera.transform.localPosition = (Vector3)Random.insideUnitCircle * amount + cameraOriginPos;

            timer += Time.deltaTime;
            yield return null;
        }
        Camera.transform.localPosition = cameraOriginPos;
    }
}
