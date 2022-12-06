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
    public int enemyHP = 5; // ����: private���� �ٲٱ�
    public int enemyAD = 1;
    int movementFlag = 0;

    float timer;        // ������
    float waitingTime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        scanCollider = GetComponent<BoxCollider2D>();
        scan = GameObject.Find("Enemy").transform.Find("EnemyAI");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponentsInChildren<CapsuleCollider2D>()[0]);  // �θ��ڽ� ���� �浹 ����
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
        // ���̾� �˻� ������ Update���� ����
        if (gameObject.layer != 7)  // ��ũ ���°� �ƴϸ� ��������
            Move();
        else // ��ũ �����̸� �����
        {
            anim.SetBool("is_Walking", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        if (tunnel.Tun == false)
        {
            GameObject.Find("Enemy").transform.Find("EnemyAI").gameObject.SetActive(true);// ��ĵ �ݶ��̴� Ȱ��ȭ
        }
        else if(tunnel.Tun == true)
        {
            scan.gameObject.SetActive(false);// ��ĵ �ݶ��̴� ��Ȱ��ȭ
        }
    }


    public void OnDamaged()
    {
        enemyHP--;
        gameManager.AttackCntDown();
        if (gameManager.playerAttack < 1)   // �����̾����� 3�� ���� ����
        {
            enemyHP -= 1;   // ���� �߰� ������
            OnShocked();
            player.attackTimer = 3;
        }
        else                                // �Ϲ� ���� �� 2�ʵ��� ���� ������ ���� Ƚ�� �ʱ�ȭ
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
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // �� ���� ����
        gameObject.layer = 7;                  // ���̾� ����: shockedEnemy
        Invoke("OffShocked", 2.0f);                         // 2�� �Ŀ� ��ũ ���� ����
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
        // ���� ���տ� �ִ��� Ȯ��
        Debug.DrawRay(rigid.position, rigid.velocity, new Color(0, 2, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, rigid.velocity, 1, LayerMask.GetMask("Player"));
        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            if (rayHit.collider != null && rayHit.collider.tag == "Player")
            {
                Debug.Log(rayHit.collider.tag);
                anim.SetBool("is_Attack", true);
                EnemyAttack();
            }
            else
            {
                Debug.Log("������ ����");
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

    void Move() // �� �̵�
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
   

    // �÷��̾� Ž��
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
