using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 검 콜라이더 위치: player->bone1->bone2->bone3->bone12->bone52->bone13->bone14->bone55
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

            // 적과 검 충돌 알림
            Enemy.touchedSword = true;

            // 공격
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
