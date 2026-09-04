using UnityEngine;
using UnityEngine.Pool;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData; // Inspector'dan silah datasını atayacağımız yer
    public LayerMask enemyLayer;
    
    private PlayerStats playerStats;
    private float currentCooldown;

    private IObjectPool<Projectile> projectilePool; 
    
    private void Start()
    {
        // Oyuncunun üzerindeki statları bul (Silahlar genelde oyuncunun alt objesi (child) olur)
        playerStats = GetComponentInParent<PlayerStats>();
        currentCooldown = weaponData.baseCooldown;
        
        //Havuz
        projectilePool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet:OnGetProjectile,
            actionOnRelease: OnRelaseProjectile,
            actionOnDestroy:OnDestroyProjectile,
            defaultCapacity: 50,
            maxSize: 300
        );
    }

    private void Update()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0f)
        {
            Transform target = GetClosestEnemy();
            if (weaponData.isProjectile)
            {
                if (target != null)
                    FireProjectile(target);
            }
            else
                HitMelee();
            
            
            
        }
    }

    private Transform GetClosestEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, weaponData.baseRange, enemyLayer);
        
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
    
    private void HitMelee()
    {
        ResetCooldown();
    }

    private void FireProjectile(Transform target)
    {
        // 1. Mermiyi oluştur
        Projectile proj = projectilePool.Get();
        proj.transform.position = transform.position;
        
        //Hedef yönü
        Vector2 fireDirection = (target.position - transform.position).normalized;
        
        // 2. Nihai hasarı hesapla: Silahın Taban Hasarı * Oyuncunun Hasar Çarpanı
        float finalDamage = playerStats.GetStat(StatType.Damage)* (weaponData.baseDamage + (weaponData.fireScaling * playerStats.GetStat(StatType.FirePower)) + (weaponData.airScaling* playerStats.GetStat(StatType.AirPower)) + (weaponData.earthScaling * playerStats.GetStat(StatType.EarthPower)) + (weaponData.waterScaling * playerStats.GetStat(StatType.WaterPower)));
        
        proj.Initialize(projectilePool, weaponData.projectileSpeed, fireDirection, finalDamage, weaponData.baseRange, weaponData.basePiercing, transform.position);
        
        ResetCooldown();
    }
    
    private void ResetCooldown()
    {
        // Oyuncunun saldırı hızı (AttackSpeedMultiplier) ne kadar yüksekse, bekleme süresi o kadar DÜŞER
        float attackSpeed = playerStats.GetStat(StatType.AttackSpeed);
        currentCooldown = weaponData.baseCooldown / attackSpeed;
    }

    private Projectile CreateProjectile()
    {
        GameObject obj = Instantiate(weaponData.projectilePrefab);
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