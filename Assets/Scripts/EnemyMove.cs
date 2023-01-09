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
    int AttackStack; // ����
    [SerializeField]
    bool canCounterattack = false; // �ݰ� ���� ����
    bool isCounted = false; // �ݰ� ���� �� ����
    float preStimer;    // ���� �غ� �ð� �� �ݰ� �������� ������ ��ٸ���
    bool AttackScan = false;

    float timer;        // ������
    float waitingTime;

    float SwaitingTime;
    bool isAttacking = false;
    bool attackReady = false; // �÷��̾� ���� ����
    public bool touchedSword = false;    // �÷��̾��� �˿� ��Ҵ°�

    public bool Boss; // ���� ������Ʈ�� �����ΰ�

    RaycastHit2D rayHit;    // �÷��̾ �� �þ� ���� �ִ��� Ȯ���ϴ� ����ĳ��Ʈ

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        scanCollider = GetComponent<BoxCollider2D>();
        scan = gameObject.transform.Find("EnemyAI");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // �θ��ڽ� ���� �浹 ����
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
        canCounterattack = false; // �ݰ� ���� ����
        isCounted = false;  // �ݰ� ���� �� ����
        preStimer = SwaitingTime / 2.0f;    // �ݰ� �������������� ��ٸ��� Ÿ�̸�
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
        if (player.isInterrupting == true) //�÷��̾ �ݰ� Ű�� ������ ��
        {
            if (OnInterrupt()) //�ݰ� �����ϸ�
            {
                player.anim.SetTrigger("onCounter"); //�÷��̾� ī���� �ִ� ���

            }

        }
        
        // ���̾� �˻� ������ Update���� ����
        if (gameObject.layer != 7)  // ��ũ ���°� �ƴϸ� ��������
        {
            Move();
        }
        else if(gameObject.layer == 7)// ��ũ �����̸� �����
        {
            anim.SetBool("isWalking", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
            
        if (tunnel.Tun == false) // �� AI OnOff
        {
            gameObject.transform.Find("EnemyAI").gameObject.SetActive(true);// ��ĵ �ݶ��̴� Ȱ��ȭ
            AttackScan = true;
        }
        else if (tunnel.Tun == true && InterOb.trigger == true)
        {
            scan.gameObject.SetActive(false);// ��ĵ �ݶ��̴� ��Ȱ��ȭ
            AttackScan = false;
        }
        if (isAttacking)
        {
            if (preStimer < 0)
            {
                preStimer = SwaitingTime / 2.0f;
                Debug.Log("�ݰ� ��!!");
                canCounterattack = true;     // �ݰ� ����
            }
            else if (!canCounterattack)
            {
                preStimer -= Time.deltaTime;  // Ÿ�̸� �ߵ�
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
                        Debug.Log("�ͳ� �ϻ�!");
                    Debug.Log("�ݰ� �� �� ����");
                    OnShocked();
                }
                if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("attack_4") && touchedSword && (isCounted || (tunnel.Tun && !tunnelL.states)))
                {
                    isCounted = false;
                    if (tunnel.Tun == true && !tunnelL.states)
                        Debug.Log("�ͳ� �ϻ�!");
                    Debug.Log("�ݰ� ����!");
                    canCounterattack = false;
                    preStimer = SwaitingTime / 2.0f;
                    enemyHP = 0;
                    OnDie();
                }
            }
        }
    }

    public bool OnInterrupt()   //�÷��̾��� �ݰ� ���� ���� ��ȯ
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

        if (gameManager.playerAttack < 1)   // �����̾����� 3�� ���� ���� *
        {
            Debug.Log("����!");
            enemyHP -= 1;   // ���� �߰� ������
            if (!canCounterattack)
                OnShocked();
            player.attackTimer = 5;
        }
        else                                // �Ϲ� ���� 
        {
            player.attackTimer = 5; //3�ʵ��� ���� ������ ���� Ƚ�� �ʱ�ȭ
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

        if (gameManager.playerAttack < 1)   // �����̾����� 3�� ���� ���� *
        {
            Debug.Log("����!");
            bossHP -= 1;   // ���� �߰� ������
            if (!canCounterattack)
                OnShocked();
            player.attackTimer = 3;
        }
        else                                // �Ϲ� ���� �� 3�ʵ��� ���� ������ ���� Ƚ�� �ʱ�ȭ
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
            Debug.Log("��: ����!");
        else
            Debug.Log("����: ����!");
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
        gameManager.currentStageEnemy--;
        if (!Boss)
            Debug.Log("Enemy Dead!");
        else
            Debug.Log("Boss Dead!");
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // �÷��̾ ���տ� �ִ��� Ȯ��
        if (!attackReady)
            rayHit = Physics2D.Raycast(rigid.position, rigid.velocity, 1, LayerMask.GetMask("Player"));
        else
            rayHit = Physics2D.Raycast(rigid.position, dection, 1, LayerMask.GetMask("Player"));
        // ����ĳ��Ʈ �׸���
        Debug.DrawRay(rigid.position, rigid.velocity, new Color(0, 2, 0));
        timer += Time.deltaTime;
        if (timer > waitingTime && AttackScan == true) // �Ϲݰ���
        {
            if (rayHit.collider != null && rayHit.collider.tag == "Player")
            {
                if (AttackStack == 1 && isAttacking == false) // ����
                {
                    attackReady = true;
                    Debug.Log("���� �غ� ����!");
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
                //Debug.Log("������ ����");
            }
            timer = 0;
        }
    }
    public void EnemyAttack()
    {
        Debug.Log("Enemy Attacks Player!!");
        if (rayHit.collider != null && rayHit.collider.tag == "Player")
        {
            if(!player.anim.GetCurrentAnimatorStateInfo(0).IsName("dash")) //�뽬 ���� ����
              player.OnDamaged(rigid.transform.position); //�÷��̾� ������ ����
        }
        anim.SetBool("isAttacking", false);
        attackReady = false;
    }
    public void EnemyStrongAttack()
    {

        Debug.Log("���� �غ� ��!");
        isAttacking = false;
        canCounterattack = false;    // �ݰ� �Ұ�
        if(rayHit.collider != null && rayHit.collider.tag == "Player")
        {
            if (!player.anim.GetCurrentAnimatorStateInfo(0).IsName("counter")&& !player.anim.GetCurrentAnimatorStateInfo(0).IsName("dash")) //�ݰ� ������ �� or �뽬 ���� ��
            {
                player.OnDamaged(rigid.transform.position); //�÷��̾� ������ ����.
            }
            

        }
        anim.SetBool("isStrongAttacking", false);
        attackReady = false;
        AttackStack = 0;
    }

    void Move() // �� �̵�
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