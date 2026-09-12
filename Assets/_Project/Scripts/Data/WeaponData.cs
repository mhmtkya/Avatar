using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [System.Serializable]
    public struct WeaponLevelStats
    {
        public int basePiercing;
        public float baseRange;
        public float explosionRadius;
    
        [Header("Weapon Stats")]
        public float baseDamage;
        public float baseKnockback;
        public float baseCooldown;

        [Header("Elemental Stats")] 
        public float fireScaling;
        public float waterScaling;
        public float airScaling;
        public float earthScaling;
        
        [TextArea]
        public string levelDescription;
    }
        
    
    
    public string weaponName;
    public GameObject weaponPrefab;
    public Sprite weaponIcon;
    
    public enum WeaponType
    {
        Projectile,
        Melee,
        Aura
    }

    public WeaponType weaponType;
    
    [Header("Bools")]
    public bool isExplosive;
    
    
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

    [Header("Level Stats")] public List<WeaponLevelStats> statsPerLevel;
    
    public int MaxLevel => statsPerLevel.Count;

}