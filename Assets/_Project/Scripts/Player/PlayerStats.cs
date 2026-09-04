using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Data Template")]
    [SerializeField] private CharacterData characterData;

    // Çalışma anında anlık statları tuttuğumuz sözlük
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

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