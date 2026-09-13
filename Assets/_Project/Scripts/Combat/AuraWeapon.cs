using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AuraWeapon : MonoBehaviour
{
    private float currentCooldown;
    private List<EnemyController> enemiesInAura = new List<EnemyController>();

    private WeaponController parentController;
    private WeaponData weaponData;
    private PlayerStats playerStats;

    public void Initialize(WeaponController controller, PlayerStats stats, WeaponData data)
    {
        parentController = controller;
        playerStats = stats;
        weaponData = data;

        currentCooldown = 0f;

        UpdateAuraRange();
    }
    
    private void Update()
    {
        WeaponData.WeaponLevelStats currentStats = parentController.GetCurrentLevelStats();

        float attackSpeed = playerStats.GetStat(StatType.AttackSpeed);
        float finalCooldown = currentStats.baseCooldown / attackSpeed;

        UpdateAuraRange();
        
        
        if (finalCooldown <= 0)
        {
            ApplyContinuousDamage(currentStats);
        }
        else
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                ApplyPulseDamage(currentStats);
                currentCooldown = finalCooldown;
            }
        }
    }

    private float GetDynamicDamage(WeaponData.WeaponLevelStats currentStats)
    {
        return playerStats.GetStat(StatType.Damage)* (currentStats.baseDamage + (currentStats.fireScaling * playerStats.GetStat(StatType.FirePower)) + (currentStats.airScaling* playerStats.GetStat(StatType.AirPower)) + (currentStats.earthScaling * playerStats.GetStat(StatType.EarthPower)) + (currentStats.waterScaling * playerStats.GetStat(StatType.WaterPower)));
    }

    private void UpdateAuraRange()
    {
        float currentRange = parentController.GetCurrentLevelStats().baseRange;
        transform.localScale = new Vector3(currentRange, currentRange, 1f);
    }
    
    private void ApplyContinuousDamage(WeaponData.WeaponLevelStats currentStats)
    {
        float dynamicDamage = GetDynamicDamage(currentStats);
        
        for (int i = enemiesInAura.Count - 1; i >= 0; i--)
        {
           
            EnemyController enemyController =  enemiesInAura[i];
            if (enemyController != null && enemyController.gameObject.activeInHierarchy)
            {
                enemyController.TakeDamage(dynamicDamage * Time.deltaTime);
            }
            else
            {
                enemiesInAura.RemoveAt(i);
            }
        }
    }

    private void ApplyPulseDamage(WeaponData.WeaponLevelStats  currentStats)
    {
        float dynamicDamage = GetDynamicDamage(currentStats);
        float dynamicKnockback = currentStats.baseKnockback;
        for (int i = enemiesInAura.Count - 1; i >= 0; i--)
        {
            EnemyController enemyController = enemiesInAura[i];
            if (enemyController != null && enemyController.gameObject.activeInHierarchy)
            {
                enemyController.TakeDamage(dynamicDamage);
                enemyController.ApplyKnockback(transform.position, dynamicKnockback);

            }
            else
            {
                enemiesInAura.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            if(enemyController != null && !enemiesInAura.Contains(enemyController)) enemiesInAura.Add(enemyController);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            if(enemyController != null && enemiesInAura.Contains(enemyController)) enemiesInAura.Remove(enemyController);
        }
    }
}
