using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Xp and Levels")]
    public int currentLevel = 0;
    public float currentXp = 0;
    public float xpToNextLevel = 5f;

    private PlayerStats playerStats;
    
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void AddXp(int xpToAdd)
    {
        currentXp += xpToAdd * playerStats.GetStat(StatType.ExpGainRate);
        Debug.Log($"XP Toplandı! +{xpToAdd} | Durum: {currentXp} / {xpToNextLevel}");
        
        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = xpToNextLevel * 1.5f;

    }
    
}
