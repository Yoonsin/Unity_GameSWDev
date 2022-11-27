using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMove player;
    public EnemyMove enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && player.isAttacking == true)
        {
            Debug.Log("Enemy Attacked");
            player.isAttacking = false;
            enemy.OnDamaged();
        }
    }
}
