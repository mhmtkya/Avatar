using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData; // Inspector'dan silah datasını atayacağımız yer
    public LayerMask enemyLayer;

    public int currentLevel = 1;
    
    private PlayerStats playerStats;
    private float currentCooldown;

    private IObjectPool<Projectile> projectilePool;

    private AuraWeapon activeAura;
    
    private void Start()
    {
        // Oyuncunun üzerindeki statları bul (Silahlar genelde oyuncunun alt objesi (child) olur)
        playerStats = GetComponentInParent<PlayerStats>();
        
        //Havuz
        projectilePool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet:OnGetProjectile,
            actionOnRelease: OnRelaseProjectile,
            actionOnDestroy:OnDestroyProjectile,
            defaultCapacity: 50,
            maxSize: 300
        );

        if (weaponData.weaponType == WeaponData.WeaponType.Aura) SpawnAura();
        else currentCooldown = GetCurrentLevelStats().baseCooldown;
        
    }

    public void LevelUpWeapon()
    {
        if (currentLevel < weaponData.MaxLevel)
        {
            currentLevel++;
            Debug.Log($"🔥 {weaponData.weaponName} güçlendi! Yeni Seviye: {currentLevel}");

        }
        
    }

    public WeaponData.WeaponLevelStats GetCurrentLevelStats()
    {
        int index = Mathf.Clamp(currentLevel -1, 0, weaponData.statsPerLevel.Count - 1);
        return weaponData.statsPerLevel[index];
    }

    private void Update()
    {
        if(weaponData.weaponType == WeaponData.WeaponType.Aura) return;
        
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0f)
        {
            Transform target = GetClosestEnemy();
            
            if(target == null) return;

            switch (weaponData.weaponType)
            {
                case WeaponData.WeaponType.Projectile:
                    FireProjectile(target);
                    break;
                case WeaponData.WeaponType.Melee:
                    HitMelee(target);
                    break;
            }
        }
    }

    private Transform GetClosestEnemy()
    {
        float currentRange = GetCurrentLevelStats().baseRange;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, currentRange, enemyLayer);
        
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in hitEnemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private Transform GetRandomEnemy()
    {
        float currentRange = GetCurrentLevelStats().baseRange;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, currentRange, enemyLayer);
        
        Transform randomEnemy = null;
        
        if (hitEnemies.Length > 0)
        {
            int randomIndex = Random.Range(0, hitEnemies.Length);
            randomEnemy = hitEnemies[randomIndex].transform;
        }

        return randomEnemy;

    }
    
    private void HitMelee(Transform target)
    {
        Vector2 attackDirection = (target.position - transform.position).normalized;
        WeaponData.WeaponLevelStats currentLevelStats = GetCurrentLevelStats();

        float spawnOffset = 1f * (currentLevelStats.baseRange * 0.5f);
        Vector3 spawnPosition = transform.position + (Vector3)(attackDirection*spawnOffset);
        
        float angle = Mathf.Atan2(attackDirection.x, attackDirection.y)  * Mathf.Rad2Deg;
        quaternion rotation = Quaternion.Euler(0,0,angle - 90);
        
        GameObject meleeObj = Instantiate(weaponData.spawnPrefab, spawnPosition, rotation);

        meleeObj.transform.localScale = new Vector3(currentLevelStats.baseRange, currentLevelStats.baseRange, 1);
        MeleeHitbox hitbox = meleeObj.GetComponent<MeleeHitbox>();
        float finalDamage = playerStats.GetStat(StatType.Damage)* (currentLevelStats.baseDamage + (currentLevelStats.fireScaling * playerStats.GetStat(StatType.FirePower)) + (currentLevelStats.airScaling* playerStats.GetStat(StatType.AirPower)) + (currentLevelStats.earthScaling * playerStats.GetStat(StatType.EarthPower)) + (currentLevelStats.waterScaling * playerStats.GetStat(StatType.WaterPower)));

        hitbox.Initialize(finalDamage, currentLevelStats.baseKnockback, transform.position);
        
        ResetCooldown();
    }

    private void FireProjectile(Transform target)
    {
        // 1. Mermiyi oluştur
        Projectile proj = projectilePool.Get();
        proj.transform.position = transform.position;
        
        //Hedef yönü
        Vector2 fireDirection = (target.position - transform.position).normalized;
        
        WeaponData.WeaponLevelStats currentLevelStats = GetCurrentLevelStats();
        
        // 2. Nihai hasarı hesapla: Silahın Taban Hasarı * Oyuncunun Hasar Çarpanı
        float finalDamage = CalculateFinalDamage(playerStats, currentLevelStats);
        proj.Initialize(projectilePool, fireDirection, transform.position, finalDamage, weaponData, currentLevelStats);
        
        ResetCooldown();
    }

    private void SpawnAura()
    {
        GameObject auraObj = Instantiate(weaponData.spawnPrefab, transform.position, Quaternion.identity);
        
        auraObj.transform.SetParent(this.transform);

        activeAura = auraObj.GetComponent<AuraWeapon>();
        
        activeAura.Initialize(this, playerStats, weaponData);

    }

    private float CalculateFinalDamage(PlayerStats currentPlayerStats, WeaponData.WeaponLevelStats currentLevelStats)
    {
        return currentPlayerStats.GetStat(StatType.Damage)* (currentLevelStats.baseDamage + (currentLevelStats.fireScaling * playerStats.GetStat(StatType.FirePower)) + (currentLevelStats.airScaling* playerStats.GetStat(StatType.AirPower)) + (currentLevelStats.earthScaling * playerStats.GetStat(StatType.EarthPower)) + (currentLevelStats.waterScaling * playerStats.GetStat(StatType.WaterPower)));

    }
    
    
    private void ResetCooldown()
    {
        // Oyuncunun saldırı hızı (AttackSpeedMultiplier) ne kadar yüksekse, bekleme süresi o kadar DÜŞER
        float attackSpeed = playerStats.GetStat(StatType.AttackSpeed);
        
        currentCooldown = GetCurrentLevelStats().baseCooldown / attackSpeed;
    }

    private Projectile CreateProjectile()
    {
        GameObject obj = Instantiate(weaponData.spawnPrefab);
        return obj.GetComponent<Projectile>();
    }

    private void OnGetProjectile(Projectile proj)
    {
        proj.gameObject.SetActive(true);
    }

    private void OnRelaseProjectile(Projectile proj)
    {
        proj.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile proj)
    {
        Destroy(proj.gameObject);
    }
}