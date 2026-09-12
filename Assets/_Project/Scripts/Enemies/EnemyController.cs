using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("Drops")]
    public GameObject chestPrefab;
    
    [Header("Damage Flash Settings")]
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    public Material flashMaterial;
    public float flashDuration = 0.1f;
    
    private Coroutine flashRoutine = null;
    
    public EnemyData enemyData;
    private float currentHealth;
    private Transform playerTarget;
    private IObjectPool<EnemyController> myPool;


    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalMaterial = spriteRenderer.material;
    }

    public void SetPool(IObjectPool<EnemyController> pool)
    {
        myPool = pool;
    }
    
    private void OnEnable()
    {
        if(flashRoutine != null) StopCoroutine(flashRoutine); flashRoutine = null;
        
        if(spriteRenderer != null && originalMaterial!=null) spriteRenderer.material = originalMaterial;

        currentHealth = enemyData.maxHealth;
            
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }
    }
    

    private void Update()
    {
        if (isKnockedBack) return;
        if (playerTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position,playerTarget.position,enemyData.moveSpeed * Time.deltaTime);
            
            transform.localScale = ((transform.position - playerTarget.position).normalized.x < 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
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

    public void ApplyKnockback(Vector2 atackerPos, float knockbackForce)
    {
        if(knockbackForce <= 0 || rb == null ) return;
        
        Vector2 knockbackDirection = ((Vector2)transform.position - atackerPos).normalized;
        
        StopCoroutine("KnockbackRoutine");
        StartCoroutine(KnockbackRoutine(knockbackDirection, knockbackForce));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float knockbackForce)
    {
        isKnockedBack = true;

        float pushDuration = 0.15f;
        float timer = 0;

        while (timer < pushDuration)
        {
            transform.Translate(direction * knockbackForce * (1f- (timer / pushDuration)) *  Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }
        
        isKnockedBack = false;

    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        
        if (spriteRenderer != null)
        {
            if(flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashRoutine());
        }
        
        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRoutine()
    {
        if(flashMaterial != null) spriteRenderer.material = flashMaterial;
        
        yield  return new WaitForSeconds(flashDuration);
        
        spriteRenderer.material = originalMaterial;
        flashRoutine =  null;
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
