using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player;
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

    int nextMove;
    public int enemyHP = 5; // 수정: private으로 바꾸기
    public int enemyAD = 1;
    int movementFlag = 0;
    int StrongAD = 0;
    float attackDelay = 2;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        scanCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponentsInChildren<CapsuleCollider2D>()[0]);  // 부모자식 간의 충돌 무시
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
        // 레이어 검사 때문에 Update에서 수행
        if (gameObject.layer != 7)  // 쇼크 상태가 아니면 움직여라
            Move();
        else // 쇼크 상태이면 멈춰라
        {
            anim.SetBool("is_Walking", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    public void OnDamaged()
    {
        enemyHP--;
        gameManager.AttackCntDown();
        if (gameManager.playerAttack < 1)   // 강공이었으면 3초 공격 쉬기
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

    void Attack(Collider2D other)
    {
        attackDelay -= Time.deltaTime;
        if(attackDelay < 0) attackDelay = 0;
        if (isAttack && StrongAD == 0)
        {

        }
        else if(isAttack && StrongAD == 1)
        {
            enemyAD = 2;

            enemyAD = 1;
        }
        else
        {
            StrongAD = 0;

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
            anim.SetBool("is_Attack", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            isAttack = false;
            StartCoroutine("ChangeMovement");
        }
    }

}
