using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private HashSet<int> hitEnemies = new HashSet<int>();
    
    private IObjectPool<Projectile> myPool;
    private float speed;
    private float damage;
    private float range;
    private float rangeSquared;
    private int piercing;
    private float knockBack;
    private Vector2 origin;
    private Vector2 moveDirection;

    private bool isExplosive;
    private float explosiveRange;
    
    
    // Silah bu mermiyi havuzdan çektiğinde ona ayarlarını vermek için çağıracak
    public void Initialize(IObjectPool<Projectile> pool, Vector2 projDirection,Vector2 projOrigin, float finalDamage, WeaponData data, WeaponData.WeaponLevelStats levelStats )
    {
        myPool = pool;
        moveDirection = projDirection;
        origin = projOrigin;
        damage = finalDamage;
        
        speed = data.projectileSpeed;
        piercing = levelStats.basePiercing;
        knockBack = levelStats.baseKnockback;
        range = levelStats.baseRange;
        rangeSquared = range * range;
        
        
        isExplosive = data.isExplosive;
        explosiveRange = levelStats.explosionRadius;
        
        hitEnemies.Clear();
        
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) *  Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
        
    }

    private void Update()
    {
        // Mermiyi ileri doğru hareket ettir
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        
        if(((Vector2)transform.position - origin).sqrMagnitude > rangeSquared)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // Düşmana çarpma kontrolü burada yapılacak
        if (collision.CompareTag("Enemy"))
        {
            int enemyID = collision.gameObject.GetInstanceID();

            if (!hitEnemies.Contains(enemyID))
            {
                hitEnemies.Add(enemyID);

                if (isExplosive) Explode();
                else
                {
                    EnemyController enemy = collision.GetComponent<EnemyController>();
                    if (enemy != null) enemy.TakeDamage(damage); enemy.ApplyKnockback(transform.position, knockBack);
                }

                piercing--; 
                if (piercing <= 0) ReturnToPool();
            }
        }
    }

    private void Explode()
    {
        Collider2D[] enemiesInRadius = Physics2D.OverlapCircleAll(transform.position, explosiveRange);

        foreach (Collider2D hit in enemiesInRadius)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null) enemy.TakeDamage(damage);
            }
        }
    }

    private void ReturnToPool()
    {
        // Unity'nin Destroy() komutu YERİNE bunu kullanıyoruz:
        myPool.Release(this);
    }
}