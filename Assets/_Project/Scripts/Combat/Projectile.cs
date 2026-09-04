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
    private int piercing;
    private Vector2 origin;
    private Vector2 moveDirection;
    
    
    // Silah bu mermiyi havuzdan çektiğinde ona ayarlarını vermek için çağıracak
    public void Initialize(IObjectPool<Projectile> pool, float projSpeed, Vector2 projDirection, float projDamage, float projRange, int projPiercing, Vector2 projOrigin)
    {
        myPool = pool;
        speed = projSpeed;
        damage = projDamage;
        range = projRange;
        origin = projOrigin;
        moveDirection = projDirection;
        piercing = projPiercing;
        hitEnemies.Clear();
    }

    private void Update()
    {
        // Mermiyi ileri doğru hareket ettir
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, origin) > range)
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

                EnemyController enemy = collision.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                
                piercing--; 
                if (piercing <= 0)
                {
                    ReturnToPool();
                }
            }
        }
    }

    private void ReturnToPool()
    {
        // Unity'nin Destroy() komutu YERİNE bunu kullanıyoruz:
        myPool.Release(this);
    }
}