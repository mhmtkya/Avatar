using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;

    private float currentHealth;
    private Transform playerTarget;
    private IObjectPool<EnemyController> myPool;

    public void SetPool(IObjectPool<EnemyController> pool)
    {
        myPool = pool;
    }
    
    private void OnEnable()
    {
        currentHealth = enemyData.maxHealth;


        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }
    }
    

    private void Update()
    {
        if (playerTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position,playerTarget.position,enemyData.moveSpeed * Time.deltaTime);
        }
    }

    private float damageCooldown = 0.5f;
    private float lastDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                PlayerStats player = collision.GetComponent<PlayerStats>();
                if (player != null)
                {
                    player.TakeDamage(enemyData.damageToPlayer);
                    lastDamageTime = Time.time;
                }
            }
        }
    }


    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        
        
        if (myPool != null)
            myPool.Release(this);
        else
            Destroy(gameObject);
    }
}
