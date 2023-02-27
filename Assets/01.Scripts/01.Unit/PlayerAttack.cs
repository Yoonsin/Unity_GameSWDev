using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� �ݶ��̴� ��ġ: player->bone1->bone2->bone3->bone12->bone52->bone13->bone14->bone55
public class PlayerAttack : MonoBehaviour
{
    public PlayerMove player;

    private EnemyMove Enemy = null;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("playerAttack_TrrigerEnter: Enemy");
            Enemy = other.GetComponent<EnemyMove>();

            // ���� �� �浹 �˸�
            Enemy.touchedSword = true;

            // ����
            if (player.isAttacking == true)
            {
                //Debug.Log("playerAttack_TrrigerEnter: Enemy Attacked");
                player.isAttacking = false;
                if (Enemy.Boss == false)
                    Enemy.OnDamaged();
                else
                    Enemy.BossOnDamaged();
            }

            //Enemy = null;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy = other.GetComponent<EnemyMove>();

            Enemy.touchedSword = false;
            Enemy = null;
        }
    }
}
