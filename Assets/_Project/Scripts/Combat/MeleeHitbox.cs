using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private float knockbackForce;
    private float damage;
    private Vector2 attackerPos;
    private HashSet<int> hitEnemies = new HashSet<int>();

    [Header("Settings")]
    public float lifetime;

    public void Initialize(float meleeDamage, float kbForce, Vector2 playerPos)
    {
        damage = meleeDamage;
        knockbackForce = kbForce;
        attackerPos = playerPos;
        hitEnemies.Clear();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int enemyID = other.gameObject.GetInstanceID();

            if (!hitEnemies.Contains(enemyID))
            {
                hitEnemies.Add(enemyID);

                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null) enemy.TakeDamage(damage); enemy.ApplyKnockback(attackerPos, knockbackForce);
            }
        }
    }
}
