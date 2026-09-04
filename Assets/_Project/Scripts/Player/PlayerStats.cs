using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Data Template")]
    [SerializeField] private CharacterData characterData;

    // Çalışma anında anlık statları tuttuğumuz sözlük
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    private float currentHealth;
    
    private void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        currentStats.Clear();

        if (characterData == null)
        {
            Debug.LogError("CharacterData atanmamış!");
            return;
        }

        // ScriptableObject'teki temel statları kopyalıyoruz
        foreach (var stat in characterData.baseStats)
        {
            currentStats[stat.statType] = stat.baseValue;
        }

        currentHealth = GetStat(StatType.MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        
        float currentArmor = GetStat(StatType.Armor);
        float damageMultiplier;
        
        if (currentArmor > 0) damageMultiplier = 100f / (100f + currentArmor); //zırha göre damage multiplier hesabı
        else damageMultiplier = (100f - currentArmor) / 100f;
        
        float finalDamage = damage * damageMultiplier;
        
        currentHealth -= finalDamage;
        Debug.Log($"Oyuncu hasar aldı! Vurulan: {finalDamage} | Kalan Can: {currentHealth}");
        
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        Debug.Log("Bitti");
    }
    
    
    

    // Herhangi bir statın anlık değerini döndürür
    public float GetStat(StatType type)
    {
        if (currentStats.TryGetValue(type, out float value))
        {
            return value;
        }

        return 1f; // Tanımlı değilse varsayılan değer
    }

    // Seviye atlayınca veya sandıktan eşya alınca stat eklemek için:
    public void AddStat(StatType type, float amount)
    {
        if (currentStats.ContainsKey(type))
        {
            currentStats[type] += amount;
        }
        else
        {
            currentStats[type] = amount;
        }
    }
}