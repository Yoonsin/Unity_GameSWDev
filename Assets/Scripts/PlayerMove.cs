using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.VersionControl.Asset;
using System;
using System.Threading;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public TextManager textManager;
    public TunnelControl tunnel;
    public InteractiveObject InterOb;
    public TunnelLightControl tunnelL;
    public GameObject child;

    Rigidbody2D rigid;
    //CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    public Animator anim;

    private float speed = 7.0f;     // 이동 속도
    public float dashPower = 5.0f;
    private bool isDash = false;
    public float jumpPower;         // 점프력
    private int jumpCnt = 1;        // 점프 횟수

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
    public int childCnt = 0; //아이 위치 옮겨준 횟수

    public InteractiveObject interactiveObject = null;  // 상호작용 물체
    public bool isFinalBomb = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // 부모자식 간의 충돌 무시
        playerLight = GameObject.Find("Player").transform.Find("Player_light");
    }

    void Start()
    {
        
        gameObject.layer = 8;   // 플레이어의 레이어를 Player로 함
        damagedBgAlpha = 0;
        damagedBg.color = new Color(1, 1, 1, 0);

        jumpCnt = 1;
        speed = 7.0f;
        isDamaged = false;
        damagedTimer = 1;

        isShaked = false; // 피격시 카메라 흔들림 여부 => cameraMove 스크립트에서 사용

        attackTimer = -1;      // 공격 타이머 (공격 횟수 초기화에 사용)
        isAttacking = false;
        isInterrupting = false; // 반격을 시도했는지 확인
        interactiveObject = null;
    
        childCnt = 0;  //아이 위치 옮겨준 횟수
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
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && jumpCnt > 0)
        {
            jumpCnt--;
            isAttacking = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp("w") || Input.GetKeyUp("up")) && anim.GetBool("isJumping"))
            anim.SetBool("isJumping", false);

        // 방향 전환
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(3, 3, 3);   // Left flip
        } else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(-3, 3, 3);   // Right flip
        }
        
        // 걷기 애니메이션
        if (rigid.velocity.normalized.x == 0 ||isDash == true)
            anim.SetBool("isWalking", false);
        else
        {
            anim.SetBool("isWalking", true);
            
        }

        // 공격 *
        if (Input.GetKeyDown(KeyCode.LeftControl) && gameManager.playerAttack > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            isAttacking = true;
            anim.SetTrigger("" + (5 - gameManager.playerAttack));
            //Debug.Log("Attack " + (5 - gameManager.playerAttack));
        }
        // 반격
        if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            isInterrupting = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isInterrupting = false;
        }
        // 대쉬
        if (Input.GetKeyDown(KeyCode.C))
        {
            //평타 애니 트리거 다 끄고 
            for(int i=1;i<=4;i++)
              anim.ResetTrigger(""+ i);
            anim.SetBool("isJumping", false);
            //대쉬 중에는 무적. 키 입력 불가.( 대쉬 애니 이벤트에 넣기)
            isDash = true;
            anim.SetTrigger("onDash"); //대쉬 애니 재생
            Debug.Log("대쉬중");
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            // 방향 전환
            if (transform.localScale.x == 3) //왼쪽
            {
                rigid.AddForce(Vector2.left * dashPower, ForceMode2D.Impulse);  
            }
            else if (transform.localScale.x == -3) //오른쪽
            {
                rigid.AddForce(Vector2.right * dashPower, ForceMode2D.Impulse);
            }

        }

        // 상호작용
        if (Input.GetKeyDown(KeyCode.LeftAlt) && interactiveObject)
        {
            interactiveObject.Interaction();
        }

        // 플레이어 조명(터널에서 스위치 못 올렸을 때만 켜지기)
        if (tunnel.Tun == true && tunnelL.states == false && InterOb.trigger == true)
        {
            GameObject.Find("Player").transform.Find("Player_light").gameObject.SetActive(true);// 조명 오브젝트 활성화
        }
        else if(tunnel.Tun == false || tunnelL.states == false)
        {
            playerLight.gameObject.SetActive(false);// 조명 오브젝트 비활성화
        }
    }

    public void FinishDashAnim()
    {
        isDash = false;
        anim.SetBool("isWalking", true);
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    public void FinishAttackAnim()
    {
        isAttacking = false;
        anim.ResetTrigger("onCounter");
        anim.ResetTrigger("1");
        anim.ResetTrigger("2");
        anim.ResetTrigger("3");
        anim.ResetTrigger("4");
    }

    // 주로 지속적 이벤트.
    private void FixedUpdate()
    {
        // 이동 left, a: -1 / right, d: 1 / 안 움직이거나 양쪽 다 누를 때: 0
        float key = Input.GetAxisRaw("Horizontal");
        // x축 이동은 x * speed로, y축 이동은 기존의 속력 값(현재는 중력)
        Debug.Log(isDash);
        if (gameObject.layer != 9 && isDash == false)  // 피격시 반동 or 대쉬 중일 때는 아래 구문 제어를 받지 못하게 해야함.
            rigid.velocity = new Vector2(key * speed, rigid.velocity.y);
        
        // Landing Platform, 레이 캐스트
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1.5f, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.1f)
                {
                    Debug.Log("착지");
                    jumpCnt = 1;
                    anim.SetBool("isJumping", false);
                }
            }
        }
        
        // 열린 문 닫기
        if (rigid.position.x >= gameManager.wallX[gameManager.currentStage] + 2.0f && gameManager.isOpened)
        {
            Debug.Log("Player X: " + rigid.position.x);
            GameObject.Find("Wall").transform.GetChild(gameManager.currentStage).gameObject.SetActive(true);
            gameManager.isOpened = false;
            gameManager.currentStage++;
            if(gameManager.currentStage == 5) //스테이지 끝이면
            {
                gameManager.endUiObj.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                gameManager.currentStageEnemy = gameManager.enemyNum[gameManager.currentStage - 1];
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        // 몸빵 코드
        if (collision.gameObject.tag == "Enemy")
        {
            damagedTimer = 1;
            isDamaged = true;
            OnDamaged(collision.transform.position);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision) //다음 스테이지로 넘어감
    {
        //Debug.Log("collision: " + collision.gameObject.tag);
        if (!textManager.getFlag() && (collision.gameObject.tag == "Stage") ) //대화창 비활성화 상태고 태그가 스테이지라면
        {
            if((gameManager.currentStage == 4 && isFinalBomb == true) || gameManager.currentStage != 4) //보스 스테이지에서는 폭탄을 해체해야 다음 스테이지로 넘어갈 수 있음
            {
                GameObject coll = collision.gameObject;
                coll.transform.position = new Vector3(gameManager.stageX[childCnt + 1], coll.transform.position.y, coll.transform.position.z); //스테이지 블록은 다음 스테이지로 옮기기
                child.transform.position = new Vector3(gameManager.stageX[childCnt], child.transform.position.y, child.transform.position.z); //아이 위치도 스테이지에 맞게 옮겨주기
                childCnt++;
                textManager.setFlag(true); //대화창 활성화;
                textManager.showText(); //되자마자 대화창 다시 띄우기
                Time.timeScale = 0; //대화창 되면 시간 멈추게 하기

                if (gameManager.currentStage == 4 && isFinalBomb == true) //스테이지가 다 끝났다면
                {
                    coll.SetActive(false); //스테이지 블록 비활성화
                }


            }
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
        if (gameObject.activeInHierarchy)
            StartCoroutine(Shake(0.2f, 0.15f));
        damagedBgAlpha = 0.2f * (5 - gameManager.playerHP);
        damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
            StartCoroutine("reduceDamagedBg");

        // 플레이어 투명도 조절
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 반동
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 0) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);    // OffDamaged() 호출 시간 지연. 무적 해제. 수정 필요.
    }

    private void OffDamaged()
    {
        // 플레이어 이펙트
        //spriteRenderer.color = new Color(1, 1, 1, 1);   // 투명도 조절
        gameObject.layer = 8;   // 레이어 변경: Player 
    }

    public void OnDie()
    {
        // 죽음 이펙트
        gameObject.layer = 9;   // PlayerDamaged 레이어
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("Respawn", 2);   // 2초 후 리스폰
    }
    
    private void Respawn()
    {
        //씬 리로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
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
            Camera.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * amount + cameraOriginPos;

            timer += Time.deltaTime;
            yield return null;
        }
        isShaked = false;
        Camera.transform.localPosition = cameraOriginPos;
    }
}
