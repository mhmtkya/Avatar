using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [Header("Drops")]
    public GameObject chestPrefab;
    
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
        //sandık şans hesaplama
        float finalChestChance = enemyData.chestDropChance;

        if (playerTarget != null)
        {
            PlayerStats playerstats = playerTarget.GetComponent<PlayerStats>();
            if (playerstats != null)
                finalChestChance *= playerstats.GetStat(StatType.Luck);
        }

        //sandık
        if (Random.value <= finalChestChance && chestPrefab != null)
        {
            Instantiate(chestPrefab, transform.position, Quaternion.identity);
        }
        
        //gold
        if (LootPoolManager.Instance != null)
        {
            Vector3 goldOffset = (Vector3)Random.insideUnitCircle * 0.3f;
            
            GameObject goldObj = LootPoolManager.Instance.goldGemPool.Get();
            goldObj.transform.position = transform.position + goldOffset;
            
            GoldPickup goldPickup = goldObj.GetComponent<GoldPickup>();
            if (goldPickup != null) goldPickup.goldAmount = enemyData.goldDropAmount;
        }
        
        //xp
        if (LootPoolManager.Instance != null)
        {
            Vector3 xpOffset = (Vector3)Random.insideUnitCircle * 0.3f;
            
            GameObject xpObj = LootPoolManager.Instance.xpGemPool.Get();
            xpObj.transform.position = transform.position + xpOffset;
                
            XpPickup xpPickup = xpObj.GetComponent<XpPickup>();
            if (xpPickup != null) xpPickup.xpAmount = enemyData.xpDropAmount;
        }
        
        //Havuza Dönme
        if (myPool != null) myPool.Release(this);
        else Destroy(gameObject);
    }
}
