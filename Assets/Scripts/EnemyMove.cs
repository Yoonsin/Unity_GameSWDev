using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyMove : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
    public TunnelControl tunnel;
    public InteractiveObject InterOb;
    public TunnelLightControl tunnelL;

    public float movePower = 1f;
    Rigidbody2D rigid;
    CapsuleCollider2D enemyCollider;
    BoxCollider2D scanCollider;
    Vector3 spos_x;
    Vector3 ppos_x;
    Vector3 movement;
    Vector2 dection;
    Animator anim;
    SpriteRenderer spriteRenderer;

    bool isTracing = false;
    GameObject traceTarget;
    private Transform scan;

    int nextMove;
    int enemyHP;
    int bossHP;
    int movementFlag = 0;
    int AttackStack; // 강격
    [SerializeField]
    bool canCounterattack = false; // 반격 가능 여부
    bool isCounted = false; // 반격 진행 중 여부
    float preStimer;    // 강공 준비 시간 중 반격 가능해질 때까지 기다리기
    bool AttackScan = false;

    float timer;        // 딜레이
    float waitingTime;

    float SwaitingTime;
    bool isAttacking = false;
    bool attackReady = false; // 플레이어 정지 변수
    public bool touchedSword = false;    // 플레이어의 검에 닿았는가

    public bool Boss; // 현재 오브젝트가 보스인가

    RaycastHit2D rayHit;    // 플레이어가 적 시야 내에 있는지 확인하는 레이캐스트

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        scanCollider = GetComponent<BoxCollider2D>();
        scan = gameObject.transform.Find("EnemyAI");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // 부모자식 간의 충돌 무시
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movePower = 1f;
        spos_x = this.gameObject.transform.position;
        StartCoroutine("ChangeMovement");
        nextMove = Random.Range(-1, 1);
        if (nextMove == 0)
            nextMove = 1;
        anim.SetBool("isWalking", true);
        spriteRenderer.flipX = nextMove == 1;
        timer = 0.0f;
        waitingTime = 0.5f;
        AttackStack = 0;
        AttackScan = false;
        SwaitingTime = 1.5f;
        canCounterattack = false; // 반격 가능 여부
        isCounted = false;  // 반격 진행 중 여부
        preStimer = SwaitingTime / 2.0f;    // 반격 가능해질때까지 기다리는 타이머
        isAttacking = false;
        attackReady = false;
        touchedSword = false;
        isTracing = false;
        enemyHP = 5;
        bossHP = enemyHP * 2;
        movementFlag = 0;
        traceTarget = null;
    }

    IEnumerator ChangeMovement()
    {
        if (ppos_x.x > spos_x.x + 2)
        {
            movementFlag = -1;
        }
        else if (ppos_x.x < spos_x.x - 2)
        {
            movementFlag = 1;
        }

        if (movementFlag == 1 || movementFlag == -1)
            anim.SetBool("isWalking", true);

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isInterrupting == true) //플레이어가 반격 키를 눌렀을 때
        {
            if (OnInterrupt()) //반격 가능하면
            {
                player.anim.SetTrigger("onCounter"); //플레이어 카운터 애니 재생

            }

        }
        
        // 레이어 검사 때문에 Update에서 수행
        if (gameObject.layer != 7)  // 쇼크 상태가 아니면 움직여라
        {
            Move();
        }
        else if(gameObject.layer == 7)// 쇼크 상태이면 멈춰라
        {
            anim.SetBool("isWalking", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
            
        if (tunnel.Tun == false) // 적 AI OnOff
        {
            gameObject.transform.Find("EnemyAI").gameObject.SetActive(true);// 스캔 콜라이더 활성화
            AttackScan = true;
        }
        else if (tunnel.Tun == true && InterOb.trigger == true)
        {
            scan.gameObject.SetActive(false);// 스캔 콜라이더 비활성화
            AttackScan = false;
        }
        if (isAttacking)
        {
            if (preStimer < 0)
            {
                preStimer = SwaitingTime / 2.0f;
                Debug.Log("반격 해!!");
                canCounterattack = true;     // 반격 가능
            }
            else if (!canCounterattack)
            {
                preStimer -= Time.deltaTime;  // 타이머 발동
            }
        }
        if (player.anim)
        {
            if (!Boss)
            {
                if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("counter") && OnInterrupt())
                {
                    isCounted = true;
                    if (tunnel.Tun == true && !tunnelL.states)
                        Debug.Log("터널 암살!");
                    Debug.Log("반격 중 적 강공");
                    OnShocked();
                }
                if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("attack_4") && touchedSword && (isCounted || (tunnel.Tun && !tunnelL.states)))
                {
                    isCounted = false;
                    if (tunnel.Tun == true && !tunnelL.states)
                        Debug.Log("터널 암살!");
                    Debug.Log("반격 성공!");
                    canCounterattack = false;
                    preStimer = SwaitingTime / 2.0f;
                    enemyHP = 0;
                    OnDie();
                }
            }
        }
    }

    public bool OnInterrupt()   //플레이어의 반격 가능 여부 반환
    {
        //Debug.Log("OnInterrupt() " + canCounterattack);
        if (touchedSword && (canCounterattack == true || (tunnel.Tun == true && !tunnelL.states)))
            return true;
        else 
            return false;

    }

    

    public void OnDamaged()
    {
        attackReady = true;
        anim.SetBool("isDamaged", true);
        --enemyHP;
        Debug.Log("enemy_OnDamaged: " + enemyHP);
        gameManager.AttackCntDown();

        if (gameManager.playerAttack < 1)   // 강공이었으면 3초 공격 쉬기 *
        {
            Debug.Log("강공!");
            enemyHP -= 1;   // 강공 추가 데미지
            if (!canCounterattack)
                OnShocked();
            player.attackTimer = 5;
        }
        else                                // 일반 공격 
        {
            player.attackTimer = 5; //3초동안 공격 없으면 공격 횟수 초기화
        }
        Debug.Log("EnemyMove_OnDamaged player attacked " + gameManager.playerAttack);
        Invoke("Sterne", 0.5f);
        Debug.Log("Enemy HP: " + enemyHP);
        if (enemyHP < 1) 
        {
            anim.SetBool("isDamaged", true);
            Invoke("OnDie", 1.5f);
        }
    }
    public void BossOnDamaged()
    {
        attackReady = true;
        anim.SetBool("isDamaged", true);
        --bossHP;
        Debug.Log("enemy_OnDamaged: " + bossHP);
        gameManager.AttackCntDown();

        if (gameManager.playerAttack < 1)   // 강공이었으면 3초 공격 쉬기 *
        {
            Debug.Log("강공!");
            bossHP -= 1;   // 강공 추가 데미지
            if (!canCounterattack)
                OnShocked();
            player.attackTimer = 3;
        }
        else                                // 일반 공격 시 3초동안 공격 없으면 공격 횟수 초기화
        {
            player.attackTimer = 3;
        }
        Debug.Log("EnemyMove_OnDamaged player attacked " + gameManager.playerAttack);
        Invoke("Sterne", 0.5f);
        Debug.Log("Enemy HP: " + bossHP);
        if (bossHP < 1)
        {
            anim.SetBool("isDamaged", true);
            Invoke("OnDie", 1.5f);
        }
    }

    private void Sterne()
    {
        anim.SetBool("isDamaged", false);
        attackReady = false;
    }
    private void OnShocked()
    {
        if (!Boss)
            Debug.Log("적: 으악!");
        else
            Debug.Log("보스: 으악!");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 적 투명도 조절
        gameObject.layer = 7;                  // 레이어 변경: shockedEnemy
        Invoke("OffShocked", 2.0f);                         // 2초 후에 쇼크 상태 해제
    }
    private void OffShocked()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.layer = 6;
    }

    private void OnDie()
    {
        gameManager.currentStageEnemy--;
        if (!Boss)
            Debug.Log("Enemy Dead!");
        else
            Debug.Log("Boss Dead!");
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // 플레이어가 눈앞에 있는지 확인
        if (!attackReady)
            rayHit = Physics2D.Raycast(rigid.position, rigid.velocity, 1, LayerMask.GetMask("Player"));
        else
            rayHit = Physics2D.Raycast(rigid.position, dection, 1, LayerMask.GetMask("Player"));
        // 레이캐스트 그리기
        Debug.DrawRay(rigid.position, rigid.velocity, new Color(0, 2, 0));
        timer += Time.deltaTime;
        if (timer > waitingTime && AttackScan == true) // 일반공격
        {
            if (rayHit.collider != null && rayHit.collider.tag == "Player")
            {
                if (AttackStack == 1 && isAttacking == false) // 강격
                {
                    attackReady = true;
                    Debug.Log("강격 준비 시작!");
                    anim.SetBool("isStrongAttacking", true);
                    Invoke("EnemyStrongAttack", SwaitingTime);
                    isAttacking = true;
                }
                else if (AttackStack == 0)
                {
                    anim.SetBool("isAttacking", true);
                    attackReady = true;
                    Invoke("EnemyAttack", 0.3f);
                    AttackStack += 1;
                    //Debug.Log("AttackStack: " + AttackStack);
                }
            }
            else
            {
                //Debug.Log("범위에 없음");
            }
            timer = 0;
        }
    }
    public void EnemyAttack()
    {
        Debug.Log("Enemy Attacks Player!!");
        if (rayHit.collider != null && rayHit.collider.tag == "Player")
        {
            if(!player.anim.GetCurrentAnimatorStateInfo(0).IsName("dash")) //대쉬 안할 때만
              player.OnDamaged(rigid.transform.position); //플레이어 데미지 받음
        }
        anim.SetBool("isAttacking", false);
        attackReady = false;
    }
    public void EnemyStrongAttack()
    {

        Debug.Log("강격 준비 끝!");
        isAttacking = false;
        canCounterattack = false;    // 반격 불가
        if(rayHit.collider != null && rayHit.collider.tag == "Player")
        {
            if (!player.anim.GetCurrentAnimatorStateInfo(0).IsName("counter")&& !player.anim.GetCurrentAnimatorStateInfo(0).IsName("dash")) //반격 못했을 때 or 대쉬 안할 때
            {
                player.OnDamaged(rigid.transform.position); //플레이어 데미지 받음.
            }
            

        }
        anim.SetBool("isStrongAttacking", false);
        attackReady = false;
        AttackStack = 0;
    }

    void Move() // 적 이동
    {
        if (!attackReady)
        {
            Vector3 moveVelocity = Vector3.zero;
            ppos_x = this.gameObject.transform.position;
            if (isTracing)
            {
                Vector3 playerPos = traceTarget.transform.position;
                if (playerPos.x < transform.position.x)
                {
                    nextMove = -1;
                    spriteRenderer.flipX = nextMove == 1;
                }
                else if (playerPos.x > transform.position.x)
                {
                    nextMove = 1;
                    spriteRenderer.flipX = nextMove == 1;
                }
                rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
            }
            else
            {
                if (ppos_x.x > spos_x.x + 2)
                {
                    nextMove = -1;
                    spriteRenderer.flipX = nextMove == 1;
                }
                else if (ppos_x.x < spos_x.x - 2)
                {
                    nextMove = 1;
                    spriteRenderer.flipX = nextMove == 1;
                }
                rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
            }
            dection = rigid.velocity;
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }


    // 플레이어 탐지
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            anim.SetBool("isWalking", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }

}