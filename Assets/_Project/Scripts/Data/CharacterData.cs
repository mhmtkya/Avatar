using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;
    public GameObject characterPrefab;
    
    public List<CharacterStat> baseStats = new List<CharacterStat>();
   
    // Unity'de Project penceresinde sağ tıklayıp "Create -> Game Data -> Character"
    // dediğin ANDA bu fonksiyon kendiliğinden tetiklenir.
    private void Reset()
    {
        baseStats.Clear();

        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            baseStats.Add(new CharacterStat
            {
                statType = type,
                baseValue = 1f
            });
        }
    }
    
    
    public float GetBaseStats(StatType type, float defaultValue = 1f)
    {
        foreach (var stat in baseStats)
        {
            if(stat.statType == type)
                return stat.baseValue;
        }        
        return defaultValue;
    }
}
