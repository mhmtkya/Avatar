using System;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [Header("Currency")]
    public int currentGold = 0;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void AddGold(int amount)
    {
        currentGold += Mathf.RoundToInt(amount * playerStats.GetStat(StatType.GoldGainRate)) ;
    }
    
    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }
    
    public void SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            currentGold -= amount;
        }
    }
    
}
