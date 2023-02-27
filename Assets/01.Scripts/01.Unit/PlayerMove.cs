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
    public InteractiveParent InterOb;
    public TunnelLightControl tunnelL;
    public PlatformGenerator platGenerator;
    public GameObject child;

    Rigidbody2D rigid;
    //CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    public Animator anim;

    private float speed = 7.0f;     // �̵� �ӵ�
    public float dashPower = 5.0f;
    private bool isDashing = false;
    private bool canDash = true;
    private float coolTime_Dash = 1.5f;
    public float jumpPower;         // ������
    private int jumpCnt = 1;        // ���� Ƚ��

    private bool isDamaged = false;       // �ǰ� ���� �Ǻ�
    private float damagedTimer = 1;       // �ǰ� ���� ���� �ð�

    private Transform playerLight;      // �÷��̾� ����

    // �ǰ� ����Ʈ
    public Image damagedBg;
    private float damagedBgAlpha = 0;
    public GameObject Camera;
    Vector3 cameraOriginPos;
    public bool isShaked = false; // �ǰݽ� ī�޶� ��鸲 ���� => cameraMove ��ũ��Ʈ���� ���

    public float attackTimer = -1;      // ���� Ÿ�̸� (���� Ƚ�� �ʱ�ȭ�� ���)
    public bool isAttacking = false;
    public bool isInterrupting = false; // �ݰ��� �õ��ߴ��� Ȯ��
    public int childCnt = 0; //���� ��ġ �Ű��� Ƚ��

    public InteractiveParent interactiveObject = null;  // ��ȣ�ۿ� ��ü (�� ��ü ��ũ��Ʈ���� �˾Ƽ� �־��� ���� ��)
    public bool isFinalBomb = false;
    bool finalFlag = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // �θ��ڽ� ���� �浹 ����
        playerLight = GameObject.Find("Player").transform.Find("Player_light");
    }

    void Start()
    {

        tunnel = GameObject.Find("Tunnel").GetComponent<TunnelControl>();
        gameObject.layer = 8;   // �÷��̾��� ���̾ Player�� ��
        damagedBgAlpha = 0;
        damagedBg.color = new Color(1, 1, 1, 0);

        jumpCnt = 1;
        speed = 7.0f;
        isDamaged = false;
        damagedTimer = 1;

        isShaked = false; // �ǰݽ� ī�޶� ��鸲 ���� => cameraMove ��ũ��Ʈ���� ���

        attackTimer = -1;      // ���� Ÿ�̸� (���� Ƚ�� �ʱ�ȭ�� ���)
        isAttacking = false;
        isInterrupting = false; // �ݰ��� �õ��ߴ��� Ȯ��
        interactiveObject = null;
    
        childCnt = 0;  //���� ��ġ �Ű��� Ƚ��
    }


    // �� ������ ȣ��. �ַ� �ܹ��� �̺�Ʈ.
    private void Update()
    {
        // �ǰ� ���� ���� �ð�
        if (isDamaged)
        {
            damagedTimer -= Time.deltaTime;     // deltaTime: 1/FPS
            if (damagedTimer <= 0)
            {
                damagedBgAlpha = 0;
                gameManager.playerHP = 4;       // �÷��̾� HP �ʱ�ȭ
                isDamaged = false;
                damagedTimer = 1;
            }
        }


        // ����
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && jumpCnt > 0)
        {
            jumpCnt--;
            isAttacking = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp("w") || Input.GetKeyUp("up")) && anim.GetBool("isJumping"))
            anim.SetBool("isJumping", false);

        // ���� ��ȯ. �뽬 �߿��� ������ȯ �Ұ�
        if (Input.GetAxisRaw("Horizontal") < 0 && isDashing == false) 
        {
            transform.localScale = new Vector3(3, 3, 3);   // Left flip
        } else if (Input.GetAxisRaw("Horizontal") > 0 && isDashing == false)
        {
            transform.localScale = new Vector3(-3, 3, 3);   // Right flip
        }
        
        // �ȱ� �ִϸ��̼�
        if (rigid.velocity.normalized.x == 0 ||isDashing == true)
            anim.SetBool("isWalking", false);
        else
        {
            anim.SetBool("isWalking", true);
            
        }


        
        // ���� ������ �� ���� Ƚ�� �ʱ�ȭ *
        if (attackTimer >= 0)
        {
            //Debug.Log("attackTimer " + attackTimer);
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer > -1)    // Ÿ�̸� ������ �ʱⰪ���� Ŭ ��
        {

            Debug.Log("playerAttack reset");
            gameManager.playerAttack = 4;
            attackTimer = -1;
        }

        // ���� *
        if (Input.GetKeyDown(KeyCode.LeftControl) && gameManager.playerAttack > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            isAttacking = true;
            Debug.Log(gameManager.playerAttack);
            anim.SetTrigger("" + (5 - gameManager.playerAttack));
            //Debug.Log("Attack " + (5 - gameManager.playerAttack));
        }
        // �ݰ�
        if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            isInterrupting = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isInterrupting = false;
        }
        // �뽬
        if (Input.GetKeyDown(KeyCode.C) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("counter")) && canDash == true )
        {
            //�뽬 �߿��� ����. Ű �Է� �Ұ�.( �뽬 �ִ� �̺�Ʈ�� �ֱ�)
            canDash = false;
            isDashing = true;
            Invoke("coolTimeDash", coolTime_Dash); //��Ÿ�� 
            anim.SetTrigger("onDash"); //�뽬 �ִ� ���
            rigid.velocity = new Vector2(0, rigid.velocity.y); //���� �� �뽬�ϸ� ���� ����. �װ� ����
            // ���� ��ȯ
            if (transform.localScale.x == 3) //����
            {
                rigid.AddForce(Vector2.left * dashPower, ForceMode2D.Impulse);  
            }
            else if (transform.localScale.x == -3) //������
            {
                rigid.AddForce(Vector2.right * dashPower, ForceMode2D.Impulse);
            }

        }

        // ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.LeftAlt) && interactiveObject)
        {
            interactiveObject.Interaction();
        }

        // �÷��̾� ����(�ͳο��� ����ġ �� �÷��� ���� ������)
        if (tunnel.Tun == true)
        {
            bool lightFlag = false;
             for(int stage = 0; stage < platGenerator.platformList.Count; stage++)
            {
                if (platGenerator.platformList[stage].transform.Find("LightController").GetComponent<TunnelLightControl>().states == true)
                {
                    lightFlag = true; 
                    break;
                }
            }
             if(lightFlag)
            {
                Debug.Log("PlayerMove �÷��̾� ���� �Ҵ�");
                GameObject.Find("Player").transform.Find("Player_light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
            }


        }
        else if(tunnel.Tun == false)
        {
            //Debug.Log("PlayerMove �÷��̾� ���� ����");
            playerLight.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
        }
    }

    void coolTimeDash()
    {
        canDash = true;
    }

    public void FinishDashAnim()
    {
        isDashing = false; //�뽬 ��
        rigid.velocity = new Vector2(0, rigid.velocity.y); //�뽬 �� �ݵ� ������ �����
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

    // �ַ� ������ �̺�Ʈ.
    private void FixedUpdate()
    {
        // �̵� left, a: -1 / right, d: 1 / �� �����̰ų� ���� �� ���� ��: 0
        float key = Input.GetAxisRaw("Horizontal");
        // x�� �̵��� x * speed��, y�� �̵��� ������ �ӷ� ��(����� �߷�)
        if (gameObject.layer != 9 && isDashing == false)  // �ǰݽ� �ݵ� or �뽬 ���� ���� �Ʒ� ���� ��� ���� ���ϰ� �ؾ���.
            rigid.velocity = new Vector2(key * speed, rigid.velocity.y);
        
        
        
        // Landing Platform, ���� ĳ��Ʈ
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1.5f, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.1f)
                {
                    //Debug.Log("����");
                    jumpCnt = 1;
                    anim.SetBool("isJumping", false);
                }
            }
        }

        // ���� �� �ݱ�
        if(gameManager.currentStage < gameManager.stageX.Count + 1) //�������� X �迭 ���� �ʰ� ��������  
        {
            if (gameManager.currentStage >= 2 && rigid.position.x >= gameManager.stageX[gameManager.currentStage - 1] && gameManager.isOpened)
            {

                Debug.Log("Player X: " + rigid.position.x);
                gameManager.wallX[gameManager.currentStage - 1].SetActive(true);
                gameManager.isOpened = false;


            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        // ���� �ڵ�
        if (collision.gameObject.tag == "Enemy")
        {
            damagedTimer = 1;
            isDamaged = true;
            OnDamaged(collision.transform.position);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision) //���� ���������� �Ѿ
    {
        
        if (!textManager.getFlag() && (collision.gameObject.tag == "Stage") ) //��ȭâ ��Ȱ��ȭ ���°� �±װ� �����������
        { 
            GameObject coll = collision.gameObject;
            child.transform.position = new Vector3(coll.transform.position.x, child.transform.position.y, child.transform.position.z); //���� ��ġ�� ���������� �°� �Ű��ֱ�
            childCnt++;

            if(gameManager.currentStage == gameManager.stageX.Count)
            {
                //���� ������ ����������
                //��Ȱ��ȭ ���� �ʰ� ������ ������ �������� ��� �Ű��ֱ�
                if (!finalFlag)
                {
                    textManager.setFlag(true); //��ȭâ Ȱ��ȭ;
                    textManager.showText(); //���ڸ��� ��ȭâ �ٽ� ����
                    Time.timeScale = 0; //��ȭâ �Ǹ� �ð� ���߰� �ϱ�

                    coll.transform.position = new Vector3(coll.transform.position.x + PlatformGenerator.platInterval*0.9f, coll.transform.position.y, coll.transform.position.z);
                    finalFlag = true;

                }
                else if(finalFlag && isFinalBomb)
                {

                    textManager.setFlag(true); //��ȭâ Ȱ��ȭ;
                    textManager.showText(); //���ڸ��� ��ȭâ �ٽ� ����
                    Time.timeScale = 0; //��ȭâ �Ǹ� �ð� ���߰� �ϱ�

                    coll.SetActive(false);
                }
               
            }
            else
            {
                textManager.setFlag(true); //��ȭâ Ȱ��ȭ;
                textManager.showText(); //���ڸ��� ��ȭâ �ٽ� ����
                Time.timeScale = 0; //��ȭâ �Ǹ� �ð� ���߰� �ϱ�

                //������ ���������� �������� ���� ��� ��Ȱ��ȭ��.
                coll.SetActive(false);
            }
        }
    }

    public GameObject mainCamera; //ī�޶� ������Ʈ �ҷ�����

    public void OnDamaged(Vector2 targetPos)
    {
     
        // ü�� ����
        gameManager.HealthDown();
        if (gameManager.playerHP < 1)
            return;

        // ���̾� ����
        gameObject.layer = 9;   // PlayerDamaged ���̾� (�÷��̾� ���� ���̾�)

        // ��� ����Ʈ
        if (gameObject.activeInHierarchy)
            mainCamera.GetComponent<CameraShake>().ShakeCamera(); //ī�޶� ���� �ڷ�ƾ ī�޶� ���� ��ũ��Ʈ�� �̵�, ī�޶� ������Ʈ�� �����Ͽ� �Լ� ����
            //StartCoroutine(Shake(0.2f, 0.15f));
        damagedBgAlpha = 0.2f * (5 - gameManager.playerHP);
        damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
            StartCoroutine("reduceDamagedBg");

        // �÷��̾� ���� ����
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // �ݵ�
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 0) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);    // OffDamaged() ȣ�� �ð� ����. ���� ����. ���� �ʿ�.
    }

    private void OffDamaged()
    {
        // �÷��̾� ����Ʈ
        //spriteRenderer.color = new Color(1, 1, 1, 1);   // ���� ����
        gameObject.layer = 8;   // ���̾� ����: Player 
    }

    public void OnDie()
    {
        // ���� ����Ʈ
        gameObject.layer = 9;   // PlayerDamaged ���̾�
        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("Respawn", 2);   // 2�� �� ������
    }
    
    private void Respawn()
    {
        //�� ���ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    private IEnumerator reduceDamagedBg()
    {
        while (damagedBgAlpha >= 0)
        {
            damagedBgAlpha -= (0.2f * (5 - gameManager.playerHP)) / 80;
            yield return new WaitForSeconds(0.01f); // 0.01�ʸ��� ����
            damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
        }
    }

    
}
