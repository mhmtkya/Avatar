using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{
    public static WeaponInventoryManager Instance;

    [Header("Inventory Settings")] 
    public int maxWeaponSlot = 4;
    public List<WeaponData> equippedWeapons = new List<WeaponData>();

    [Header("References")] public Transform weaponParent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddWeapon(WeaponData newWeapon)
    {
        if (equippedWeapons.Contains(newWeapon))
        {
            return;
        }

        if (equippedWeapons.Count >= maxWeaponSlot)
        {
            return;
        }
        
        equippedWeapons.Add(newWeapon);

        if (newWeapon.weaponPrefab != null)
        {
            GameObject spawnedWeapon = Instantiate(newWeapon.weaponPrefab, weaponParent);
            spawnedWeapon.name = newWeapon.weaponName;
            
        }
    }
    public bool HasEmptySlot()
    {
        return equippedWeapons.Count < maxWeaponSlot;
    }
}
