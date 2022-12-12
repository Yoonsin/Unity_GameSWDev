using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Animator anim;
    SpriteRenderer spriteRenderer;
    bool isTracing = false;
    bool isAttack = false;
    GameObject traceTarget;
    private Transform scan;

    int nextMove;
    public int enemyHP = 5; // 수정: private으로 바꾸기
    public int enemyAD = 1;
    int movementFlag = 0;
    int AttackStack; // 강격
    [SerializeField]
    bool preStrongAttack = false; // 강공 준비 시간

    float timer;        // 딜레이
    float waitingTime;

    float Stimer;       // 강격 딜레이
    float SwaitingTime;

    RaycastHit2D rayHit;    // 플레이어가 적 시야 내에 있는지 확인하는 레이캐스트

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        scanCollider = GetComponent<BoxCollider2D>();
        scan = GameObject.Find("Enemy").transform.Find("EnemyAI");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponentsInChildren<CapsuleCollider2D>()[0]);  // 부모자식 간의 충돌 무시
        InterOb = GameObject.Find("Spike").GetComponent<InteractiveObject>();
        tunnelL = GameObject.Find("Light_Parent").GetComponent<TunnelLightControl>();
        tunnel = GameObject.Find("Tunnel").GetComponent<TunnelControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spos_x = this.gameObject.transform.position;
        StartCoroutine("ChangeMovement");
        nextMove = Random.Range(-1, 1);
        if (nextMove == 0)
            nextMove = 1;
        anim.SetBool("is_Walking", true);
        spriteRenderer.flipX = nextMove == 1;
        timer = 0.0f;
        waitingTime = 0.5f;
        AttackStack = 0;
        Stimer = 0.0f;
        SwaitingTime = 3.0f;    // 원래 0.8f
        preStrongAttack = false; // 강공 준비 시간
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
            anim.SetBool("is_Walking", true);

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isInterrupting == true)
            OnInterrupt();
        // 레이어 검사 때문에 Update에서 수행
        if (gameObject.layer != 7)  // 쇼크 상태가 아니면 움직여라
            Move();
        else // 쇼크 상태이면 멈춰라
        {
            anim.SetBool("is_Walking", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        if (tunnel.Tun == false)
        {
            GameObject.Find("Enemy").transform.Find("EnemyAI").gameObject.SetActive(true);// 스캔 콜라이더 활성화
        }
        else if(tunnel.Tun == true)
        {
            scan.gameObject.SetActive(false);// 스캔 콜라이더 비활성화
        }
    }

    public void OnInterrupt()   // 플레이어가 반격 키를 눌렀을 때 실행
    {
        player.isInterrupting = false;
        Debug.Log("OnInterrupt()");
        if (preStrongAttack == true)
        {
            Debug.Log("반격 성공!");
            OnShocked();
            enemyHP = 0;
            OnDie();
        }
    }

    public void OnDamaged()
    {
        enemyHP--;
        gameManager.AttackCntDown();
        if (gameManager.playerAttack < 1)   // 강공이었으면 3초 공격 쉬기 *
        {
            enemyHP -= 1;   // 강공 추가 데미지
            OnShocked();
            player.attackTimer = 3;
        }
        else                                // 일반 공격 시 2초동안 공격 없으면 공격 횟수 초기화
        {
            player.attackTimer = 2;
        }
        Debug.Log("player attacked " + gameManager.playerAttack);
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        Debug.Log("Enemy HP: " + enemyHP);
        if (enemyHP < 1)
        {
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            Invoke("OnDie", 1.5f);
        }
    }
    private void OnShocked()
    {
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
        Debug.Log("Enemy Dead!");
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // 적이 눈앞에 있는지 확인
        rayHit = Physics2D.Raycast(rigid.position, rigid.velocity, 3, LayerMask.GetMask("Player"));
        // 레이캐스트 그리기
        Debug.DrawRay(rigid.position, rigid.velocity, new Color(0, 2, 0));
        timer += Time.deltaTime;
        if (timer > waitingTime) // 일반공격
        {
            if (rayHit.collider != null && rayHit.collider.tag == "Player")
            {
                Debug.Log(rayHit.collider.tag);
                anim.SetBool("is_Attack", true);
                
                if (AttackStack == 1) // 강격
                {
                    Debug.Log("강격 준비 시작!");
                    preStrongAttack = true;     // 강공 대기 시작
                    //EnemyStrongAttack();
                    Invoke("EnemyStrongAttack", SwaitingTime);
                }
                else if (AttackStack == 0)
                {
                    EnemyAttack();
                    AttackStack += 1;
                    Debug.Log("AttackStack: " + AttackStack);
                }
            }
            else
            {
                //Debug.Log("범위에 없음");
                anim.SetBool("is_Attack", false);
            }
            timer = 0;
        }
    }
    public void EnemyAttack()
    {
        Debug.Log("Player Attack!!");
        player.OnDamaged(rigid.transform.position);

    }
    public void EnemyStrongAttack()
    {

        Debug.Log("강격 준비 끝!");
        preStrongAttack = false;    // 강공 대기 종료
        Debug.Log("Player StrongAttack!!");
        player.OnDamaged(rigid.transform.position);
        AttackStack = 0;

    }

    void Move() // 적 이동
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
            isAttack = true;
            anim.SetBool("is_Walking", true);
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
