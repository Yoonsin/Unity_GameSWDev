using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Enemy = other.GetComponent<EnemyMove>();

            // 적과 검 충돌 알림
            Enemy.touchedSword = true;

            // 공격
            if (player.isAttacking == true)
            {
                Debug.Log("Enemy Attacked");
                player.isAttacking = false;
                Enemy.OnDamaged();

            }

            Enemy = null;
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
