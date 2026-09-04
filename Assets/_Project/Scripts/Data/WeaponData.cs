using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    
    [Header("Bools")]
    public bool isProjectile;
    public bool isExplosive;
    
    
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public int basePiercing;
    public float baseRange;
    
    [Header("Weapon Stats")]
    public float baseDamage;
    public float baseKnockback;
    public float baseCooldown;

    [Header("Elemental Stats")] 
    public float fireScaling;
    public float waterScaling;
    public float airScaling;
    public float earthScaling;
}