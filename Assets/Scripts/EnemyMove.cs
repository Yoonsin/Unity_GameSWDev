using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    CapsuleCollider2D enemyCollider;

    int enemyHP = 5;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log("Enemy Attacked");
            OnDamaged();
        }
    }

    public void OnDamaged()
    {
        enemyHP--;
        Debug.Log("Enemy HP: " + enemyHP);
        if (enemyHP < 1)
            OnDie();
    }

    private void OnDie()
    {
        Debug.Log("Enemy Dead!");
        enemyHP = 5;
    }
}
