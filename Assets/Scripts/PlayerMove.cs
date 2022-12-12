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

    private float speed = 7.0f;     // �̵� �ӵ�
    public float jumpPower;         // ������

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

    public InteractiveObject interactiveObject = null;  // ��ȣ�ۿ� ��ü

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), GetComponentsInChildren<BoxCollider2D>()[0]);  // �θ��ڽ� ���� �浹 ����
        playerLight = GameObject.Find("Player").transform.Find("Player_light");
        InterOb = GameObject.Find("Spike").GetComponent<InteractiveObject>();
        tunnelL = GameObject.Find("Light_Parent").GetComponent<TunnelLightControl>();
        tunnel = GameObject.Find("Tunnel").GetComponent<TunnelControl>();
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

        // ���� ������ �� ���� Ƚ�� �ʱ�ȭ *
        if (attackTimer >= 0)
        {
            //Debug.Log("attackTimer " + attackTimer);
            attackTimer -= Time.deltaTime;
        } else if (attackTimer > -1)    // Ÿ�̸� ������ �ʱⰪ���� Ŭ ��
        {

            Debug.Log("playerAttack reset");
            gameManager.playerAttack = 4;
            attackTimer = -1;
        }

        // ����
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && !anim.GetBool("isJumping"))
        {
            isAttacking = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // ���� ��ȯ
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);   // Left flip
        } else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);   // Left flip
        }

        // �ȱ� �ִϸ��̼�
        if (rigid.velocity.normalized.x == 0)
            anim.SetBool("isWalking", false);
        else
        {
            anim.SetBool("isWalking", true);
            isAttacking = false;
        }

        // ���� *
        if (Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && gameManager.playerAttack > 0)
        {
            isAttacking = true;
            //Debug.Log("isAttacking " + isAttacking);
            anim.SetTrigger("Attack");
        }
        // �ݰ�
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Z Pressed");
            isInterrupting = true;
            //Debug.Log("�ݰ� �õ�");
            anim.SetTrigger("Attack");
        }

        // ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.LeftAlt) && interactiveObject)
        {
            interactiveObject.Interaction();
        }

        // �÷��̾� ����(�ͳο��� ����ġ �� �÷��� ���� ������)
        if (tunnel.Tun == true && tunnelL.states == true && InterOb.trigger == false)
        {
            GameObject.Find("Player").transform.Find("Player_light").gameObject.SetActive(true);// ���� ������Ʈ Ȱ��ȭ
        }
        else if(tunnelL.states == true)
        {
            playerLight.gameObject.SetActive(false);// ���� ������Ʈ ��Ȱ��ȭ
        }
    }

    // �ַ� ������ �̺�Ʈ.
    private void FixedUpdate()
    {
        // �̵� left, a: -1 / right, d: 1 / �� �����̰ų� ���� �� ���� ��: 0
        float key = Input.GetAxisRaw("Horizontal");
        // x�� �̵��� x * speed��, y�� �̵��� ������ �ӷ� ��(����� �߷�)
        if (gameObject.layer != 9)  // �ǰݽ� �ݵ��� ���ؼ� ������ �� ���� ��
            rigid.velocity = new Vector2(key * speed, rigid.velocity.y);

        // Landing Platform, ���� ĳ��Ʈ
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)  // �÷��̾� ũ���� ����
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
        // ü�� ����
        gameManager.HealthDown();
        if (gameManager.playerHP < 1)
            return;

        // ���̾� ����
        gameObject.layer = 9;   // PlayerDamaged ���̾� (�÷��̾� ���� ���̾�)

        // ��� ����Ʈ
        StartCoroutine(Shake(0.2f, 0.15f));
        damagedBgAlpha = 0.2f * (5 - gameManager.playerHP);
        damagedBg.color = new Color(1, 1, 1, damagedBgAlpha);
        StartCoroutine("reduceDamagedBg");

        // �÷��̾� ���� ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // �ݵ�
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);    // OffDamaged() ȣ�� �ð� ����. ���� ����. ���� �ʿ�.
    }

    private void OffDamaged()
    {
        // �÷��̾� ����Ʈ
        spriteRenderer.color = new Color(1, 1, 1, 1);   // ���� ����
        gameObject.layer = 8;   // ���̾� ����: Player 
    }

    public void OnDie()
    {
        // ���� ����Ʈ
        gameObject.layer = 9;   // PlayerDamaged ���̾�
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("Respawn", 2);   // 2�� �� ������
    }
    
    private void Respawn()
    {
        // �÷��̾� ����ġ
        spriteRenderer.flipX = false;
        gameObject.transform.position = new Vector3(-5, -3, -1);
        gameManager.playerHP = 4;
        gameObject.layer = 8;   // Player ���̾�
        spriteRenderer.color = new Color(1, 1, 1, 1);
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

    private IEnumerator Shake(float amount, float duration)
    {
        cameraOriginPos = Camera.transform.localPosition;
        float timer = 0;
        while (timer <= duration)
        {
            isShaked = true; //true�� cameraMove ���� ī�޶� �÷��̾� ���󰡴� �ڵ� ������Ű��. (cameraMove�� ī�޶� ��ġ ���� �ڵ尡 ���� �ڵ带 �����־ ��鸲�� ����ȵű� ����)
            Camera.transform.localPosition = (Vector3)Random.insideUnitCircle * amount + cameraOriginPos;

            timer += Time.deltaTime;
            yield return null;
        }
        isShaked = false;
        Camera.transform.localPosition = cameraOriginPos;
    }
}
